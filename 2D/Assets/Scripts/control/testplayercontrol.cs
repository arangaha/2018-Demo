using UnityEngine;
using System.Collections;


public class testplayercontrol : unitControl {

    [SerializeField] bool jump = false;               // Condition for whether the player should jump
    [SerializeField] protected float jumpForce = 1000f;         // Amount of force added when the player jumps.
    [SerializeField] protected float dashForce = 20f;

    public bool iniDash = false;
    public bool dashing = false;
    public bool dashRight = true;

    public GameObject NA2h1;
    public GameObject NA2hInstance;
    public float cliplength;
    public bool canReNA1 = false;
    public bool canNA2 = false;
    public bool canNA3 = false;
    public bool canNA4 = false;
    private bool iniAttack1 = false;
    public bool iniAttack2 = false;
    public bool iniAttack3 = false;
    private bool iniAttack4 = false;
    public bool inNA = false;

    public bool jumping = false;
    public float jumpclipcount = 0.75f;
    public float jumpcounter = 0f;
    public bool canJump = false;
    public float nextNA = 1;
    public bool collidingWithEnemy = false;


    bool fire = false;
    bool ice = false;
    bool lightning = false;
    //bodyparts
    public GameObject bicepfront;
    public GameObject bicepback;
    public GameObject forearmfront;
    public GameObject forearmback;
    public GameObject thighfront;
    public GameObject thighback;
    public GameObject handfront;
    public GameObject handback;
    public GameObject body;
    public GameObject waist;
    public GameObject belt;
    public GameObject head;
    public GameObject calffront;
    public GameObject calfback;
    public GameObject twohandweapon;
    public GameObject shoulderbackback;
    public GameObject shoulderback;
    public GameObject shoulderfront;
    public GameObject feetfront;
    public GameObject feetback;
    public int layerMultiplier = 3;

    //for delay before n-a/smash chain stops
    public bool attackdelay = false;
    public float baseDelayTimer = 1f;
    public float delayTimer;

    //for delay between attacks
    public float baseAttackPause = 0;
    public float attackPause;
    public bool pauseDelay = false;


    //for delay that allows dash normal attack
    public float dashAttackTimer = 1f;
    public bool startDashDelay = false;

    protected override void Awake()
    {
        // Setting up references.
        base.Awake();
        NA2h1 = Resources.Load("FX/2hNA1") as GameObject;
        bicepback = transform.Find("player bicep back").gameObject;
        bicepfront = transform.Find("player bicep front").gameObject;
        forearmback = transform.Find("player forearm back").gameObject;
        forearmfront = transform.Find("player forearm front").gameObject;
        thighback = transform.Find("player thigh back").gameObject;
        thighfront = transform.Find("player thigh front").gameObject;
        forearmfront = transform.Find("player forearm front").gameObject;
        handback = transform.Find("player hand back").gameObject;
        handfront = transform.Find("player hand front").gameObject;
        feetback = transform.Find("player feet back").gameObject;
        feetfront = transform.Find("player feet front").gameObject;
        body = transform.Find("player body").gameObject;
        waist = transform.Find("player body").transform.Find("player waist").gameObject;
        belt = transform.Find("player body").transform.Find("player waist").transform.Find("player belt").gameObject;
        head = transform.Find("player body").transform.Find("player head").gameObject;
        calffront = transform.Find("player calf front").gameObject;
        calfback = transform.Find("player calf back").gameObject;
        twohandweapon = transform.Find("player 2h weap").gameObject;
        shoulderback = transform.Find("player body").transform.Find("player shoulder back").gameObject;
        shoulderfront = transform.Find("player body").transform.Find("player shoulder front").gameObject;
        shoulderbackback = transform.Find("player body").transform.Find("player shoulder back").transform.Find("player shoulder back back").gameObject;

        bicepback.GetComponent<bodypart>().setEquip("_borak heavy");
        bicepfront.GetComponent<bodypart>().setEquip("_borak heavy");
        forearmback.GetComponent<bodypart>().setEquip("_borak heavy");
        forearmfront.GetComponent<bodypart>().setEquip("_borak heavy");
        thighback.GetComponent<bodypart>().setEquip("_borak heavy");
        thighfront.GetComponent<bodypart>().setEquip("_borak heavy");
        forearmfront.GetComponent<bodypart>().setEquip("_borak heavy");
        handback.GetComponent<bodypart>().setEquip("_borak heavy");
        handfront.GetComponent<bodypart>().setEquip("_borak heavy");
        feetback.GetComponent<bodypart>().setEquip("_borak heavy");
        feetfront.GetComponent<bodypart>().setEquip("_borak heavy");
        body.GetComponent<bodypart>().setEquip("_borak heavy");
        waist.GetComponent<bodypart>().setEquip("_borak heavy");
        belt.GetComponent<bodypart>().setEquip("_borak heavy");
        head.GetComponent<bodypart>().setEquip("_borak heavy");
        calffront.GetComponent<bodypart>().setEquip("_borak heavy");
        calfback.GetComponent<bodypart>().setEquip("_borak heavy");
        twohandweapon.GetComponent<bodypart>().setEquip("_borak heavy");
        shoulderback.GetComponent<bodypart>().setEquip("_borak heavy");
        shoulderfront.GetComponent<bodypart>().setEquip("_borak heavy");
        shoulderbackback.GetComponent<bodypart>().setEquip("_borak heavy");
    }

