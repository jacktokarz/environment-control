using UnityEngine;
using System;


public class BasicMovement : MonoBehaviour
{
    public BrackeysMovement bm;
    public EnvironmentChange envChange;
    public Vector3 spawnPoint;

    public float topHSpeed;
    public float climbSpeed;
    public float maxJumpForce;
    public float decayRate;
    
    public Transform gripCheck;
    public Transform groundCheck;
    public float gripCheckRadius;
    public float groundCheckRadius;
    public LayerMask whatIsGrippable;
    public LayerMask whatIsDeadly;
    public LayerMask whatIsPlatform;
    [SerializeField] public LayerMask whatIsGround;
    
    
    float defaultGravity;
    float h;
    float v;
    float jumpForce;
    private bool gripping;
    private bool grounded;
    private bool jumping = false;

    Rigidbody2D rigid;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        defaultGravity = rigid.gravityScale;

        if (PersistentManager.Instance.lastDoorId == "Respawn")
        {
            GameObject checkpoint = GameObject.FindWithTag("checkpoint");
            if (checkpoint == null)
            {
                checkpoint = GameObject.FindWithTag("hub");
            }
            spawnPoint = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + 0.55f);
        }
        else {
            GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
            foreach(GameObject door in doors)
            {
                OpenDoor od = door.GetComponent(typeof (OpenDoor)) as OpenDoor;
                if(od.doorId == PersistentManager.Instance.lastDoorId)
                {
                    spawnPoint = new Vector3(door.transform.position.x - od.flipped * 1.0f, door.transform.position.y - 0.7f, 0);
                    if(od.flipped == 1)
                    {
                        bm.Flip();
                    }
                }
            }
            if (spawnPoint==null)
            {
                Debug.Log("ERRRORRRR!!!!");
                OpenDoor od = doors[0].GetComponent(typeof (OpenDoor)) as OpenDoor;
                spawnPoint = new Vector3(doors[0].transform.position.x - od.flipped * 1.0f, doors[0].transform.position.y - 0.7f, 0);
                if(od.flipped == 1)
                {
                    bm.Flip();
                }
            }
        }
        this.transform.position = spawnPoint;
    }

    void Update()
    {
        h = gripping ? Input.GetAxis("Horizontal") * climbSpeed : Input.GetAxis("Horizontal") * topHSpeed;
        
        checkFlip();

        if (gripping && !(Input.GetKey("1") || Input.GetKey("2")))
        {
            v = Input.GetAxis("Vertical") * climbSpeed;
        }
        else {
            v = 0;
        }

        checkGrounded();

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumpForce = maxJumpForce;
        }
        
        if (Input.GetButton("Jump") && (jumpForce > 0.05f))
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }
            //if on a lily pad, add that velocity too
        Collider2D platCollider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsPlatform);
        if (platCollider != null)
        {
            Rigidbody2D groundRig = platCollider.gameObject.GetComponent<Rigidbody2D>();
            if (groundRig != null)
            {
                h += groundRig.velocity.x * Mathf.Clamp(PersistentManager.Instance.windLevel, 1, 3);
            }
        }

        bool canGrip= false;
        Collider2D[] grippableColliders = Physics2D.OverlapCircleAll(gripCheck.position, gripCheckRadius, whatIsGrippable);
        for (int i = 0; i < grippableColliders.Length; i++)
        {
            if (grippableColliders[i].gameObject != gameObject)
            {
                canGrip= true;
            }
        }
            // if they can grip
        if(canGrip) {
                // and press the button, they do it
            if (Input.GetButtonDown("Grip"))
            {
                if (gripping == false)
                {   // which makes them stop moving
                    Debug.Log("STOP!");
                    rigid.velocity = new Vector2(0, 0);
                    gripOn();
                }   
                else
                {       // if they were already gripping, they let go now.
                    gripOff();
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkDeath();

        checkMove();

        checkJump();
    }

    void checkDeath()
    {
        Collider2D[] deadlyColliders = Physics2D.OverlapCircleAll(gripCheck.position, gripCheckRadius, whatIsDeadly);
        for (int i = 0; i < deadlyColliders.Length; i++)
        {
            if (deadlyColliders[i].gameObject != gameObject)
            {
                death();
            }
        }
    }

    void checkGrounded()
    {
        bool wasGrounded = grounded;
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] groundColls = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < groundColls.Length; i++)
        {
            if (groundColls[i].gameObject != gameObject)
            {
                grounded = true;
                // if (!wasGrounded)
                // {
                //     OnLandEvent.Invoke();
                // }
            }
        }
    }

    void checkJump()
    {
        if(jumping)
        {
            Debug.Log("jumping");
            bm.VerticalMove(jumpForce * Time.fixedDeltaTime);
            jumpForce = jumpForce - decayRate * Time.fixedDeltaTime;
            gripOff();
        }
        else
        {
            jumping = false;
        }
    }

    void checkMove()
    {
        bool moveH = true;
        bool moveY = false;
        if (gripping)
        {
            moveY = true;
            Vector3 newHPos = gripCheck.position + bm.predictMove(new Vector3(h * Time.fixedDeltaTime, 0));
            Collider2D[] grippableColliders = Physics2D.OverlapCircleAll(newHPos, gripCheckRadius, whatIsGrippable);
            //Debug.Log("grippable colls 1 - "+grippableColliders.Length);
            if (grippableColliders.Length == 0)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                moveH = false;        
            }
            Vector3 newYPos = gripCheck.position + bm.predictMove(new Vector3(0, v * Time.fixedDeltaTime));
            grippableColliders = Physics2D.OverlapCircleAll(newYPos, gripCheckRadius, whatIsGrippable);
            //Debug.Log("grippable colls 2 - "+grippableColliders.Length);
            if (grippableColliders.Length == 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                moveY = false;
            }            
        }
        if (moveH)
        {
            bm.Move(h * Time.fixedDeltaTime);
        }
        if (moveY)
        {
            bm.VerticalMove(v * Time.fixedDeltaTime);
        }
    }

    void death() {
        PersistentManager.Instance.lastDoorId = "Respawn";
        PersistentManager.GoToScene(PersistentManager.Instance.lastCheckpoint);
    }

    void gripOff()
    {
        gripping = false;
        rigid.gravityScale = defaultGravity;
    }
    void gripOn()
    {
        gripping = true;
        rigid.gravityScale = 0f;
    }

    void checkFlip()
    {
        if (h > 0 && !bm.m_FacingRight)
        {
            // ... flip the player.
            bm.Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (h < 0 && bm.m_FacingRight)
        {
            // ... flip the player.
            bm.Flip();
        }
    }

}