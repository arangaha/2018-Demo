using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class default_fx : MonoBehaviour {
    System.Random rand = new System.Random();
    [SerializeField] protected GameObject owner;
    [SerializeField] protected float baseScale=0; //base AoE
    [SerializeField] protected float currentScale=0;
    [SerializeField] protected float maxScale=0;
    [SerializeField] protected float baseSpeed = 0; //for projectiles
    [SerializeField] protected float currentSpeed = 0;
    [SerializeField] protected float maxSpeed = 0;
    [SerializeField] protected bool isMelee = false;
    [SerializeField] protected bool isAoE = false;
    [SerializeField] protected bool isProjectile = false;
    [SerializeField] protected bool isDoT = false;
    [SerializeField] protected bool canHit = false;
    [SerializeField] protected bool canDoT = false;
    [SerializeField] protected bool decay = false;
    [SerializeField] protected float decayRate = 0.005f;
    [SerializeField] protected float opacity = 1;
    List<GameObject> hitEnemyList = new List<GameObject>();
    List<GameObject> touchingEnemyList = new List<GameObject>();
    [SerializeField] protected float duration = 10000000; //duration of skill. set to infinite unless specified. skill cannot damage anything after duration is 0
    #region hit stats
    [SerializeField] float physicalDamage = 0;
    [SerializeField] float fireDamage = 0;
    [SerializeField] float iceDamage = 0;
    [SerializeField] float toxicDamage = 0;
    [SerializeField] float lightningDamage = 0;
    [SerializeField] float shadowDamage = 0;
    [SerializeField] float armorPen= 0;
    [SerializeField] float firePen = 0;
    [SerializeField] float icePen = 0;
    [SerializeField] float toxicPen = 0;
    [SerializeField] float lightningPen = 0;
    [SerializeField] float accuracy = 0;
    [SerializeField] float critChance = 0;
    [SerializeField] float critDamage = 0;
    [SerializeField] float igniteChance = 0;
    [SerializeField] float chillChance = 0;
    [SerializeField] float toxinChance = 0;
    [SerializeField] float shockChance = 0;
    [SerializeField] float dreadChance = 0;
    [SerializeField] float lifeSteal = 0;
    #endregion
    #region dot stats
    float damagePerSecond = 0;
    Elements element;
    #endregion

    //way of how an effect stretches out
    [SerializeField] protected bool horizontalStretch = false;
    [SerializeField] protected bool verticalStretch = false;

    // Use this for initialization
    protected virtual void Start () {

	}

    // Update is called once per frame
    protected virtual void Update () {
        duration -= Time.deltaTime;
        if(duration <=0)
        {
            canHit = false;
            canDoT = false;
        }
		if(decay)
        {
            opacity -= decayRate;

        }
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
        if(opacity<=0)
        {
            Destroy(gameObject);
        }
        if(canDoT)
        dotEnemies();
        if(horizontalStretch)
            transform.localScale = new Vector3(currentScale,transform.localScale.y, 0);
        if(verticalStretch)
            transform.localScale = new Vector3(transform.localScale.x, currentScale, 0);
    }
    public virtual void initialize(GameObject owner,bool isMelee, bool isProj, bool isAoE, float increasedProjSpeed, float increasedAoE)
    {
        
        this.owner = owner;
        this.isAoE = isAoE;
        isProjectile = isProj;
        this.isMelee = isMelee;
        if(isAoE)
        {
            maxScale = baseScale*( 1 + increasedAoE);
        }
        if(isProjectile)
        {
            maxSpeed = baseSpeed * (1 + increasedProjSpeed);
        }
    }
    //initializes damage of this FX,
    //all elementss except for shadow contains an array of 2 elements (damage, penetration). 
    //shadow contains damage
    public void initializeHit(float[,] damageValues, float acc, float crit, float critMulti, float igniteCh, float chillCh, float toxinCh, float shockCh, float dreadCh, float lifeSt)
    {
        canHit = true;
        physicalDamage = damageValues[0, 0];
        fireDamage = damageValues[1, 0];
        iceDamage = damageValues[2, 0];
        toxicDamage = damageValues[3, 0];
        lightningDamage = damageValues[4, 0];
        shadowDamage = damageValues[5, 0];
        armorPen = damageValues[0, 1];
        firePen = damageValues[1, 1];
        icePen = damageValues[2, 1];
        toxicPen = damageValues[3, 1];
        lightningPen = damageValues[4, 1];
        accuracy = acc;
        critChance = crit;
        critDamage = critMulti;
        igniteChance = igniteCh;
        chillChance = chillCh;
        toxinChance = toxinCh;
        shockChance = shockCh;
        dreadChance = dreadCh;
        lifeSteal = lifeSt;


    }
    public void initializeDoT (float dot, Elements e)
    {
        canDoT = true;
        isDoT = true;
        damagePerSecond = dot;
        element = e;
    }
    public void setOpacity(float o)
    {
        opacity = o;
    }
    public void canDecay()
    {
        decay = true;
    }
    public void cantDecay()
    {
        decay=false;
    }
    public void setDuration(float dura)
    {
        duration = dura;
    }
    protected void dotEnemies()
    {
        for(int i = 0; i<touchingEnemyList.Count;i++)
        {
            touchingEnemyList[i].GetComponent<unitstats>().TakeDoT(damagePerSecond,element,Time.deltaTime);
        }
    }
    protected void hitEnemy(GameObject target)
    {
        bool missedHit = false;
        bool isCrit = false;
        float[,] dmgValues = {{physicalDamage, armorPen},{ fireDamage,firePen},{ iceDamage, icePen },{ toxicDamage, toxicPen },{ lightningDamage, lightningPen },{ shadowDamage, 0 } };
        //accuracy roll
        float accRoll = rand.Next(1, 101);
        if (accRoll > accuracy)
        {
            missedHit = true;
            owner.GetComponent<unitstats>().missHit();
        }
        //roll and apply crit
        float critRoll = rand.Next(1, 101);
        if (critRoll <= critChance)
        {
            isCrit = true;
            dmgValues[0, 0] *= critDamage;
            dmgValues[1, 0] *= critDamage;
            dmgValues[2, 0] *= critDamage;
            dmgValues[3, 0] *= critDamage;
            dmgValues[4, 0] *= critDamage;
            dmgValues[5, 0] *= critDamage;
        }
        float[,] hitValues = target.GetComponent<unitstats>().takeHit(dmgValues, missedHit, isCrit, isMelee, isProjectile, isAoE);

        //lifesteal
        float stolenLife = lifeSteal * (hitValues[0, 0] + hitValues[1, 0] + hitValues[2, 0] + hitValues[3, 0] + hitValues[4, 0] + hitValues[5, 0]);
        owner.GetComponent<unitstats>().heal(stolenLife);

        //elemental effects
        //ignite (fire)
        if (hitValues[1, 0] > 0)
        {

            float igniteRoll = rand.Next(1, 101);
            if (igniteRoll < igniteChance)
            {
                owner.GetComponent<unitstats>().ApplyElementalEffect(ElementalEffects.Ignite, target);
            }
        }
        //chill (ice)
        if (hitValues[2, 0] > 0)
        {
            float chillRoll = rand.Next(1, 101);
            if (chillRoll < chillChance)
            {
                owner.GetComponent<unitstats>().ApplyElementalEffect(ElementalEffects.Chill, target);
            }
        }
        //Toxin (toxic)
        if (hitValues[3, 0] > 0)
        {
            float toxinRoll = rand.Next(1, 101);
            if (toxinRoll < toxinChance)
            {
                owner.GetComponent<unitstats>().ApplyElementalEffect(ElementalEffects.Toxin, target);
            }
        }
        //shock (lightning)
        if (hitValues[4, 0] > 0)
        {
            float shockRoll = rand.Next(1, 101);
            if (shockRoll < shockChance)
            {
                owner.GetComponent<unitstats>().ApplyElementalEffect(ElementalEffects.Shock, target);
            }
        }
        //Dread (shadow)
        if (hitValues[5, 0] > 0)
        {
            float dreadRoll = rand.Next(1, 101);
            if (dreadRoll < dreadChance)
            {
                owner.GetComponent<unitstats>().ApplyElementalEffect(ElementalEffects.Dread, target);
            }
        }

    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if ((owner.CompareTag("Player")&& collision.gameObject.CompareTag("Enemy")) ||(owner.CompareTag("Enemy") && collision.gameObject.CompareTag("Player")))
        {
            //for DoT
            bool inList = false;
            for (int i = 0; i < touchingEnemyList.Count; i++)
            {
                if (touchingEnemyList[i] == collision.gameObject)
                {
                    inList = true;
                }
            }
            if (!inList)
                touchingEnemyList.Add(collision.gameObject);

            //for hits
            bool hit = false;
            if (canHit)
            {
                for (int i = 0; i < hitEnemyList.Count; i++)
                {
                    if (hitEnemyList[i] == collision.gameObject)
                    {
                        hit = true;
                    }
                }
                if (!hit)
                {
                    hitEnemyList.Add(collision.gameObject);
                    hitEnemy(collision.gameObject);

                }
            }
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        bool inList = false;
        for (int i = 0; i < touchingEnemyList.Count; i++)
        {
            if (touchingEnemyList[i] == collision.gameObject)
            {
                inList = true;
            }
        }
        if (inList)
            touchingEnemyList.Remove(collision.gameObject);
    }
}