    protected override void Start()
    {
        //resetSprite();
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
        if (dying)
        {
            opacity -= decayRate;
            ChangeOpacity(opacity);
            if (opacity <= 0)
            {
                Destroy(gameObject);
                GameObject spawner = GameObject.Find("PlayerSpawner");
                spawner.GetComponent<PlayerSpawn>().deadPlayer();
            }
        }
        if (grounded)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        if (dashing)
        {
            inAction = true;
            anim.SetBool("inAction", true);
            anim.SetFloat("dashing", 1);
        }
        else
        {
            anim.SetFloat("dashing", 0);
        }
        if (!jumping && !inAction)
        {
            //if completely idle
            if (!(grounded && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) != 0))
            {
                anim.SetBool("isIdle", true);

            }

        }
        //inputs
        if (Input.GetKey("1"))
        {
            if (!fire)
            {
                ClearInfusion();
                GetComponent<testplayerstats>().fireInfusion();
                twohandweapon.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Fire");
                fire = true;
            }
        }
        else if (Input.GetKey("2"))
        {
            if (!ice)
            {
                ClearInfusion();
                GetComponent<testplayerstats>().IceInfusion();
                twohandweapon.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Ice");
                ice = true;
            }
        }
        else if (Input.GetKey("3"))
        {
            if (!lightning)
            {
                ClearInfusion();
                GetComponent<testplayerstats>().LightningInfusion();
                twohandweapon.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Lightning");
                lightning = true;
            }
        }
        else if (Input.GetKey("4"))
        {
            ClearInfusion();
        }

        //get stats from unitstats.cs
        dashSpeed = GetComponent<testplayerstats>().getDashSpeed();
        dashForce = 20f * dashSpeed;
        anim.SetFloat("dashSpeed", attackSpeed);

