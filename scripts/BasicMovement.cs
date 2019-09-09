using UnityEngine;
using System;


public class BasicMovement : MonoBehaviour
{
    public BrackeysMovement bm;
    public EnvironmentChange envChange;
    public Vector3 spawnPoint;

    public float topHSpeed;
    public float climbSpeed;
    
    public Transform gripCheck;
    public Transform groundCheck;
    public float gripCheckRadius;
    public LayerMask whatIsGrippable;
    public LayerMask whatIsDeadly;
    public LayerMask whatIsPlatform;

    float defaultGravity;
    float h;
    float v;
    bool gripping;
    bool jumping = false;

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

        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] deadlyColliders = Physics2D.OverlapCircleAll(gripCheck.position, gripCheckRadius, whatIsDeadly);
        for (int i = 0; i < deadlyColliders.Length; i++)
        {
            if (deadlyColliders[i].gameObject != gameObject)
            {
                death();
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
        if(canGrip) {
            if (Input.GetButton("Grip"))
            {
                if (gripping == false)
                {
                    rigid.velocity = new Vector2(0, 0);
                }
                gripOn();
            }
        }
        else {
            if(gripping) {
                gripOff();
            }
        }

        if (gripping && !jumping)
        {
            bm.VerticalMove(v * Time.fixedDeltaTime);
        }
        bm.Move(h * Time.fixedDeltaTime);

        if(jumping) {
            Debug.Log("gripping? "+gripping);
            bool jumped= bm.Jump(gripping);
            Debug.Log("so they jumped? "+jumped);
            if (jumped) {
                gripOff();
            }
            jumping = false;
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