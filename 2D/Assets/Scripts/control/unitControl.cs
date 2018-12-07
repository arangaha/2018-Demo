using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class unitControl : MonoBehaviour {
    [SerializeField] protected bool facingRight = false;         // For determining which way the unit is currently facing.
    [SerializeField] protected Transform groundCheck;          // A position marking where to check if the unit is grounded.
    [SerializeField] protected bool grounded = true;          // Whether or not the player is grounded.
    [SerializeField] protected Animator anim;                  // Reference to the player's animator component.
    [SerializeField] protected bool attacking = false;
    [SerializeField] protected bool casting = false;
    [SerializeField] protected bool dying = false;
    [SerializeField] protected bool inAction = false;
    public string currentAnimation = null;
    [SerializeField] protected bool canAttack = true;

    [SerializeField] protected bool isFrozen = false;
    [SerializeField] protected bool firstFreeze = false;

    //animator parameters
    [SerializeField] protected float moveMulti = 1f;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float genericMulti = 1f;
    [SerializeField] protected float velocity = 0f;
    //stats for unitstats
    [SerializeField] protected Actions currentAction = Actions.Default;

    //nearby unit detection
    [SerializeField] List<GameObject> NearbyAllies = new List<GameObject>();
    [SerializeField] List<GameObject> NearbyEnemies = new List<GameObject>();
    [SerializeField] GameObject ClosestAlly;
    [SerializeField] GameObject ClosestEnemy;
    public float decayRate = 0.05f;
    public float opacity = 1;
    protected virtual void Awake()
    {
        groundCheck = transform.Find("ground check");
        anim = GetComponent<Animator>();
    }

    protected virtual void Start () {
        groundCheck = transform.Find("ground check");
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        //frozen/unfrozen 
        if (isFrozen)
        {
            inAction = true;
            if (!firstFreeze)
            {
                anim.enabled = false;
                canAttack = false;
                resetStance();
                firstFreeze = true;
            }
        }
        else if (firstFreeze && !isFrozen)
        {
            inAction = false;
            anim.enabled = true;
            canAttack = true;
            firstFreeze = false;
        }
        //set current states

        if (grounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetFloat("grounded", 0);
        }
        else
        {
            anim.SetBool("isGrounded", false);
            anim.SetFloat("grounded", 1);
        }
        if (attacking || casting || isFrozen || dying)
        {
            inAction = true;

            anim.SetBool("inAction", true);
        }
        else
        {
            inAction = false;
            anim.SetBool("inAction", false);
        }
        //idle checks
        float h = Input.GetAxis("Horizontal");
        //if (Mathf.Abs(h) > 0 && grounded)
        //{
        //    anim.SetBool("isIdle", false);
        //}
        //groundcheck
        if (groundCheck != null)
        {
            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        }
        //retrieve stats from unitstats.cs
        attackSpeed = GetComponent<unitstats>().getAttackSpeed();
        movementSpeed = GetComponent<unitstats>().getMovementSpeed();
        moveMulti = GetComponent<unitstats>().getMoveMulti();
        genericMulti = GetComponent<unitstats>().getGenericMulti();
        isFrozen = GetComponent<unitstats>().Frozen();
        anim.SetFloat("AttackSpeed", attackSpeed);
        velocity = GetComponent<Rigidbody2D>().velocity.x;
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 0.1)
            anim.SetFloat("moveMultiplier", moveMulti);
        else
            anim.SetFloat("moveMultiplier", genericMulti);



    }

    protected virtual void FlipDirection()
    {
        
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected virtual void resetStance()
    {
        attacking = false;
        casting = false;
        currentAction = Actions.Default;
    }

    public abstract void AddEffectOverlay(Sprite sprite);

    public abstract void RemoveEffectOverlay(Sprite sprite);

    public abstract void AddFlames();

    public abstract void TriggerDeath();

    public abstract void ChangeOpacity(float opacity);

    protected void setAnimation(string stringInput)
    {
        if (currentAnimation != null)
        {
            anim.ResetTrigger(currentAnimation);
        }
        anim.SetTrigger(stringInput);
        currentAnimation = stringInput;
    }

    public virtual void updateNearbyAllies (List<GameObject> allies)
    {
        NearbyAllies = allies;
        ClosestAlly = closestUnit(NearbyAllies);
    }
    public virtual void updateNearbyEnemies(List<GameObject> enemies)
    {
        NearbyEnemies = enemies;
        ClosestEnemy = closestUnit(NearbyEnemies );
    }
    public void CreateProjectile(ProjectileScript p)
    {
        //create a gameobject projectile with this script
        //projectile.addComponent(p);
    }



    protected void setAction(Actions a)
    {
        currentAction = a;
        GetComponent<unitstats>().setCurrentAction(a);
    }
    public Actions getAction()
    {
        return currentAction;
    }

    protected float getUnitDistance (GameObject unit)
    {
        float tempx = unit.transform.position.x-transform.position.x;
        float tempy = unit.transform.position.y- transform.position.y;
        float unitDistance = Mathf.Sqrt(tempx * tempx + tempy * tempy);
        
        return unitDistance;
    }

    public List<GameObject> getUnitsByDistance (bool isAlly, float distance)
    {
        List<GameObject> unitsInRange = new List<GameObject>();
        List<GameObject> allUnits = new List<GameObject>();
        if(isAlly)
        {
            allUnits = NearbyAllies;
        }
        else
        {
            allUnits = NearbyEnemies;
        }
        for(int i = 0; i<allUnits.Count;i++)
        {
            float unitDistance = getUnitDistance(allUnits[i]);
            if(unitDistance<=distance)
            {
                unitsInRange.Add(allUnits[i]);
            }
        }
        return unitsInRange;
    }
    public GameObject closestUnit(List<GameObject> listOfUnits)
    {
        if (listOfUnits.Count > 0)
        {
            float closestDistance = getUnitDistance(listOfUnits[0]);
            int index = 0;
            for (int i = 1; i < listOfUnits.Count; i++)
            {
                if (getUnitDistance(listOfUnits[i]) < closestDistance)
                {
                    closestDistance = getUnitDistance(listOfUnits[i]);
                    index = i;
                }
            }
            return listOfUnits[index];
        }
        else
        {
            return null;
        }
    }
}