        //jump counters
        if (jumping)
        {
            jumpcounter += Time.deltaTime;
            if (jumpcounter >= jumpclipcount)
            {
                jumping = false;
                //if (currentAnimation != "PlayerJump")
                //{
                //    setAnimation("PlayerJump");
                //}
            }
        }
        else
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x), 0f);
            jumpcounter = 0f;
        }
        //not counted as jumping if in action
        if (inAction)
        {
            if (!(currentAction == Actions.Dash))
            {
                dashing = false;
            }
            jumping = false;
        }

        if (!inAction)
        {
            // If the jump button is pressed and the player is grounded then the player should jump.
            if (Input.GetButtonDown("Jump") && grounded && canJump)
                jump = true;
            else if (Input.GetButtonDown("Fire1"))
            {

                if (canNA2)
                {
                    iniAttack2 = true;
                }
                else if (canNA3)
                {
                    iniAttack3 = true;
                }
                else if (canNA4)
                {
                    iniAttack4 = true;
                }
                else if (canReNA1 || !inNA)
                {
                    iniAttack1 = true;
                }
            }
            else if (Input.GetKey("z") && !dashing)
            {
                iniDash = true;
            }
            //follow up inis
            if (canAttack)
            {

                if (iniAttack1)
                {
                    attack1();
                    nextNA = 1;
                }
                else if (iniAttack2)
                {
                    attack2();
                    nextNA = 2;
                }
                else if (iniAttack3)
                {
                    attack3();
                    nextNA = 3;
                }
                else if (iniAttack4)
                {
                    attack4();
                    nextNA = 4;
                }
            }
            anim.SetFloat("nextNA", nextNA);
        }
        if (iniDash)
        {
            dash();
        }
        //run FX if unit it attacking, also, if player presses attack button again, queue for next attack
        if (!isFrozen)
        {
            if (currentAction == Actions.NA1)
            {
                startAttack1Anim();
                if (Input.GetButtonDown("Fire1"))
                {
                    iniAttack2 = true;
                }
            }
            else if (currentAction == Actions.NA2)
            {
                startAttack2anim();
                if (Input.GetButtonDown("Fire1"))
                {
                    iniAttack3 = true;
                }
            }
            else if (currentAction == Actions.NA3)
            {
                startAttack3anim();
                if (Input.GetButtonDown("Fire1"))
                {
                    iniAttack4 = true;
                }
            }
            else if (currentAction == Actions.NA4)
            {
                startAttack4anim();
            }
            else if (currentAction == Actions.Dash)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    iniAttack1 = true;
                }
            }
        }
        //setting and using delay before n-a streak is over
        if (attackdelay)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0 && !isFrozen)
            {
                attackdelay = false;
                resetNAcounter();
                resetSprite();
                anim.SetBool("canIdle", true);
            }
        }
        else
        {
            if (attackSpeed > 1)
                delayTimer = baseDelayTimer;
            else
                delayTimer = baseDelayTimer / attackSpeed;

        }
        //setting and implementing pause between attacks
        if (pauseDelay)
        {
            attackPause -= Time.deltaTime;
            if (attackPause <= 0)
            {
                canAttack = true;
                pauseDelay = false;
            }
        }
        else
        {
            attackPause = baseAttackPause / attackSpeed;
        }
        if (startDashDelay)
        {
            dashAttackTimer -= Time.deltaTime;
            if (dashAttackTimer <= 0 && !attacking)
            {
                anim.SetFloat("dashAttack", 0);
                startDashDelay = false;
            }
        }
        else
        {
            dashAttackTimer = 1f;
        }

    }

    void FixedUpdate()
    {

        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");
        cliplength = anim.GetCurrentAnimatorStateInfo(0).length;

        if (attacking && grounded)
        {

            float attackAdvanceSpeed = 2f;
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                attackAdvanceSpeed = movementSpeed * attackSpeed;
            }
            if (anim.GetFloat("dashAttack") == 1)
            {
                attackAdvanceSpeed = 8f;
            }
            if (!collidingWithEnemy)
            {
                if (facingRight)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(attackAdvanceSpeed, 0f);
                else
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-attackAdvanceSpeed, 0f);
            }
        }
        if (dashing)
        {

            noGravity();
            if (dashRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(dashForce * dashSpeed, 0f);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-dashForce * dashSpeed, 0f);
            }
        }


        // if player is not performing any actions 
        if (!inAction)
        {

            if (!isFrozen)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * h, GetComponent<Rigidbody2D>().velocity.y);
                // GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
                if ((h * GetComponent<Rigidbody2D>().velocity.x > 0.1))
                // ... add a force to the player.
                {
                    if (grounded)
                    {
                        if (!Input.GetButtonDown("Fire1") && !jumping)
                            setAnimation("PlayerWalk");
                    }

                }

                // If the input is moving the player right and the player is facing left...
                if (h > 0 && !facingRight)
                    // ... flip the player.
                    FlipDirection();
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (h < 0 && facingRight)
                    // ... flip the player.
                    FlipDirection();

                if (jump)
                {
                    if (grounded && canJump)
                        setAnimation("PlayerJump");
                }
            }
            if (Mathf.Abs(h) > 0)
                anim.SetFloat("Speed", 1);
            else
                anim.SetFloat("Speed", 0);
        }

    }


    protected override void FlipDirection()
    {
        base.FlipDirection();
        anim.SetFloat("dashAttack", 0);

    }
    void attack1()
    {
        setAnimation("PlayerNormalAttack");
    }
    void startAttack1()
    {
        canReNA1 = false;
        inNA = true;
        anim.SetBool("isIdle", false);
        iniAttack1 = false;
        attacking = true;
        stopAttackDelays();

    }
    void NA1()
    {
        setAction(Actions.NA1);
        setWeaponHitOn();
    }
    void startAttack1Anim()
    {

        NA2hInstance = Instantiate(NA2h1, transform.Find("player 2h weap/tip").position, transform.Find("player 2h weap/tip").rotation);
        NA2hInstance.GetComponent<NAtwoh>().initialize(facingRight, transform.Find("player 2h weap/tip"));
        NA2hInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        NA2hInstance.GetComponent<Rigidbody2D>().gravityScale = 0;

    }
    void finishAttack1()
    {
        startAttackDelays();
        attacking = false;
        setAction(Actions.Default);
        inNA = false;
        canNA2 = true;
        setWeaponHitOff();
        anim.SetFloat("dashAttack", 0);
    }
    void attack2()
    {
        setAnimation("PlayerNormalAttack");
    }
    void startAttack2()
    {
        inNA = true;
        canNA2 = false;
        anim.SetBool("isIdle", false);
        iniAttack2 = false;
        attacking = true;
        stopAttackDelays();

    }
    void NA2()
    {
        setAction(Actions.NA2);
        setWeaponHitOn();
    }
    void startAttack2anim()
    {
        NA2hInstance = Instantiate(NA2h1, transform.Find("player 2h weap/tip").position, transform.Find("player 2h weap/tip").rotation);
        NA2hInstance.GetComponent<NAtwoh>().initialize(facingRight, transform.Find("player 2h weap/tip"));
        NA2hInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        NA2hInstance.GetComponent<Rigidbody2D>().gravityScale = 0;
        NA2hInstance.GetComponent<NAtwoh>().changeScale(transform.Find("player 2h weap"));
    }
    void finishedAttack2()
    {
        startAttackDelays();
        attacking = false;
        setAction(Actions.Default);
        inNA = false;
        canNA3 = true;
        setWeaponHitOff();
    }
    void attack3()
    {
        setAnimation("PlayerNormalAttack");
    }

    void startAttack3()
    {
        canNA3 = false;
        inNA = true;
        anim.SetBool("isIdle", false);
        iniAttack3 = false;
        attacking = true;
        stopAttackDelays();

    }
    void NA3()
    {
        setAction(Actions.NA3);
        setWeaponHitOn();
    }
    void startAttack3anim()
    {
        NA2hInstance = Instantiate(NA2h1, transform.Find("player 2h weap/tip").position, transform.Find("player 2h weap/tip").rotation);
        NA2hInstance.GetComponent<NAtwoh>().initialize(facingRight, transform.Find("player 2h weap/tip"));
        NA2hInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        NA2hInstance.GetComponent<Rigidbody2D>().gravityScale = 0;
        NA2hInstance.GetComponent<NAtwoh>().changeScale(transform.Find("player 2h weap"));
    }
    void finishedAttack3()
    {
        startAttackDelays();
        attacking = false;
        setAction(Actions.Default);
        canNA4 = true;
        inNA = false;
        setWeaponHitOff();
    }
    void attack4()
    {
        setAnimation("PlayerNormalAttack");
    }
    void startAttack4()
    {
        canNA4 = false;
        inNA = true;
        anim.SetBool("isIdle", false);
        iniAttack4 = false;
        attacking = true;
        stopAttackDelays();

    }
    void NA4()
    {
        setAction(Actions.NA3);
        setWeaponHitOn();
    }
    void startAttack4anim()
    {
        NA2hInstance = Instantiate(NA2h1, transform.Find("player 2h weap/tip").position, transform.Find("player 2h weap/tip").rotation);
        NA2hInstance.GetComponent<NAtwoh>().initialize(facingRight, transform.Find("player 2h weap/tip"));
        NA2hInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        NA2hInstance.GetComponent<Rigidbody2D>().gravityScale = 0;
        NA2hInstance.GetComponent<NAtwoh>().changeScale(transform.Find("player 2h weap"));
        //if(!grounded)
        //    GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x), -10f);
    }
    void finishedAttack4()
    {
        startAttackDelays();
        attacking = false;
        setAction(Actions.Default);
        canReNA1 = true;
        inNA = false;
        setWeaponHitOff();
    }
    void startAttack4to1()
    {

        inNA = true;
        stopAttackDelays();
        iniAttack1 = false;
    }
    void dash()
    {
        setAnimation("PlayerDash");
    }
    void startDash()
    {
        setAction(Actions.Dash);
        resetSprite();
        resetNAcounter();
        anim.SetBool("isIdle", false);
        dashing = true;
        iniDash = false;
        anim.SetFloat("dashAttack", 1);
        GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x), 0); ;
        if (Input.GetAxis("Horizontal") > 0)
        {

            if (!facingRight)
                FlipDirection();
            dashRight = true;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {

            if (facingRight)
                FlipDirection();
            dashRight = false;
        }
        else
        {
            if (facingRight)
            {
                dashRight = true;
            }
            else
            {
                dashRight = false;
            }
        }
    }
    void finishDash()
    {
        dashing = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        startDashDelay = true;
    }

    void noGravity()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x), 0.5f);
    }

    void walk()
    {
        resetSprite();
        resetNAcounter();
        attacking = false;
        dashing = false;
        casting = false;
        stopAttackDelays();

    }
    void startjump()
    {
        resetSprite();
        resetNAcounter();
        anim.SetBool("isIdle", false);
        if (grounded)
            GetComponent<Rigidbody2D>().velocity = new Vector2((GetComponent<Rigidbody2D>().velocity.x), 20f);
        jumping = true;
        jump = false;
    }
    void stopjump()
    {
        anim.SetBool("isIdle", true);
        jumping = false;
        jump = false;
    }
    void resetNAcounter()
    {
        if (canReNA1)
            canReNA1 = false;
        if (canNA2)
            canNA2 = false;
        if (canNA3)
            canNA3 = false;
        if (canNA4)
            canNA4 = false;
        nextNA = 1;
    }

    //Sprite Flips
    public void flipBackArm()
    {
        forearmback.GetComponent<playerforearmback>().flip();
    }
    public void flipFrontArm()
    {
        forearmfront.GetComponent<playerforearmfront>().flip();
    }
    public void flipBody()
    {
        body.GetComponent<playerbody>().flip();
    }
    public void flipfronthand()
    {
        handfront.GetComponent<playerhandfront>().flip();
    }
    public void flipbackhand()
    {
        handback.GetComponent<playerhandback>().flip();
    }
    public void flipfrontcalf()
    {
        calffront.GetComponent<playercalffront>().flip();
    }
    public void flipbackcalf()
    {
        calfback.GetComponent<playercalfback>().flip();
    }
    public void flipHead()
    {
        head.GetComponent<playerhead>().flip();
    }
    public void flipwaist()
    {
        belt.GetComponent<playerbelt>().flip();
        waist.GetComponent<playerwaist>().flip();
    }
    public void flip2h()
    {
        twohandweapon.GetComponent<player2hweap>().flip();
    }
    //layer orders
    public void frontArmBack()
    {
        bicepfront.GetComponent<SpriteRenderer>().sortingOrder = 2 * layerMultiplier;
        forearmfront.GetComponent<SpriteRenderer>().sortingOrder = 3 * layerMultiplier;
        handfront.GetComponent<SpriteRenderer>().sortingOrder = 2 * layerMultiplier;
        bicepfront.GetComponent<SpriteMask>().frontSortingOrder = 2 * layerMultiplier + 1;
        forearmfront.GetComponent<SpriteMask>().frontSortingOrder = 3 * layerMultiplier + 1;
        handfront.GetComponent<SpriteMask>().frontSortingOrder = 2 * layerMultiplier + 1;
    }
    public void frontArmFront()
    {
        bicepfront.GetComponent<SpriteRenderer>().sortingOrder = 10 * layerMultiplier;
        forearmfront.GetComponent<SpriteRenderer>().sortingOrder = 11 * layerMultiplier;
        handfront.GetComponent<SpriteRenderer>().sortingOrder = 10 * layerMultiplier;
        bicepfront.GetComponent<SpriteMask>().frontSortingOrder = 10 * layerMultiplier + 1;
        forearmfront.GetComponent<SpriteMask>().frontSortingOrder = 11 * layerMultiplier + 1;
        handfront.GetComponent<SpriteMask>().frontSortingOrder = 10 * layerMultiplier + 1;
    }
    public void backArmBack()
    {
        bicepback.GetComponent<SpriteRenderer>().sortingOrder = 2 * layerMultiplier;
        forearmback.GetComponent<SpriteRenderer>().sortingOrder = 3 * layerMultiplier;
        handback.GetComponent<SpriteRenderer>().sortingOrder = 2 * layerMultiplier;
        bicepback.GetComponent<SpriteMask>().frontSortingOrder = 2 * layerMultiplier + 1;
        forearmback.GetComponent<SpriteMask>().frontSortingOrder = 3 * layerMultiplier + 1;
        handback.GetComponent<SpriteMask>().frontSortingOrder = 2 * layerMultiplier + 1;
    }
    public void backArmMid()
    {
        bicepback.GetComponent<SpriteRenderer>().sortingOrder = 3 * layerMultiplier;
        forearmback.GetComponent<SpriteRenderer>().sortingOrder = 10 * layerMultiplier;
        handback.GetComponent<SpriteRenderer>().sortingOrder = 9 * layerMultiplier;
        bicepback.GetComponent<SpriteMask>().frontSortingOrder = 3 * layerMultiplier + 1;
        forearmback.GetComponent<SpriteMask>().frontSortingOrder = 10 * layerMultiplier + 1;
        handback.GetComponent<SpriteMask>().frontSortingOrder = 9 * layerMultiplier + 1;
    }
    public void backArmFront()
    {
        bicepback.GetComponent<SpriteRenderer>().sortingOrder = 8 * layerMultiplier;
        forearmback.GetComponent<SpriteRenderer>().sortingOrder = 10 * layerMultiplier;
        handback.GetComponent<SpriteRenderer>().sortingOrder = 9 * layerMultiplier;
        bicepback.GetComponent<SpriteMask>().frontSortingOrder = 8 * layerMultiplier + 1;
        forearmback.GetComponent<SpriteMask>().frontSortingOrder = 10 * layerMultiplier + 1;
        handback.GetComponent<SpriteMask>().frontSortingOrder = 9 * layerMultiplier + 1;
    }
    public void twohBack()
    {
        twohandweapon.GetComponent<SpriteRenderer>().sortingOrder = 1 * layerMultiplier;
        twohandweapon.GetComponent<SpriteMask>().frontSortingOrder = 1 * layerMultiplier + 1;
    }
    public void twohFront()
    {
        twohandweapon.GetComponent<SpriteRenderer>().sortingOrder = 8 * layerMultiplier;
        twohandweapon.GetComponent<SpriteMask>().frontSortingOrder = 8 * layerMultiplier + 1;
    }
    public void backshoulderback()
    {
        shoulderback.GetComponent<SpriteRenderer>().sortingOrder = 3 * layerMultiplier;
        shoulderback.GetComponent<SpriteMask>().frontSortingOrder = 3 * layerMultiplier + 1;
    }
    public void backshoulderfront()
    {
        shoulderback.GetComponent<SpriteRenderer>().sortingOrder = 10 * layerMultiplier;
        shoulderback.GetComponent<SpriteMask>().frontSortingOrder = 10 * layerMultiplier + 1;
    }
    public void resetSprite()
    {
        forearmback.GetComponent<playerforearmback>().reset();
        forearmfront.GetComponent<playerforearmfront>().reset();
        body.GetComponent<playerbody>().reset();
        handfront.GetComponent<playerhandfront>().reset();
        handback.GetComponent<playerhandback>().reset();
        calffront.GetComponent<playercalffront>().reset();
        calfback.GetComponent<playercalfback>().reset();
        head.GetComponent<playerhead>().reset();
        belt.GetComponent<playerbelt>().reset();
        waist.GetComponent<playerwaist>().reset();
        twohandweapon.GetComponent<player2hweap>().reset();
        frontArmFront();
        backArmMid();
        twohFront();
    }
    protected override void resetStance()
    {
        base.resetStance();
        dashing = false;
        inNA = false;
        setWeaponHitOff();
    }
    void startAttackDelays()
    {
        attackdelay = true;
        pauseDelay = true;
        canAttack = false;
        anim.SetBool("canIdle", false);
        inNA = false;
    }
    void stopAttackDelays()
    {
        attackdelay = false;
        pauseDelay = false;
        canAttack = true;
        anim.SetBool("canIdle", true);
    }

    void setWeaponHitOn()
    {
        twohandweapon.GetComponent<player2hweap>().DetectOn();
    }
    void setWeaponHitOff()
    {
        twohandweapon.GetComponent<player2hweap>().DetectOff();
    }
    public void isCollidingWithEnemy()
    {
        collidingWithEnemy = true;
    }
    public void notCollidingWithEnemy()
    {
        collidingWithEnemy = false;
    }

    public override void TriggerDeath()
    {
        inAction = true;
        dying = true;
        setAnimation("TriggerDeath");
    }

    public override void AddEffectOverlay(Sprite sprite)
    {


        twohandweapon.GetComponent<bodypart>().applyOverlay(sprite);
        bicepback.GetComponent<bodypart>().applyOverlay(sprite);
        bicepfront.GetComponent<bodypart>().applyOverlay(sprite);
        forearmback.GetComponent<bodypart>().applyOverlay(sprite);
        forearmfront.GetComponent<bodypart>().applyOverlay(sprite);
        thighback.GetComponent<bodypart>().applyOverlay(sprite);
        thighfront.GetComponent<bodypart>().applyOverlay(sprite);
        forearmfront.GetComponent<bodypart>().applyOverlay(sprite);
        handback.GetComponent<bodypart>().applyOverlay(sprite);
        handfront.GetComponent<bodypart>().applyOverlay(sprite);
        body.GetComponent<bodypart>().applyOverlay(sprite);
        waist.GetComponent<bodypart>().applyOverlay(sprite);
        belt.GetComponent<bodypart>().applyOverlay(sprite);
        head.GetComponent<bodypart>().applyOverlay(sprite);
        calffront.GetComponent<bodypart>().applyOverlay(sprite);
        calfback.GetComponent<bodypart>().applyOverlay(sprite);
        twohandweapon.GetComponent<bodypart>().applyOverlay(sprite);
        shoulderfront.GetComponent<bodypart>().applyOverlay(sprite);
        shoulderback.GetComponent<bodypart>().applyOverlay(sprite);
        shoulderbackback.GetComponent<bodypart>().applyOverlay(sprite);
        feetfront.GetComponent<bodypart>().applyOverlay(sprite);
        feetback.GetComponent<bodypart>().applyOverlay(sprite);
    }
    public override void RemoveEffectOverlay(Sprite sprite)
    {
        twohandweapon.GetComponent<bodypart>().removeOverlay(sprite);
        bicepback.GetComponent<bodypart>().removeOverlay(sprite);
        bicepfront.GetComponent<bodypart>().removeOverlay(sprite);
        forearmback.GetComponent<bodypart>().removeOverlay(sprite);
        forearmfront.GetComponent<bodypart>().removeOverlay(sprite);
        thighback.GetComponent<bodypart>().removeOverlay(sprite);
        thighfront.GetComponent<bodypart>().removeOverlay(sprite);
        forearmfront.GetComponent<bodypart>().removeOverlay(sprite);
        handback.GetComponent<bodypart>().removeOverlay(sprite);
        handfront.GetComponent<bodypart>().removeOverlay(sprite);
        body.GetComponent<bodypart>().removeOverlay(sprite);
        waist.GetComponent<bodypart>().removeOverlay(sprite);
        belt.GetComponent<bodypart>().removeOverlay(sprite);
        head.GetComponent<bodypart>().removeOverlay(sprite);
        calffront.GetComponent<bodypart>().removeOverlay(sprite);
        calfback.GetComponent<bodypart>().removeOverlay(sprite);
        twohandweapon.GetComponent<bodypart>().removeOverlay(sprite);
        shoulderfront.GetComponent<bodypart>().removeOverlay(sprite);
        shoulderback.GetComponent<bodypart>().removeOverlay(sprite);
        shoulderbackback.GetComponent<bodypart>().removeOverlay(sprite);
        feetfront.GetComponent<bodypart>().removeOverlay(sprite);
        feetback.GetComponent<bodypart>().removeOverlay(sprite);
    }
    public override void AddFlames()
    {


        twohandweapon.GetComponent<bodypart>().AddFlames();
        bicepback.GetComponent<bodypart>().AddFlames();
        bicepfront.GetComponent<bodypart>().AddFlames();
        forearmback.GetComponent<bodypart>().AddFlames();
        forearmfront.GetComponent<bodypart>().AddFlames();
        thighback.GetComponent<bodypart>().AddFlames();
        thighfront.GetComponent<bodypart>().AddFlames();
        forearmfront.GetComponent<bodypart>().AddFlames();
        handback.GetComponent<bodypart>().AddFlames();
        handfront.GetComponent<bodypart>().AddFlames();
        body.GetComponent<bodypart>().AddFlames();
        waist.GetComponent<bodypart>().AddFlames();
        belt.GetComponent<bodypart>().AddFlames();
        head.GetComponent<bodypart>().AddFlames();
        calffront.GetComponent<bodypart>().AddFlames();
        calfback.GetComponent<bodypart>().AddFlames();
        twohandweapon.GetComponent<bodypart>().AddFlames();
        shoulderfront.GetComponent<bodypart>().AddFlames();
        shoulderback.GetComponent<bodypart>().AddFlames();
        shoulderbackback.GetComponent<bodypart>().AddFlames();
        feetfront.GetComponent<bodypart>().AddFlames();
        feetback.GetComponent<bodypart>().AddFlames();
    }
    public override void ChangeOpacity(float opacity)
    {
        twohandweapon.GetComponent<bodypart>().ChangeOpacity(opacity);
        bicepback.GetComponent<bodypart>().ChangeOpacity(opacity);
        bicepfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        forearmback.GetComponent<bodypart>().ChangeOpacity(opacity);
        forearmfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        thighback.GetComponent<bodypart>().ChangeOpacity(opacity);
        thighfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        forearmfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        handback.GetComponent<bodypart>().ChangeOpacity(opacity);
        handfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        body.GetComponent<bodypart>().ChangeOpacity(opacity);
        waist.GetComponent<bodypart>().ChangeOpacity(opacity);
        belt.GetComponent<bodypart>().ChangeOpacity(opacity);
        head.GetComponent<bodypart>().ChangeOpacity(opacity);
        calffront.GetComponent<bodypart>().ChangeOpacity(opacity);
        calfback.GetComponent<bodypart>().ChangeOpacity(opacity);
        twohandweapon.GetComponent<bodypart>().ChangeOpacity(opacity);
        shoulderfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        shoulderback.GetComponent<bodypart>().ChangeOpacity(opacity);
        shoulderbackback.GetComponent<bodypart>().ChangeOpacity(opacity);
        feetfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        feetback.GetComponent<bodypart>().ChangeOpacity(opacity);
    }
    void ClearInfusion()
    {
        GetComponent<testplayerstats>().clearInfusions();
        twohandweapon.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Default");
        fire = false;
        ice = false;
        lightning = false;

    }
}
