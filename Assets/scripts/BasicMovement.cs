using UnityEngine;
using System;


public class BasicMovement : MonoBehaviour
{
    public BrackeysMovement bm;
    public Vector3 spawnPoint;

    public float defaultTopSpeed;
    public float climbSpeed;
    public float maxJumpForce;
    public float defaultDecay;
    
    public Transform gripCheck;
    public Transform groundCheck;
    public float gripCheckRadius;
    public float groundCheckRadius;
    public LayerMask whatIsGrippable;
    public LayerMask whatIsDeadly;
    public LayerMask whatIsPlatform;
    [SerializeField] public LayerMask whatIsGround;
    
    public float topHSpeed;
    public float decayRate;

    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip grabVine;
    public AudioClip letgoVine;
    public AudioClip stepA;
    public AudioClip stepB;

    float coolTopSpeed;
    float coldTopSpeed;
    float coolDecay;
    float coldDecay;

    float defaultGravity;
    float h;
    float v;
    float jumpForce;
    private bool gripping;
    private bool grounded;
    private bool jumping = false;
    private AudioSource source;
    Rigidbody2D rigid;


    void Start()
    {
        coolDecay = defaultDecay * 1.15f;
        coldDecay = defaultDecay * 1.35f;
        coolTopSpeed = defaultTopSpeed * 0.85f;
        coldTopSpeed = defaultTopSpeed * 0.65f;
        changePlayerStats();

        rigid = GetComponent<Rigidbody2D>();
        defaultGravity = rigid.gravityScale;

        source = GetComponent<AudioSource>();

        if (PersistentManager.Instance.lastDoorId == "Respawn")
        {
            Debug.Log("respawning");
            GameObject checkpoint = GameObject.FindWithTag("checkpoint");
            if (checkpoint == null)
            {
                checkpoint = GameObject.FindWithTag("hub");
            }
            this.transform.position = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + 0.55f);
            CheckpointActivity ca = checkpoint.GetComponent(typeof (CheckpointActivity)) as CheckpointActivity;
            Debug.Log("calling spawn anim"+ca);
            ca.spawn(this.transform.GetChild(0).gameObject);
            return;
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
                    this.transform.position = spawnPoint;
                    od.spawn(this.transform.GetChild(0).gameObject);
                    Debug.Log("spawned?");
                    break;
                }
            }
            Debug.Log("AAAAAAAAAAAAAAAAAAA"+spawnPoint);
            if(spawnPoint == Vector3.zero) {
                Debug.Log("ERRRORRRR!!!!");
                OpenDoor od = doors[0].GetComponent(typeof (OpenDoor)) as OpenDoor;
                this.transform.position = new Vector3(doors[0].transform.position.x - od.flipped * 1.0f, doors[0].transform.position.y - 0.7f, 0);
                if(od.flipped == 1)
                {
                    bm.Flip();
                }
            }
        }
    }

    void Update()
    {
        if(PersistentManager.Instance.immobile) { return; }

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

        if (Input.GetButtonDown("Jump") && (grounded || gripping))
        {
            jumpForce = maxJumpForce;
            playJumpSound();
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
                {
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
        //checkDeath();

        checkMove();

        checkJump();
    }

    // void checkDeath()
    // {
    //     Collider2D[] deadlyColliders = Physics2D.OverlapCircleAll(gripCheck.position, gripCheckRadius, whatIsDeadly);
    //     for (int i = 0; i < deadlyColliders.Length; i++)
    //     {
    //         if (deadlyColliders[i].gameObject != gameObject)
    //         {
    //             death();
    //         }
    //     }
    // }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if((whatIsDeadly.value & 1<<coll.gameObject.layer) != 0)
        {
            death();
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
                
                 if (!wasGrounded)
                 {
                    playLandSound();
                }
            }
        }
    }

    

    void checkJump()
    {
        if(jumping)
        {
            bm.VerticalMove(jumpForce * Time.fixedDeltaTime);
            jumpForce = jumpForce - decayRate * Time.fixedDeltaTime;
            if(gripping)
            {
                gripOff();
            }
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
            walksound(h);
        }
        if (moveY)
        {
            bm.VerticalMove(v * Time.fixedDeltaTime);
        }
    }

    void walksound(float walking)
    {
        source.clip = stepA;
        if (source.isPlaying != true)
        {
            if (walking != 0 ){ source.Play(); }
            else { source.Stop(); }
        }
        
    }
    

    void death() 
    {
        PersistentManager.Instance.GameOver.SetActive(true);
    }

    void gripOff()
    {
        Debug.Log("Let go");
        gripping = false;
        source.PlayOneShot(letgoVine);
        rigid.gravityScale = defaultGravity;
    }
    void gripOn()
    {
        Debug.Log("grab");
        gripping = true;
        source.PlayOneShot(grabVine);
        rigid.gravityScale = 0f;                    
        rigid.velocity = new Vector2(0, 0);
        jumping = false;
        jumpForce = 0;
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

    public void changePlayerStats()
    {
        int currTemp = PersistentManager.Instance.tempLevel;
        if(currTemp == -2)
        {
            topHSpeed = coldTopSpeed;
            decayRate = coldDecay;
        }
        else if(currTemp == -1)
        {
            topHSpeed = coolTopSpeed;
            decayRate = coolDecay;
        }
        else
        {
            topHSpeed = defaultTopSpeed;
            decayRate = defaultDecay;
        }
    }

    void playJumpSound()
    {
        source.PlayOneShot(jumpSound);
    }

    void playLandSound()
    {
        source.PlayOneShot(landSound);
    }


}