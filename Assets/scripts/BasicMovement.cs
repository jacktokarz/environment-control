﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class BasicMovement : MonoBehaviour
{
    public BrackeysMovement bm;
    public PlayerAudio pa;
    public Vector3 spawnPoint;

    public float defaultGravity;
    public float defaultTopSpeed;
    public float climbSpeed;
    public float maxJumpForce;
    public float defaultDecay;
    public float maxFallSpeed;
    private float fastestFallSpeed;
    
    public Transform gripCheck;
    public Transform groundCheck;
    public float gripCheckRadius;
    public float groundCheckRadius;
    private float gripTimeWindow;
    private float gripTime;
    public LayerMask whatIsGrippable;
    public LayerMask whatIsDeadly;
    public LayerMask whatIsPlatform;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public LayerMask whatIsWater;
    
    private float topHSpeed;
    private float decayRate;
    float coolTopSpeed;
    float coldTopSpeed;
    float coolDecay;
    float coldDecay;
    float timeStartedBurning;
    Color startingColor;
    Color burnedColor;
    public float heatTolerance;

    float h;
    float v;
    float jumpForce;
    private int lostGripCount = 0;
    [HideInInspector] public bool gripping = false;
    [HideInInspector] public bool grounded = true;
    [HideInInspector] public bool underwater = false;
    private bool jumping = false;

    Rigidbody2D rigid;
    GameObject bodyObject;
    Animator bodyAnimator;
    SpriteRenderer bodySprite;

    void Awake()
    {
        bodyObject = this.transform.GetChild(0).gameObject;
        bodyObject.SetActive(false);
        bodyAnimator = bodyObject.GetComponent(typeof (Animator)) as Animator;
        bodySprite = bodyObject.GetComponent(typeof (SpriteRenderer)) as SpriteRenderer;
        startingColor = Color.white;
        burnedColor = new Color(100, 0, 0);
    }

    void Start()
    {
        PersistentManager.Instance.immobile = true;
        bool difficult = PersistentManager.Instance.difficulty=="challenging"; //otherwise, "relaxed"
        Debug.Log("is it difficult? "+difficult);
        gripTimeWindow = difficult ? 0.15f : 0.2f;
        defaultGravity = difficult ? 5f : 2.5f;
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = defaultGravity;
        defaultTopSpeed = difficult ? 40f : 32f;
        climbSpeed = difficult ? 20f : 18f;
        maxJumpForce = difficult ? 100f : 78f;
        defaultDecay = difficult ? 200f : 130f;
        coolDecay = defaultDecay * 1.15f;
        coldDecay = defaultDecay * 1.35f;
        coolTopSpeed = defaultTopSpeed * 0.85f;
        coldTopSpeed = defaultTopSpeed * 0.65f;
        changePlayerStats();

        if (PersistentManager.Instance.lastDoorId == "NewGame")
        {
            GameObject checkpoint = GameObject.FindWithTag("hub");
            this.transform.position = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y);
            CheckpointActivity ca = checkpoint.GetComponent(typeof (CheckpointActivity)) as CheckpointActivity;
            StartCoroutine(stallHubSpawn(ca, PersistentManager.Instance.openingCutsceneLength));
            return;
        }
        if (PersistentManager.Instance.lastDoorId == "Respawn")
        {
            if (!PersistentManager.Instance.Checkpoints.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                SceneManager.LoadScene(PersistentManager.Instance.lastCheckpoint, LoadSceneMode.Single);
                return;
            }
            GameObject checkpoint = GameObject.FindWithTag("checkpoint");
            if (checkpoint == null)
            {
                checkpoint = GameObject.FindWithTag("hub");
            }
            this.transform.position = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + 0.55f);
            CheckpointActivity ca = checkpoint.GetComponent(typeof (CheckpointActivity)) as CheckpointActivity;
            ca.spawn();
            return;
        }
        else {
            GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
            foreach(GameObject door in doors)
            {
                OpenDoor od = door.GetComponent(typeof (OpenDoor)) as OpenDoor;
                if(od.doorId == PersistentManager.Instance.lastDoorId)
                {
                    spawnPoint = new Vector3(door.transform.position.x + od.flipped, door.transform.position.y - 0.7f, 0);
                    if(od.flipped == -1)
                    {
                        bm.Flip();
                    }
                    this.transform.position = spawnPoint;
                    od.spawn(bodyObject);
                    break;
                }
            }
            if(spawnPoint == Vector3.zero) {
                Debug.Log("ERRRORRRR!!!!");
                OpenDoor od = doors[0].GetComponent(typeof (OpenDoor)) as OpenDoor;
                this.transform.position = new Vector3(doors[0].transform.position.x + od.flipped, doors[0].transform.position.y - 0.7f, 0);
                if(od.flipped == 1)
                {
                    bm.Flip();
                }
                od.spawn(bodyObject);                
            }
        }
    }

    void Update()
    {
        foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
             if(Input.GetKeyDown(vKey)){
                // Debug.Log("input "+vKey);
            }
        }

        checkGrounded();
        if(PersistentManager.Instance.immobile) { return; }
        checkUnderwater();

        h = gripping ? Input.GetAxis("Horizontal") * climbSpeed : Input.GetAxis("Horizontal") * topHSpeed;
        bodyAnimator.SetBool("walking", h!=0);
        checkFlip();

        if (gripping && !(Input.GetKey(PersistentManager.Instance.HumidityKey) || Input.GetKey("2")))
        {
            v = Input.GetAxis("Vertical") * climbSpeed;
        }
        else {
            v = 0;
        }

        if (Input.GetKeyDown(PersistentManager.Instance.JumpKey) && (grounded || gripping))
        {
            jumpForce = maxJumpForce;
            pa.PlayJumpSound();
        }
        if (Input.GetKeyUp(PersistentManager.Instance.JumpKey))
        {
            jumpForce = 0;
        }
        
        if (Input.GetKey(PersistentManager.Instance.JumpKey) && (jumpForce > 0.05f))
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
                h += groundRig.velocity.x * 4f;
            }
        }

        CheckGrip();
        
        if (PersistentManager.Instance.tempLevel >= 2)
        {
            if (!underwater)
            {
                if (timeStartedBurning <= 0)
                {
                    timeStartedBurning = Time.time;
                }
                float timeSinceStarted = Time.time - timeStartedBurning;
                float percentageComplete = timeSinceStarted / heatTolerance - heatTolerance/10;

                bodySprite.color = Color.Lerp(startingColor, burnedColor, percentageComplete);

                if (bodySprite.color == burnedColor)
                {
                    timeStartedBurning = 0;
                    death();
                }
            }
        }
        else if (timeStartedBurning > 0)
        {
            timeStartedBurning = Time.time*-1;
        }
        else if (timeStartedBurning != 0)
        {
            CoolOffSprite();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(!PersistentManager.Instance.immobile)
        {
            checkMove();
            checkJump();
        }
        if (gripping) {
            bodyAnimator.SetFloat("climbSpeed", Mathf.Abs(h + v) > 0 ? 1 : 0);
        }
        else {
            bodyAnimator.SetFloat("velocity", Math.Abs(rigid.velocity.x));
        }
        if (rigid.velocity.y < maxFallSpeed) { rigid.velocity = new Vector2(rigid.velocity.x, maxFallSpeed); }

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
    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.CompareTag("checkpoint") || col.CompareTag("hub"))
    	{
    		CheckpointActivity ca = col.GetComponent<CheckpointActivity>();
            if (!ca.alreadyChecked) {
				bodyAnimator.SetBool("activating", true);
            }
    	}
    }

    void checkGrounded()
    {
        bool wasGrounded = grounded;
        grounded = false;
        bodyAnimator.SetBool("grounded", false);
        if (rigid.velocity.y < fastestFallSpeed) { fastestFallSpeed = rigid.velocity.y; }
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] groundColls = GetGroundColliders();
        for (int i = 0; i < groundColls.Length; i++)
        {
            if (groundColls[i].gameObject != gameObject)
            {
                grounded = true;
                bodyAnimator.SetBool("grounded", true);
                
                 if (!wasGrounded)
                 {
                    pa.PlayLandSound(fastestFallSpeed);
                    fastestFallSpeed = 0;
                }
            }
        }
    }

    public Collider2D[] GetGroundColliders() 
    {
        return Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    void checkUnderwater()
    {
        bool wasUnderwater = underwater;
        underwater = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] waterColls = Physics2D.OverlapCircleAll(this.transform.position, groundCheckRadius, whatIsWater);
        for (int i = 0; i < waterColls.Length; i++)
        {
            if (waterColls[i].gameObject != gameObject)
            {
                underwater = true;
                if (!wasUnderwater)
                {
                    timeStartedBurning = Time.time*-1;
                    //playSplashSound();
                    topHSpeed = defaultTopSpeed * 0.85f;
                    rigid.drag = 10;
                    rigid.gravityScale = defaultGravity/2;
                }
                if (timeStartedBurning != 0)
                {
                    CoolOffSprite();
                }
            }
        }
        if (wasUnderwater && !underwater)
        {
            topHSpeed = defaultTopSpeed;
            rigid.drag = 0;
            rigid.gravityScale = defaultGravity;
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
                bodyAnimator.SetTrigger("JumpOff");
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
        bool moveX = true;
        bool moveY = false;
        if (gripping)
        {
            moveY = true;
            Vector3 newXPos = gripCheck.position + bm.predictMove(new Vector3(h * Time.fixedDeltaTime, 0));
            Collider2D[] grippableColliders = Physics2D.OverlapCircleAll(newXPos, gripCheckRadius, whatIsGrippable);
            //Debug.Log("grippable colls 1 - "+grippableColliders.Length);
            if (grippableColliders.Length == 0)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                moveX = false;        
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
        if (moveX)
        {
            bm.Move(h * Time.fixedDeltaTime);
        }
        if (moveY)
        {
            bm.VerticalMove(v * Time.fixedDeltaTime);
        }
    }

    void CheckGrip()
    {
        if(gripping && Input.GetKeyDown(PersistentManager.Instance.GrabKey))
        {       // if they were already gripping, they let go now.
            bodyAnimator.SetTrigger("LetGo");
            gripOff();
            return;
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
        if(Input.GetKeyDown(PersistentManager.Instance.GrabKey))
        {
            gripTime = Time.time + gripTimeWindow;
        }

            // if they can grip
        if(canGrip) {
            lostGripCount = 0;
                // and press the button, they do it
            if (gripTime > Time.time && gripping == false)
            {
                gripOn();
            }
        }
            // if they cannot grip
        else
        {
                //but they are...
            if (gripping == true)
            {
                if (lostGripCount > 10)
                {
                    bodyAnimator.SetTrigger("LetGo");
                    lostGripCount = 0;
                    gripOff();
                }
                else
                {
                    lostGripCount++;
                    h = h*-0.02f;
                    v = v*-0.02f;
                }
            }
        }
    }

    void death() 
    {
        if (!PersistentManager.Instance.immobile)
        {
            rigid.velocity= new Vector2(0,0);
            PersistentManager.Instance.immobile = true;
            bodyAnimator.SetBool("dead", true);
            PersistentManager.Instance.GameOver.SetActive(true);
            pa.PlayDeathMelody();
        }
    }

    void gripOff()
    {
        gripping = false;
        bodyAnimator.SetBool("gripping", false);
        pa.PlayLetGo();
        rigid.gravityScale = defaultGravity;
    }
    void gripOn()
    {
        gripping = true;
        bodyAnimator.SetBool("gripping", true);
        pa.PlayGrabVine();
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

    void CoolOffSprite()
    {
        if (bodySprite.color == startingColor)
        {
            timeStartedBurning = 0;
        }
        else 
        {
            bodySprite.color = Color.Lerp(bodySprite.color, startingColor,
                (Time.time + timeStartedBurning) / 6);
        }
    }

    IEnumerator stallHubSpawn(CheckpointActivity ca, float sec)
    {
        yield return new WaitForSeconds(sec);
        ca.spawn();
    }

}