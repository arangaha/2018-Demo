using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum EffectNames
{
    Placeholder,
    //Debuffs
    Shock,
    
    Electrocute,
    //ignite stack causing enemies to burn for 40%flatF for each stack per second over 3 seconds
    //At 5 stacks, stacks are consumed, dealing 120% of the remainder of the burn damage 6 feet around
    Ignite, 
    Toxin,
    Infect,
    Chill,
    Freeze,
    Dread,
    Fear,

    Silence,

    Blind,
    Bleed,

    //Buffs

    //Buffs and Debuffs

    RecklessStrikesEffect,
};
public enum Actions
{
    Default,
    //player actions
    NA1,
    NA2,
    NA3,
    NA4,
    Dash,
    Jump,

    //monster actions
    //rakab
    rakabNA,
    rakabSlam,
    //generic actions
    IgniteExplosion,


};
//effect description for multiple types:
//      (element): "Physical","Fire" , "Ice", "Toxic", "Lightning", "Shadow"
[System.Serializable]
public enum EffectDescriptions
{



    //increased/reduced
    IncreasedSpeed, //increased multiplier for attack, cast, dash, and movement speed. debuff and affected by tenacity if - , buff if +
    IncreasedAccuracy,
    //more/ less. if -, its less, 

    //more/less damage with hits
    MoreHitDamage,
    //DoT: taking damage over time: Desc - "Take" + (element) + "DoT"; Numb - float in percentage flat (i.e. 70%flat would be 0.7f) 
    TakePhysicalDoT,
    TakeFireDoT,
    TakeIceDoT,
    TakeToxicDoT,
    TakeLightningDoT,
    TakeShadowDoT,
    //booleans
    CannotTarget,
    CannotUseSpells,
    Freeze,

};
[System.Serializable]
public enum ElementalEffects
{
    Ignite,
    Chill,
    //shock stack causing them to have 7.5% reduced accuracy for 4 seconds
    //At 4 stacks, stacks are removed and target becomes electrocuted, which is blinded and silenced for 4 seconds
    Shock,
    Toxin,
    Dread,
}
[System.Serializable]
public enum Elements
{
    Physical,
    Fire,
    Ice,
    Toxic,
    Lightning,
    Shadow
}
//effect descriptin+number syntax:
//      
//      taking damage over time: Desc - "Take" + (element) + "DoT"; Numb - float in percentage flat (i.e. 70%flat would be 0.7f) 
[System.Serializable]
public class Effect
{
    bool isBuff = true;
    bool isDebuff = false;
    bool isPermanent = false;
    bool isImpairment = false; //usually only for debuffs. impairments effect can be reduced by tenacity. impairments include blind, stun, silence, chill, slow and root 
    bool refreshable = false; //only applicable to stackable effects, if true, refreshes duration of buff on gaining a new stack
    [SerializeField] float duration = 0; //only applicable to non-permanent effects
    [SerializeField] GameObject owner;
    float stacks = 1;
    float maxStacks = 1;
    EffectNames name = EffectNames.Placeholder;
    //description and number list size should always be the same
    List<EffectDescriptions> descriptions = new List<EffectDescriptions>();// a list that contains the description of one of the thing(s) this effect does. i.e. if it increases armor, it will be "increasedArmor" (exactly the way the stat is written in unitstats.cs). special effects will be written the same way it is recognized in updateStats() under effects;
    List<float> numbers = new List<float>(); //numbers that correspond to the descriptions on the previous list. i.e. if description[0] is increased armor, and number[0] is .3, then it would be 0.3(30%) increased armor
    GameObject DamageText = Resources.Load("UI/DamageText") as GameObject;
    GameObject DamageTextInstance;
    // sets the effect's values, used when creating an effect
    public void setEffect(bool isBuff, bool isDebuff, bool isPerm,bool isImp, bool refreshable, float duration, float maxStacks, EffectNames name, List<EffectDescriptions> desc, List<float> number, GameObject owner)
    {
        this.isBuff = isBuff;
        this.isDebuff = isDebuff;
        isPermanent = isPerm;
        isImpairment = isImp;
        this.maxStacks=maxStacks;
        this.refreshable = refreshable;
        this.duration = duration;
        this.name = name;
        descriptions = desc;
        numbers = number;
        this.owner = owner;
    }
    public bool getIsBuff()
    {
        return isBuff;
    }
    public bool getIsDebuff()
    {
        return isDebuff;
    }
    public bool getIsPermanent()
    {
        return isPermanent;
    }
    public bool getIsImpairment()
    {
        return isImpairment;
    }
    public bool getRefreshable()
    {
        return refreshable;
    }
    public float getDuration()
    {
        return duration;
    }
    public void setDuration(float d)
    {
        duration = d;
    }
    public EffectNames getName()
    {
        return name;
    }
    public List<EffectDescriptions> getDescriptions()
    {
        return descriptions;
    }
    public List<float> getNumbers()
    {
        return numbers;
    }
    public void addStack()
    {
        stacks++;
    }
    public float getStacks()
    {
        return stacks;
    }
    public float getMaxStacks()
    {
        return maxStacks;
    }
    //used in rare occations where the max stacks are modified through a passive or item, i.e. Rime passive 
    public void setMaxStacks(float newMax)
    {
        maxStacks = newMax;
    }
    public GameObject getOwner()
    {
        return owner;
    }
    public void initializeDamageText(Elements e, GameObject owner)
    {
        DamageTextInstance = Object.Instantiate(DamageText, owner.transform.position, owner.transform.rotation);
        DamageTextInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        DamageTextInstance.transform.position = owner.transform.position;
        DamageTextInstance.GetComponent<DamageText>().intializeText(0, e);
        DamageTextInstance.GetComponent<DamageText>().setDot(owner);
    }
    public void addAmountoDamageText (float amount)
    {
        if(DamageTextInstance!=null)
            DamageTextInstance.GetComponent<DamageText>().addAmount(amount);
    }
    }
[System.Serializable]
public class DoT
{
    float damagePerSecond = 0;
    float maxDuration = 0;
    float duration = 0;
    Elements element;
    GameObject DamageText = Resources.Load("UI/DamageText") as GameObject;
    GameObject DamageTextInstance;
    public void InitializeDoT (float damage, float maxDura, Elements e, GameObject owner)
    {
        damagePerSecond = damage;
        maxDuration = maxDura;
        duration = maxDura;
        element = e;
        DamageTextInstance = Object.Instantiate(DamageText,owner.transform.position,owner.transform.rotation);
        DamageTextInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        DamageTextInstance.transform.position = owner.transform.position;
        DamageTextInstance.GetComponent<DamageText>().intializeText(0, e);
        DamageTextInstance.GetComponent<DamageText>().setDot(owner);
    }
    public float getDamagePerSecond()
    {
        return damagePerSecond;
    }
    public void addAmountToDamageText (float amount)
    {
        DamageTextInstance.GetComponent<DamageText>().addAmount(amount);
    }
    public Elements getElement()
    {
        return element;
    }
    public float getDuration()
    {
        return duration;
    }
    public float getMaxDuration()
    {
        return maxDuration;
    }
    public void setDuration(float dura)
    {
        duration = dura;
    }
    public void setMaxDuration(float dura)
    {
        maxDuration = dura;
    }
}
public class unitstats : MonoBehaviour {
    [SerializeField]
    protected float unitLevel = 1;
    System.Random rand = new System.Random();
    [SerializeField] protected GameObject currentTarget;
    #region attributes
    //attributes
    [SerializeField] protected float strength = 0;
    [SerializeField] protected float dexterity = 0;
    [SerializeField] protected float focus = 0;
    [SerializeField] protected float vitality = 0;
    [SerializeField] protected float currentStrength = 0;
    [SerializeField] protected float currentDexterity = 0;
    [SerializeField] protected float currentFocus = 0;
    [SerializeField] protected float currentVitality = 0;
    //increased attribues
    [SerializeField] protected float increasedStrength = 1;
    [SerializeField] protected float increasedDexterity = 1;
    [SerializeField] protected float increasedFocus = 1;
    #endregion 
    #region defence
    //base defensesF
    [SerializeField] protected float baseHealth = 100;
    [SerializeField] protected float maxHealth=100;
    [SerializeField] protected float baseArmor = 0;
    [SerializeField] protected float baseFireResistance = 0;
    [SerializeField] protected float baseIceResistance = 0;
    [SerializeField] protected float baseToxicResistance = 0;
    [SerializeField] protected float baseLightningResistance = 0;
    [SerializeField] protected float blockChance = 0; //%
    [SerializeField] protected float blockAmount = 0;


    [SerializeField] protected float currentHealth;
    [SerializeField] protected float missingHealth;
    [SerializeField] protected float currentArmor = 0;
    [SerializeField] protected float currentFireResistance = 0;
    [SerializeField] protected float currentIceResistance = 0;
    [SerializeField] protected float currentToxicResistance = 0;
    [SerializeField] protected float currentLightningResistance = 0;
    [SerializeField] protected float currentShield = 0;


    
    //increased defenses %
    [SerializeField] protected float increasedHealth = 0;
    [SerializeField] protected float increasedArmor = 0;
    [SerializeField] protected float increasedFireResistance = 0;
    [SerializeField] protected float increasedIceResistance = 0;
    [SerializeField] protected float increasedToxicResistance = 0;
    [SerializeField] protected float increasedLightningResistance = 0;
    [SerializeField] protected float increasedBlockAmount = 0;
    [SerializeField] protected float increasedDamageTaken = 0;//multiplier for damage taken. (includes  DoT)
    [SerializeField] protected float increasedPhysicalDamageTaken = 0;
    [SerializeField] protected float increasedFireDamageTaken = 0;
    [SerializeField] protected float increasedIceDamageTaken = 0;
    [SerializeField] protected float increasedToxicDamageTaken = 0;
    [SerializeField] protected float increasedLightningDamageTaken = 0;
    [SerializeField] protected float increasedShadowDamageTaken = 0;
    [SerializeField] protected float increasedMeleeDamageTaken = 0;
    [SerializeField] protected float increasedProjectileDamageTaken = 0;
    [SerializeField] protected float increasedAoEDamageTaken = 0;
    [SerializeField] protected float physicalTakenAsFire = 0; // between 0 and 1
    [SerializeField] protected float physicalTakenAsIce = 0; // between 0 and 1
    [SerializeField] protected float physicalTakenAsToxic= 0; // between 0 and 1
    [SerializeField] protected float physicalTakenAsLightning= 0; // between 0 and 1
    [SerializeField] protected float physicalTakenAsShadow = 0; // between 0 and 1
    [SerializeField] protected float fireTakenAsShadow = 0; // between 0 and 1
    [SerializeField] protected float iceTakenAsShadow = 0; // between 0 and 1
    [SerializeField] protected float toxicTakenAsShadow = 0; // between 0 and 1
    [SerializeField] protected float lightningTakenAsShadow = 0; // between 0 and 1

    //additional added flat defenses (not sure if needed to code it yet)
    [SerializeField] protected float addedHealth = 0;
    [SerializeField] protected float addedArmor = 0;
    [SerializeField] protected float addedFireResistance = 0;
    [SerializeField] protected float addedIceResistance = 0;
    [SerializeField] protected float addedToxicResistance = 0;
    [SerializeField] protected float addedLightningResistance = 0;

    //more defenses
    [SerializeField] protected float moreHealth = 1;
    [SerializeField] protected float moreArmor = 1;
    [SerializeField] protected float moreFireResistance = 1;
    [SerializeField] protected float moreIceResistance = 1;
    [SerializeField] protected float moreToxicResistance = 1;
    [SerializeField] protected float moreLightningResistance = 1;
    [SerializeField] protected float moreDamageTaken = 0;//multiplier for damage taken.
    [SerializeField] protected float morePhysicalDamageTaken = 0;
    [SerializeField] protected float moreFireDamageTaken = 0;
    [SerializeField] protected float moreIceDamageTaken = 0;
    [SerializeField] protected float moreToxicDamageTaken = 0;
    [SerializeField] protected float moreLightningDamageTaken = 0;
    [SerializeField] protected float moreShadowDamageTaken = 0;
    #endregion
    #region offence
    //base offence
    [SerializeField] protected float weaponPhysicalDamage = 10;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float baseAttackSpeed = 1f;
    [SerializeField] protected float baseCritDamage = 1.5f; //%
    [SerializeField] protected float castSpeed;
    [SerializeField] protected float baseAccuracy = 70; //can go up to 100
    [SerializeField] protected float critChance = 0; //between 0 and 100
    [SerializeField] protected float armorPenetration = 0; //% between 0 and 1, applies for shadow damage as well
    [SerializeField] protected float firePenetration = 0;
    [SerializeField] protected float icePenetration = 0;
    [SerializeField] protected float toxicPenetration = 0;
    [SerializeField] protected float lightningPenetration = 0;
    [SerializeField] protected float fireLevel = 0;
    [SerializeField] protected float iceLevel = 0;
    [SerializeField] protected float toxicLevel = 0;
    [SerializeField] protected float lightningLevel = 0;
    [SerializeField] protected float shadowLevel = 0;
    //converted damage stats
    [SerializeField] protected float physicalToFire = 0; //between 0 and 1
    [SerializeField] protected float physicalToIce = 0;
    [SerializeField] protected float physicalToToxic = 0;
    [SerializeField] protected float physicalToLightning = 0;
    [SerializeField] protected float physicalToShadow = 0;
    [SerializeField] protected float fireToIce = 0;
    [SerializeField] protected float fireToToxic = 0;
    [SerializeField] protected float fireToLightning = 0;
    [SerializeField] protected float fireToShadow = 0;
    [SerializeField] protected float iceToToxic = 0;
    [SerializeField] protected float iceToLightning = 0;
    [SerializeField] protected float iceToShadow = 0;
    [SerializeField] protected float toxicToLightning = 0;
    [SerializeField] protected float toxicToShadow = 0;
    [SerializeField] protected float lightningToShadow = 0;
    //added as elemental damage stats
    [SerializeField] protected float physicalAddedAsFire = 0; //between 0 and 1
    [SerializeField] protected float physicalAddedAsIce = 0;
    [SerializeField] protected float physicalAddedAsToxic = 0;
    [SerializeField] protected float physicalAddedAsLightning = 0;
    [SerializeField] protected float physicalAddedAsShadow = 0;
    [SerializeField] protected float fireAddedAsIce = 0;
    [SerializeField] protected float fireAddedAsToxic = 0;
    [SerializeField] protected float fireAddedAsLightning = 0;
    [SerializeField] protected float fireAddedAsShadow = 0;
    [SerializeField] protected float iceAddedAsToxic = 0;
    [SerializeField] protected float iceAddedAsLightning = 0;
    [SerializeField] protected float iceAddedAsShadow = 0;
    [SerializeField] protected float toxicAddedAsLightning = 0;
    [SerializeField] protected float toxicAddedAsShadow = 0;
    [SerializeField] protected float lightningAddedAsShadow = 0;
    //increased self damage %
    [SerializeField] protected float increasedDamage = 0;
    [SerializeField] protected float increasedHitDamage = 0;
    [SerializeField] protected float increasedSingleTargetDamage=0;
    [SerializeField] protected float increasedPhysicalDamage=0;
    [SerializeField] protected float increasedAoEDamage=0;
    [SerializeField] protected float increasedFireDamage=0;
    [SerializeField] protected float increasedIceDamage = 0;
    [SerializeField] protected float increasedToxicDamage = 0;
    [SerializeField] protected float increasedLightningDamage = 0;
    [SerializeField] protected float increasedShadowDamage = 0;
    [SerializeField] protected float increasedDoTDamage = 0;
    [SerializeField] protected float increasedNADamage = 0;
    [SerializeField] protected float increasedSmashDamage = 0;
    [SerializeField] protected float increasedElementalDamage = 0;
    [SerializeField] protected float increasedSpellDamage = 0;
    [SerializeField] protected float increasedAttackDamage = 0;
    [SerializeField] protected float increasedMeleeDamage = 0;
    [SerializeField] protected float increasedBurnDamage = 0;
    [SerializeField] protected float increasedPoisonDamage = 0;
    [SerializeField] protected float increasedBleedDamage = 0;
    [SerializeField] protected float increasedAttackSpeed = 0;
    [SerializeField] protected float increasedCastSpeed = 0;

    //added self damage
    [SerializeField] protected float addedPhysicalDamage = 0; // below 6 are added additional to attacks and spells, but scales just like base weapon damage, for example 20%flat would still take 20% of the added damage
    [SerializeField] protected float addedFireDamage = 0;
    [SerializeField] protected float addedIceDamage = 0;
    [SerializeField] protected float addedToxicDamage = 0;
    [SerializeField] protected float addedLightningDamage = 0;
    [SerializeField] protected float addedShadowDamage = 0;
    [SerializeField] protected float addedCritDamage = 0;//% based, added onto base crit damage
    [SerializeField] protected float addedAccuracy = 0;//% based, added onto base accuracy

    //more self damage
    [SerializeField] protected float moreDamage = 1;
    [SerializeField] protected float moreHitDamage = 1;
    [SerializeField] protected float moreSingleTargetDamage = 1;
    [SerializeField] protected float morePhysicalDamage = 1;
    [SerializeField] protected float moreAoEDamage = 1;
    [SerializeField] protected float moreFireDamage = 1;
    [SerializeField] protected float moreIceDamage = 1;
    [SerializeField] protected float moreToxicDamage = 1;
    [SerializeField] protected float moreLightningDamage = 1;
    [SerializeField] protected float moreShadowDamage = 1;
    [SerializeField] protected float moreDoTDamage = 1;
    [SerializeField] protected float moreNADamage = 1;
    [SerializeField] protected float moreSmashDamage = 1;
    [SerializeField] protected float moreElementalDamage = 1;
    [SerializeField] protected float moreSpellDamage = 1;
    [SerializeField] protected float moreAttackDamage = 1;
    [SerializeField] protected float moreMeleeDamage = 1;
    [SerializeField] protected float moreBurnDamage = 1;
    [SerializeField] protected float morePoisonDamage = 1;
    [SerializeField] protected float moreBleedDamage = 1;
    [SerializeField] protected float moreCritChance = 1;
    #endregion
    #region misc stats
    //base misc stats
    [SerializeField] protected float baseStamina = 100;
    [SerializeField] protected float currentStamina = 100;
    [SerializeField] protected float missingStamina = 0;
    [SerializeField] protected float maxStamina = 100;
    [SerializeField] protected float baseStaminaRegen = 25;//per second
    [SerializeField] protected float currentStaminaRegen = 25;
    [SerializeField] protected float baseStaminaRegenDelay = 2;//seconds before degen 
    [SerializeField] protected float currentStaminaRegenDelay = 2;//seconds before degen 
    [SerializeField] protected float cooldown = 1;//multiplier for cooldown of skills
    [SerializeField] protected float lifeSteal = 0;//% based. multiply it to every hit to heal that amount
    [SerializeField] protected float baseRage = 60;
    [SerializeField] protected float currentRage = 0;
    [SerializeField] protected float maxRage = 60;
    [SerializeField] protected float baseRageDegen = 5;//per second
    [SerializeField] protected float currentRageDegen = 5;//per second
    [SerializeField] protected float baseRageDegenDelay = 4;//seconds without hitting or getting hit causes degen
    [SerializeField] protected float currentRageDegenDelay = 4;//seconds without hitting or getting hit causes degen
    [SerializeField] protected float cost = 1;//multiplier for cost of skills
    [SerializeField] protected float baseIgniteChance = 10;//0 to 100
    [SerializeField] protected float baseChillChance = 10;
    [SerializeField] protected float baseShockChance = 10;
    [SerializeField] protected float baseToxinChance = 10;
    [SerializeField] protected float baseDreadChance = 10;
    [SerializeField] protected float igniteStacks =0; //current stacks on this unit
    [SerializeField] protected float chillStacks = 0;
    [SerializeField] protected float shockStacks = 0;
    [SerializeField] protected float toxinStacks = 0;
    [SerializeField] protected float dreadStacks = 0;
    [SerializeField] protected float bleedStacks = 0;
    [SerializeField] protected float maxIgniteStacks = 4; //max stack before it turns into upgraded effect
    [SerializeField] protected float maxChillStacks = 5;
    [SerializeField] protected float maxShockStacks =3;
    [SerializeField] protected float maxToxinStacks = 4;
    [SerializeField] protected float maxDreadStacks = 4;
    [SerializeField] protected float currentIgniteAmount = 0; //total flat of ignite this unit takes every second
    [SerializeField] protected float healthRegen = 0;//per second, updated in update to match % health regens
    [SerializeField] protected float healing = 1; //multiplier for healing. includes health regen, life steal, and flat heals
    [SerializeField] protected float tenacity = 0; //% based, reduces impairment effects by that amount

    [SerializeField] protected float baseDashSpeed = 1;
    [SerializeField] protected float baseMovementSpeed = 10f;
    [SerializeField] protected float currentDashSpeed = 1;
    [SerializeField] protected float currentMovementSpeed = 10f;


    [SerializeField] protected bool canTarget = true;
    [SerializeField] protected bool canUseSpells = true;
    [SerializeField] protected bool isFrozen = false;
    //increased misc stats
    [SerializeField] protected float increasedStaminaRegenDelay = 0; //mostly reduced, so should be - most of the time
    [SerializeField] protected float increasedStaminaCost = 0; //mostly reduced
    [SerializeField] protected float increasedStaminaRegen = 0; //mostly reduced
    [SerializeField] protected float increasedCooldown = 0; //mostly reduced
    [SerializeField] protected float increasedLifeSteal=0;
    [SerializeField] protected float increasedAoE=0;
    [SerializeField] protected float increasedResourceCost = 0; //mostly reduced

    [SerializeField] protected float increasedHealing = 0;
    [SerializeField] protected float increasedDashSpeed = 0;
    [SerializeField] protected float increasedMovementSpeed = 0;
    [SerializeField] protected float increasedGenericSpeed = 0; // used for animations with no multipliers, such as idle
    [SerializeField] protected float increasedSkillDuration = 0;
    [SerializeField] protected float increasedResourceGeneration = 0; //increased multiplier for amount of resource generated, whether is it flat amount or regeneration
    //added misc stats
    [SerializeField] protected float addedStamina = 0;
    [SerializeField] protected float addedStaminaRegen = 0;
    [SerializeField] protected float addedRage = 0;
    [SerializeField] protected float addedRageRegen = 0;
    [SerializeField] protected float addedIgniteChance = 0;
    [SerializeField] protected float addedChillChance = 0;
    [SerializeField] protected float addedShockChance = 0;
    [SerializeField] protected float addedToxinChance = 0;
    [SerializeField] protected float addedDreadChance = 0;
    //more misc stats
    #endregion
    #region seperate entites (projectiles/minions)
    //increased damage for seperate entities %
    [SerializeField] protected float increasedProjectileDamage = 0;
    [SerializeField] protected float increasedMinionDamage = 0;
    [SerializeField] protected float increasedTurretDamage = 0;
    [SerializeField] protected float increasedMissileDamage = 0;
    #endregion

    [SerializeField] public SpriteRenderer healthBar;
    [SerializeField] protected GameObject uiHealthBar;
    [SerializeField] protected GameObject uiHBInstance;
    bool dead = false;
    bool setCanvas = false;
    GameObject canvas;
    [SerializeField] protected Vector3 healthScale;
    //a list of enemies hit by current action to prevent double hitting, use resethitEnemyList() to reset list
    [SerializeField] protected List<GameObject> hitEnemyList = new List<GameObject>();
    //list of effects on this  unit
    [SerializeField] protected List<Effect> effectList = new List<Effect>();
    [SerializeField] protected List<EffectNames> effectOverlayList = new List<EffectNames>(); // list for the overlay efects on the unit (such as a white overlay on chilled unit)
    //list of generic DoT effects on unit (not including ignites, poisons, bleeds and special DoT variations)
    [SerializeField] protected List<DoT> DoTList = new List<DoT>();
    [SerializeField] protected GameObject igniteDamageText;
    [SerializeField] protected GameObject igniteDamageTextInstance;
    #region current action tags
   //list for current action's tag (updated based on current action)
   [SerializeField] protected Actions currentAction = Actions.Default;
    [SerializeField] protected Actions currentActionTag = Actions.Default;
    [SerializeField] protected float currentActionBasePhysicalDamage = 0;
    [SerializeField] protected float currentActionBaseFireDamage = 0;
    [SerializeField] protected float currentActionBaseIceDamage = 0;
    [SerializeField] protected float currentActionBaseToxicDamage = 0;
    [SerializeField] protected float currentActionBaseLightningDamage = 0;
    [SerializeField] protected float currentActionBaseShadowDamage = 0;
    [SerializeField] protected float currentActionCritChance = 0;
    [SerializeField] protected float currentActionCritDamage = 1.5f;
    [SerializeField] protected float currentActionLifeSteal = 0;
    [SerializeField] protected float currentActionAttackSpeed = 0;
    [SerializeField] protected float currentActionAccuracy = 0.7f;
    [SerializeField] protected float currentActionSkillDuration = 0;
    [SerializeField] protected float currentActionIncreasedCooldown = 0;
    [SerializeField] protected float currentActionIncreasedAoE = 0;
    [SerializeField] protected float currentActionIncreasedDamage = 0;
    [SerializeField] protected float currentActionIncreasedHitDamage = 0;
    [SerializeField] protected float currentActionIncreasedSingleTargetDamage = 0;
    [SerializeField] protected float currentActionIncreasedPhysicalDamage = 0;
    [SerializeField] protected float currentActionIncreasedAoEDamage = 0;
    [SerializeField] protected float currentActionIncreasedFireDamage = 0;
    [SerializeField] protected float currentActionIncreasedIceDamage = 0;
    [SerializeField] protected float currentActionIncreasedToxicDamage = 0;
    [SerializeField] protected float currentActionIncreasedLightningDamage = 0;
    [SerializeField] protected float currentActionIncreasedShadowDamage = 0;
    [SerializeField] protected float currentActionIncreasedDoTDamage = 0;
    [SerializeField] protected float currentActionIncreasedNADamage = 0;
    [SerializeField] protected float currentActionIncreasedSmashDamage = 0;
    [SerializeField] protected float currentActionIncreasedElementalDamage = 0;
    [SerializeField] protected float currentActionIncreasedSpellDamage = 0;
    [SerializeField] protected float currentActionIncreasedAttackDamage = 0;
    [SerializeField] protected float currentActionIncreasedMeleeDamage = 0;
    [SerializeField] protected float currentActionIncreasedBurnDamage = 0;
    [SerializeField] protected float currentActionIncreasedPoisonDamage = 0;
    [SerializeField] protected float currentActionIncreasedBleedDamage = 0;
    [SerializeField] protected float currentActionIncreasedSkillDuration = 0;
    [SerializeField] protected float currentActionPhysicalToFire = 0; //between 0 and 1
    [SerializeField] protected float currentActionPhysicalToIce = 0;
    [SerializeField] protected float currentActionPhysicalToToxic = 0;
    [SerializeField] protected float currentActionPhysicalToLightning = 0;
    [SerializeField] protected float currentActionPhysicalToShadow = 0;
    [SerializeField] protected float currentActionFireToIce = 0;
    [SerializeField] protected float currentActionFireToToxic = 0;
    [SerializeField] protected float currentActionFireToLightning = 0;
    [SerializeField] protected float currentActionFireToShadow = 0;
    [SerializeField] protected float currentActionIceToToxic = 0;
    [SerializeField] protected float currentActionIceToLightning = 0;
    [SerializeField] protected float currentActionIceToShadow = 0;
    [SerializeField] protected float currentActionToxicToLightning = 0;
    [SerializeField] protected float currentActionToxicToShadow = 0;
    [SerializeField] protected float currentActionLightningToShadow = 0;
    [SerializeField] protected float currentActionPhysicalAddedAsFire = 0; //between 0 and 1
    [SerializeField] protected float currentActionPhysicalAddedAsIce = 0;
    [SerializeField] protected float currentActionPhysicalAddedAsToxic = 0;
    [SerializeField] protected float currentActionPhysicalAddedAsLightning = 0;
    [SerializeField] protected float currentActionPhysicalAddedAsShadow = 0;
    [SerializeField] protected float currentActionFireAddedAsIce = 0;
    [SerializeField] protected float currentActionFireAddedAsToxic = 0;
    [SerializeField] protected float currentActionFireAddedAsLightning = 0;
    [SerializeField] protected float currentActionFireAddedAsShadow = 0;
    [SerializeField] protected float currentActionIceAddedAsToxic = 0;
    [SerializeField] protected float currentActionIceAddedAsLightning = 0;
    [SerializeField] protected float currentActionIceAddedAsShadow = 0;
    [SerializeField] protected float currentActionToxicAddedAsLightning = 0;
    [SerializeField] protected float currentActionToxicAddedAsShadow = 0;
    [SerializeField] protected float currentActionLightningAddedAsShadow = 0;
    [SerializeField] protected float currentActionArmorPenetration = 0;
    [SerializeField] protected float currentActionFirePenetration = 0;
    [SerializeField] protected float currentActionIcePenetration = 0;
    [SerializeField] protected float currentActionToxicPenetration = 0;
    [SerializeField] protected float currentActionLightningPenetration = 0;
    [SerializeField] protected float currentActionIgniteChance = 25;
    [SerializeField] protected float currentActionChillChance = 25;
    [SerializeField] protected float currentActionShockChance = 25;
    [SerializeField] protected float currentActionToxinChance = 25;
    [SerializeField] protected float currentActionDreadChance = 25;

    [SerializeField] protected float currentIgniteExplosionDamage = 0; //this amount is for whem you cause an ignite explosion on an enemy, and it will change this amount to match the damage of the explosion

    [SerializeField] bool isAttack = false;
    [SerializeField] bool isSpell = false;
    [SerializeField] bool isSingleTarget = false;
    [SerializeField] bool isAoE = false;
    [SerializeField] bool isNA = false;
    [SerializeField] bool isSmash = false;
    [SerializeField] bool isMovement = false;
    [SerializeField] bool isDuration = false;
    [SerializeField] bool isMelee = false;
    [SerializeField] bool isProjectile = false;
    [SerializeField] bool isHealing = false;
    [SerializeField] bool isDash = false;
    [SerializeField] bool isSteel = false;
    [SerializeField] bool isWrath = false;
    [SerializeField] bool isCrit = false;
    [SerializeField] bool missedHit = false;
    #endregion
    //current defense tags. updated whenever defences are changed
    #region FX
    [SerializeField] protected GameObject IgniteExplosion;
    [SerializeField] protected GameObject IgniteExplosionInstance;
    #endregion
    void Start () {
		
	}

    protected virtual void Awake()
    {
        ClearCurrentTags();
        igniteDamageText = Resources.Load("UI/DamageText") as GameObject;
        IgniteExplosion = Resources.Load("FX/ignite_explosion") as GameObject;
        uiHealthBar = Resources.Load<GameObject>("UI/ui_healthDisplay");
        uiHBInstance = Instantiate(uiHealthBar);
        //uiHBInstance.GetComponent<FollowOwner>().initialize(1.5f);
        uiHBInstance.transform.SetParent(transform);
        uiHBInstance.transform.localPosition = new Vector3(0, 6f, 0);
        maxHealth = (baseHealth + 10 * unitLevel) * (1 + increasedHealth)*moreHealth;
        currentHealth = maxHealth;
        healthBar = uiHBInstance.transform.Find("HealthBar").GetComponent<SpriteRenderer>();
        

        healthScale = healthBar.transform.localScale;
        missingHealth = maxHealth - currentHealth;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        //update health bar to reflect current health
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - currentHealth / maxHealth);
        float scaleX = 1;
        if (maxHealth != 0)
        {
            scaleX = healthScale.x * currentHealth / maxHealth;
        }
    }
    // Update is called once per frame
    protected virtual void Update () {
        if (!setCanvas)
        {
            canvas = GameObject.Find("Canvas");
            setCanvas = true;
        }
        updateStats();
        updateDoT();
    }
    protected virtual void updateStats()
    {

        //uiHBInstance.transform.position = new Vector3(0, 1.5f, 0);
        //reset increased defenses
        addedHealth = 0; // add stats from items like this


        //reset increased offenses
        //reset increased misc

        //update item stats
        //base defenses
        addedHealth += 0; // add stats from items like this
        baseArmor = 2000; //take armor from every piece of equipment and apply increases and more to it
        //base offenses
        addedAccuracy = 0;
        //base misc
        lifeSteal = 0;
        canTarget = true;
        canUseSpells = true;
        if(tenacity>0.75f)
        {
            tenacity = 0.75f;
        }
        #region update from effects
        //update from effects
        List<Effect> stackedEffects = new List<Effect>();
        for (int i = 0; i < effectList.Count; i++)
        {
            Effect e = effectList[i];
            bool inList = false;
            e.setDuration(e.getDuration()-Time.deltaTime);
            if (e.getDuration() <= 0)
            {
                RemoveEffectAtPosition(i);
                i--;
                updateEffectStacks();
                updateEffectOverlays();
            }
            else
            {
                for (int j = 0; j < stackedEffects.Count; j++)
                {
                    if (stackedEffects[j] == effectList[i])
                    {
                        inList = true;
                        stackedEffects[j].addStack();
                    }
                }
                if (!inList)
                {
                    stackedEffects.Add(effectList[i]);
                }
            }
        }

        for (int i = 0; i<stackedEffects.Count; i++)
        {
            Effect eff = stackedEffects[i];
            List<EffectDescriptions> desc = eff.getDescriptions();
            List<float> numb = eff.getNumbers();
            
            for (int j = 0; j < desc.Count; j++)
            {
                EffectDescriptions s = desc[j];
                float n = numb[j];
                if (s == EffectDescriptions.TakePhysicalDoT || s == EffectDescriptions.TakeFireDoT || s == EffectDescriptions.TakeIceDoT || s == EffectDescriptions.TakeToxicDoT || s == EffectDescriptions.TakeLightningDoT || s == EffectDescriptions.TakeShadowDoT)
                {

                    //Elements ele;
                    //if (s == EffectDescriptions.TakePhysicalDoT)
                    //    ele = Elements.Physical;
                    //else if (s == EffectDescriptions.TakeFireDoT)
                    //    ele = Elements.Fire;
                    //else if (s == EffectDescriptions.TakeIceDoT)
                    //    ele = Elements.Ice;
                    //else if (s == EffectDescriptions.TakeToxicDoT)
                    //    ele = Elements.Toxic;
                    //else if (s == EffectDescriptions.TakeLightningDoT)
                    //    ele = Elements.Lightning;
                    //else if (s == EffectDescriptions.TakeShadowDoT)
                    //    ele = Elements.Shadow;
                    float damageTaken = 0;
                    //dot damage equals final dot damage times the amount of ignite stacks
                    if (eff.getName() == EffectNames.Ignite)
                    {
                        damageTaken = TakeDoT(currentIgniteAmount, Elements.Fire, Time.deltaTime);
                        igniteDamageTextInstance.GetComponent<DamageText>().addAmount(damageTaken);
                    }
                    else
                    {

                        for (int k = 0; k < effectList.Count; k++)
                        {
                            if (effectList[k].getName() == eff.getName())
                            {
                                for (int l = 0; l < effectList[k].getDescriptions().Count; l++)
                                {
                                    if (effectList[k].getDescriptions()[l] == EffectDescriptions.TakePhysicalDoT)
                                        damageTaken = TakeDoT(effectList[k].getNumbers()[l], Elements.Physical, Time.deltaTime);
                                    else if (effectList[k].getDescriptions()[l] == EffectDescriptions.TakeFireDoT)
                                        damageTaken = TakeDoT(effectList[k].getNumbers()[l], Elements.Fire, Time.deltaTime);
                                    else if (effectList[k].getDescriptions()[l] == EffectDescriptions.TakeIceDoT)
                                        damageTaken = TakeDoT(effectList[k].getNumbers()[l], Elements.Ice, Time.deltaTime);
                                    else if (effectList[k].getDescriptions()[l] == EffectDescriptions.TakeToxicDoT)
                                        damageTaken = TakeDoT(effectList[k].getNumbers()[l], Elements.Toxic, Time.deltaTime);
                                    else if (effectList[k].getDescriptions()[l] == EffectDescriptions.TakeLightningDoT)
                                        damageTaken = TakeDoT(effectList[k].getNumbers()[l], Elements.Lightning, Time.deltaTime);
                                    else if (effectList[k].getDescriptions()[l] == EffectDescriptions.TakeShadowDoT)
                                        damageTaken = TakeDoT(effectList[k].getNumbers()[l], Elements.Shadow, Time.deltaTime);

                                }
                                effectList[k].addAmountoDamageText(damageTaken);
                            }
                        }
                    }


                }
                else if (s == EffectDescriptions.IncreasedAccuracy)
                {
                    if (eff.getIsImpairment())
                    {
                        n *= (1 - tenacity);
                    }
                    addedAccuracy += n;
                }
                else if (s == EffectDescriptions.MoreHitDamage)
                {
                    moreHitDamage *= n;
                }
                else if (s == EffectDescriptions.CannotTarget)
                {
                    canTarget = false;
                }
                else if (s == EffectDescriptions.CannotUseSpells)
                {
                    canUseSpells = false;
                }
                else if (s == EffectDescriptions.IncreasedSpeed)
                {
                    increasedAttackSpeed += n;
                    increasedCastSpeed += n;
                    increasedMovementSpeed += n;
                    increasedDashSpeed += n;
                    if(n<0)
                    {
                        increasedGenericSpeed += n;
                    }
                }
            }

        }
        #endregion

        //increased and more multipliers
        //defenses
        //offenses



        //misc

        //compile defense and misc stats
        maxHealth = (baseHealth + 10 * unitLevel + addedHealth) * (1+increasedHealth )* moreHealth;
        missingHealth = maxHealth - currentHealth;
        if(currentHealth<=0)
        {
            currentHealth = 0;
            if(!dead)
            {
                dead = true;
                GetComponent<unitControl>().TriggerDeath();
            }
        }
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        //update health bar to reflect current health
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - currentHealth / maxHealth);
        float scaleX = 1;
        if (maxHealth != 0)
        {
            scaleX = healthScale.x * currentHealth / maxHealth;
        }
        healthBar.transform.localScale = new Vector3(scaleX, 1, 1);
        currentArmor = (baseArmor + addedArmor) * (1 + increasedArmor) * moreArmor;
        currentFireResistance = (baseFireResistance + addedFireResistance) * (1 + increasedFireResistance) * moreFireResistance;
        currentIceResistance = (baseIceResistance + addedIceResistance) * (1 + increasedIceResistance) * moreIceResistance;
        currentToxicResistance = (baseToxicResistance + addedToxicResistance) * (1 + increasedToxicResistance) * moreToxicResistance;
        currentLightningResistance = (baseLightningResistance + addedLightningResistance) * (1 + increasedLightningResistance) * moreLightningResistance;
        maxStamina = (baseStamina + unitLevel + addedStamina);
        missingStamina = maxStamina - currentStamina;
        currentStaminaRegen = baseStaminaRegen +addedStaminaRegen;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
        currentStaminaRegenDelay = baseStaminaRegenDelay * (1 + increasedStaminaRegenDelay);
        cooldown = 1 + increasedCooldown;
        lifeSteal *= (1+increasedLifeSteal);
        maxRage = (baseRage + addedRage);
        currentRageDegen = baseRageDegen - addedRageRegen;
        currentDashSpeed = baseDashSpeed * (1 + increasedDashSpeed);
        currentMovementSpeed = baseMovementSpeed * (1 + increasedMovementSpeed);
    }
    void updateDoT()
    {
        #region update from DoTs
        for (int i = 0; i < DoTList.Count; i++)
        {
            DoT dot = DoTList[i];
            float time = Time.deltaTime;
            dot.setDuration(dot.getDuration() - time);
            if(dot.getDuration()<=0)
            {
                removeDoTAtPosition(i);
                i--;
            }
            else
            {
                float damage = TakeDoT(dot.getDamagePerSecond(),dot.getElement(),time);
                dot.addAmountToDamageText(damage);
                
            }
        }
        #endregion
    }
    //placeholder damage take
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public float getAttackSpeed()
    {
        return currentActionAttackSpeed;
    }

    public float getMovementSpeed()
    {
        return currentMovementSpeed;
    }
    public float getMoveMulti()
    {
        return (1 + increasedMovementSpeed);
    }
    public float getDashSpeed()
    {
        return currentDashSpeed;
    }
    public float getGenericMulti()
    {
        return (1 + increasedGenericSpeed);
    }
    //array in form of {max health, current health, missing health}
    public float[] getHealth()
    {
        return new float[] { maxHealth, currentHealth, missingHealth };
    }
    //resets action of unit. 
    //action: new action the unit is currently performing
    //usually used for skills that actually use the blade to hit
    public void setCurrentAction (Actions action)
    {
        currentAction = action;
        resethitEnemyList();
        setCurrentTags(action);
    }
    //reset tags of the current unit without reseting hit list.
    //hit list of enemies calculated seperately in another class to prevent colliding with this class' list
    //
    public void setCurrentTags(Actions action)
    {
        ClearCurrentTags();
        currentActionTag = action;
        //player actions
        if (action == Actions.NA1 || action == Actions.NA2 || action == Actions.NA3 || action == Actions.NA4)
        {
            currentActionBasePhysicalDamage = weaponPhysicalDamage + addedPhysicalDamage;
            currentActionBaseFireDamage = addedFireDamage + fireLevel * weaponPhysicalDamage / 10;
            currentActionBaseIceDamage = addedIceDamage + iceLevel * weaponPhysicalDamage / 10;
            currentActionBaseToxicDamage = addedToxicDamage + toxicLevel * weaponPhysicalDamage / 10;
            currentActionBaseLightningDamage = addedLightningDamage + lightningLevel * weaponPhysicalDamage / 10;
            currentActionBaseShadowDamage = addedShadowDamage + shadowLevel * weaponPhysicalDamage / 10;
            isAttack = true;
            isNA = true;
            isAoE = true;
            isMelee = true;
            currentActionAttackSpeed *= 1.5f;
        }

        //monster actions
        //rakab actions
        else if (action==Actions.rakabNA)
        {
            currentActionBasePhysicalDamage = 3 + 1 * unitLevel;
        }
        else if (action==Actions.rakabSlam)
        {
            currentActionBasePhysicalDamage = 5 + 2 * unitLevel;
            isAoE = true;
        }
        //generic actions

        else if(action==Actions.IgniteExplosion)
        {
            currentActionBaseFireDamage = currentIgniteExplosionDamage;
            currentActionIgniteChance = 0;
            currentActionAccuracy = 100;
            isAoE = true;
        }
    }



    //reset all the current action tags to default
    public void ClearCurrentTags()
    {
        currentActionBasePhysicalDamage = 0;
        currentActionBaseFireDamage = 0;
        currentActionBaseIceDamage = 0;
        currentActionBaseToxicDamage = 0;
        currentActionBaseLightningDamage = 0;
        currentActionBaseShadowDamage = 0;
        currentActionCritChance = 0;
        currentActionCritDamage = baseCritDamage+addedCritDamage;
        currentActionLifeSteal = lifeSteal * (1+increasedLifeSteal);
        currentActionAttackSpeed = baseAttackSpeed * (1 + increasedAttackSpeed);
        currentActionAccuracy = baseAccuracy + addedAccuracy;
        currentActionSkillDuration = 0;
        currentActionIncreasedCooldown = 0;
        currentActionIncreasedAoE = increasedAoE;
        currentActionIncreasedDamage = increasedDamage;
        currentActionIncreasedHitDamage = increasedHitDamage;
        currentActionIncreasedSingleTargetDamage = increasedSingleTargetDamage;
        currentActionIncreasedPhysicalDamage = increasedPhysicalDamage;
        currentActionIncreasedAoEDamage = increasedAoEDamage;
        currentActionIncreasedFireDamage = increasedFireDamage;
        currentActionIncreasedIceDamage = increasedIceDamage;
        currentActionIncreasedToxicDamage = increasedToxicDamage;
        currentActionIncreasedLightningDamage = increasedLightningDamage;
        currentActionIncreasedShadowDamage = increasedShadowDamage;
        currentActionIncreasedDoTDamage = increasedDoTDamage;
        currentActionIncreasedNADamage = increasedNADamage;
        currentActionIncreasedSmashDamage = increasedSmashDamage;
        currentActionIncreasedElementalDamage = increasedElementalDamage;
        currentActionIncreasedSpellDamage = increasedSpellDamage;
        currentActionIncreasedAttackDamage = increasedAttackDamage;
        currentActionIncreasedMeleeDamage = increasedMeleeDamage;
        currentActionIncreasedBurnDamage = increasedBurnDamage;
        currentActionIncreasedPoisonDamage = increasedPoisonDamage;
        currentActionIncreasedBleedDamage = increasedBleedDamage;
        currentActionIncreasedSkillDuration = increasedSkillDuration;
        currentActionPhysicalToFire = physicalToFire; //between 0 and 1
        currentActionPhysicalToIce = physicalToIce;
        currentActionPhysicalToToxic = physicalToToxic;
        currentActionPhysicalToLightning = physicalToLightning;
        currentActionPhysicalToShadow = physicalToShadow;
        currentActionFireToIce = fireToIce;
        currentActionFireToToxic = fireToToxic;
        currentActionFireToLightning = fireToLightning;
        currentActionFireToShadow = fireToShadow;
        currentActionIceToToxic = iceToToxic;
        currentActionIceToLightning = iceToLightning;
        currentActionIceToShadow = iceToShadow;
        currentActionToxicToLightning = toxicToLightning;
        currentActionToxicToShadow = toxicToShadow;
        currentActionLightningToShadow = lightningToShadow;
        currentActionPhysicalAddedAsFire = physicalAddedAsFire; //between 0 and 1
        currentActionPhysicalAddedAsIce = physicalAddedAsIce;
        currentActionPhysicalAddedAsToxic = physicalAddedAsToxic;
        currentActionPhysicalAddedAsLightning = physicalAddedAsLightning;
        currentActionPhysicalAddedAsShadow = physicalAddedAsShadow;
        currentActionFireAddedAsIce = fireAddedAsIce;
        currentActionFireAddedAsToxic = fireAddedAsToxic;
        currentActionFireAddedAsLightning = fireAddedAsLightning;
        currentActionFireAddedAsShadow = fireAddedAsShadow;
        currentActionIceAddedAsToxic = iceAddedAsToxic;
        currentActionIceAddedAsLightning = iceAddedAsLightning;
        currentActionIceAddedAsShadow = iceAddedAsShadow;
        currentActionToxicAddedAsLightning = toxicAddedAsLightning;
        currentActionToxicAddedAsShadow = toxicAddedAsShadow;
        currentActionLightningAddedAsShadow = lightningAddedAsShadow;
        currentActionArmorPenetration = armorPenetration;
        currentActionFirePenetration = firePenetration;
        currentActionIcePenetration = icePenetration;
        currentActionToxicPenetration = toxicPenetration;
        currentActionLightningPenetration = lightningPenetration;
        currentActionIgniteChance = baseIgniteChance+addedIgniteChance;
        currentActionChillChance = baseChillChance + addedChillChance;
        currentActionShockChance = baseShockChance + addedShockChance;
        currentActionToxinChance = baseToxinChance + addedToxinChance;
        currentActionDreadChance = baseDreadChance + addedDreadChance;
        isAttack = false;
        isSpell = false;
        isSingleTarget = false;
        isAoE = false;
        isNA = false;
        isSmash = false;
        isMovement = false;
        isDuration = false;
        isMelee = false;
        isProjectile = false;
        isHealing = false;
        isDash = false;
        isSteel = false;
        isWrath = false;
        isCrit = false;
        missedHit = false;
        
    }

    public void hitTarget(GameObject target)
    {
        currentTarget = target;
        bool alreadyHit = false;
        for (int i = 0; i < hitEnemyList.Count; i++)
        {
            if ((hitEnemyList[i]) == target)
            {
                alreadyHit = true;
            }
        }
        if (!alreadyHit)
        {
            hitEnemyList.Add(target);
            float[,] dmgValues = hitDamage();
            //accuracy roll
            float accRoll = rand.Next(1, 101);
            if (accRoll > currentActionAccuracy)
            {
                missHit();
            }
            //roll and apply crit
            float critRoll = rand.Next(1, 101);
            if (critRoll <= currentActionCritChance)
            {
                float[] critNumbers = critStrike(dmgValues[0,0], dmgValues[1, 0], dmgValues[2, 0], dmgValues[3, 0], dmgValues[4, 0], dmgValues[5, 0]);
                dmgValues[0, 0] = critNumbers[0];
                dmgValues[1, 0] = critNumbers[1];
                dmgValues[2, 0] = critNumbers[2];
                dmgValues[3, 0] = critNumbers[3];
                dmgValues[4, 0] = critNumbers[4];
                dmgValues[5, 0] = critNumbers[5];
            }
            float[,] hitValues = target.GetComponent<unitstats>().takeHit(dmgValues, missedHit, isCrit, isMelee, isProjectile, isAoE);
            //ApplyDotToTarget(target, 10, Elements.Toxic, 6); <== (dot testing)
            applyHitEffects(target,hitValues);
            
        }
    }
    //returns array of form (physical damage, fire, ice, toxic, lightning, shadow)
    public float[,] hitDamage ()
    {
        float physicalDamage = currentActionBasePhysicalDamage;
        float fireDamage = currentActionBaseFireDamage;
        float iceDamage = currentActionBaseIceDamage;
        float toxicDamage = currentActionBaseToxicDamage;
        float lightningDamage = currentActionBaseLightningDamage;
        float shadowDamage = currentActionBaseShadowDamage;
        float physicalAppliedToFire = 0; //% of fire damage that has been converted from physical. for example, if you are dealing 100 phys + 70 fire in addition to 30% phys to fire convertion you would be dealing 100 fire damage with 30% of it being benefited from phys. therefore it is.3
        float physicalAppliedToIce = 0; //% of ice converted from phys + (physicalAppliedToFire* % of ice converted from fire)
        float physicalAppliedToToxic = 0;
        float physicalAppliedToLightning = 0;
        float physicalAppliedToShadow = 0;
        float fireAppliedToIce = 0;
        float fireAppliedToToxic = 0;
        float fireAppliedToLightning = 0;
        float fireAppliedToShadow = 0;
        float iceAppliedToToxic = 0;
        float iceAppliedToLightning = 0;
        float iceAppliedToShadow = 0;
        float toxicAppliedToLightning = 0;
        float toxicAppliedToShadow = 0;
        float lightningAppliedToShadow = 0;
        float currentIncreases = 0; //accumilated increases
        float currentMore = 1; //accumilated more multipliers

        if (isSingleTarget)
        {
            currentIncreases += increasedSingleTargetDamage;
            currentMore *= moreSingleTargetDamage;
        }
        if (isAoE)
        {
            currentIncreases += increasedAoEDamage;
            currentMore *= moreAoEDamage;
        }
        if (isAttack)
        {
            currentIncreases += increasedAttackDamage;
            currentMore *= moreAttackDamage;
        }
        if (isSpell)
        {
            currentIncreases += increasedSpellDamage;
            currentMore *= moreSpellDamage;
        }
        if (isNA)
        {
            currentIncreases += increasedNADamage;
            currentMore *= moreNADamage;
        }
        if (isSmash)
        {
            currentIncreases += increasedSmashDamage;
            currentMore *= moreSmashDamage;
        }
        if (isMelee)
        {
            currentIncreases += increasedMeleeDamage;
            currentMore *= moreMeleeDamage;
        }
        currentActionIncreasedFireDamage += currentActionIncreasedElementalDamage;
        currentActionIncreasedIceDamage += currentActionIncreasedElementalDamage;
        currentActionIncreasedToxicDamage += currentActionIncreasedElementalDamage;
        currentActionIncreasedLightningDamage += currentActionIncreasedElementalDamage;
        currentIncreases += currentActionIncreasedHitDamage + currentActionIncreasedDamage;

        //Order of calculating damage (need to test)
        //0. get rid of excess conversions
        float totalConversion = 0;
        if (currentActionPhysicalToFire > 1)
            currentActionPhysicalToFire = 1;
        totalConversion += currentActionPhysicalToFire;
        if (currentActionPhysicalToIce + totalConversion > 1)
            currentActionPhysicalToIce = 1 - totalConversion;
        totalConversion += currentActionPhysicalToIce;
        if (currentActionPhysicalToToxic + totalConversion > 1)
            currentActionPhysicalToToxic = 1 - totalConversion;
        totalConversion += currentActionPhysicalToToxic;
        if (currentActionPhysicalToLightning + totalConversion > 1)
            currentActionPhysicalToLightning = 1 - totalConversion;
        totalConversion += currentActionPhysicalToLightning;
        if (currentActionPhysicalToShadow + totalConversion > 1)
            currentActionPhysicalToShadow = 1 - totalConversion;

        totalConversion = 0;
        if (currentActionFireToIce > 1)
            currentActionFireToIce = 1;
        totalConversion += currentActionFireToIce;
        if (currentActionFireToToxic + totalConversion > 1)
            currentActionFireToToxic = 1 - totalConversion;
        totalConversion += currentActionFireToToxic;
        if (currentActionFireToLightning + totalConversion > 1)
            currentActionFireToLightning = 1 - totalConversion;
        totalConversion += currentActionFireToLightning;
        if (currentActionFireToShadow + totalConversion > 1)
            currentActionFireToShadow = 1 - totalConversion;

        totalConversion = 0;
        if (currentActionIceToToxic > 1)
            currentActionIceToToxic = 1;
        totalConversion += currentActionIceToToxic;
        if (currentActionIceToLightning + totalConversion > 1)
            currentActionIceToLightning = 1 - totalConversion;
        totalConversion += currentActionIceToLightning;
        if (currentActionIceToShadow + totalConversion > 1)
            currentActionIceToShadow = 1 - totalConversion;

        totalConversion = 0;
        if (currentActionToxicToLightning > 1)
            currentActionToxicToLightning = 1;
        totalConversion += currentActionToxicToLightning;
        if (currentActionToxicToShadow + totalConversion > 1)
            currentActionToxicToShadow = 1 - totalConversion;

        if (currentActionLightningToShadow > 1)
            currentActionLightningToShadow = 1;
        //1. convert+added as (dont remove converted numbers)

        fireDamage += currentActionPhysicalToFire * physicalDamage + currentActionPhysicalAddedAsFire * physicalDamage;
        iceDamage += currentActionPhysicalToIce * physicalDamage + currentActionFireToIce * fireDamage + currentActionPhysicalAddedAsIce * physicalDamage + currentActionFireAddedAsIce * fireDamage;
        toxicDamage += currentActionPhysicalToToxic * physicalDamage + currentActionPhysicalAddedAsToxic * physicalDamage + currentActionFireToToxic * fireDamage + currentActionFireAddedAsToxic * fireDamage +
            currentActionIceToToxic * iceDamage + currentActionIceAddedAsToxic * iceDamage;
        lightningDamage += currentActionPhysicalToLightning * physicalDamage + currentActionPhysicalAddedAsLightning * physicalDamage + currentActionFireToLightning * fireDamage + currentActionFireAddedAsLightning * fireDamage +
            currentActionIceToLightning * iceDamage + currentActionIceAddedAsLightning * iceDamage + currentActionToxicAddedAsLightning * toxicDamage + currentActionToxicToLightning * toxicDamage;
        shadowDamage += currentActionPhysicalToShadow * physicalDamage + currentActionPhysicalAddedAsShadow * physicalDamage + currentActionFireToShadow * fireDamage + currentActionFireAddedAsShadow * fireDamage +
            currentActionIceToShadow * iceDamage + currentActionIceAddedAsShadow * iceDamage + currentActionToxicToShadow * toxicDamage + currentActionToxicAddedAsShadow * toxicDamage +
            currentActionLightningToShadow * lightningDamage + currentActionLightningAddedAsShadow * lightningDamage;

        //2. calculate ratios of convertions
        float fireTemp = 1;
        float iceTemp = 1;
        float toxicTemp = 1;
        float lightningTemp = 1;
        float shadowTemp = 1;
        if (fireDamage != 0)
        {
            fireTemp = fireDamage;
        }
        if (iceDamage != 0)
        {
            iceTemp = iceDamage;
        }
        if (toxicDamage != 0)
        {
            toxicTemp = toxicDamage;
        }
        if (lightningDamage != 0)
        { lightningTemp = lightningDamage; }
        if (shadowDamage != 0)
        {
            shadowTemp = shadowDamage;
        }
        physicalAppliedToFire = (currentActionPhysicalToFire + currentActionPhysicalAddedAsFire) / fireTemp;
        fireAppliedToIce = (currentActionFireToIce + currentActionFireAddedAsIce) / iceTemp;
        physicalAppliedToIce = (currentActionPhysicalToIce + currentActionPhysicalAddedAsIce + physicalAppliedToFire * fireAppliedToIce) / iceTemp;
        iceAppliedToToxic = (currentActionIceToToxic + currentActionIceAddedAsToxic) / toxicTemp;
        fireAppliedToToxic = (currentActionFireToToxic + currentActionFireAddedAsToxic + fireAppliedToIce * iceAppliedToToxic) / toxicTemp;
        physicalAppliedToToxic = (currentActionPhysicalToToxic + currentActionPhysicalAddedAsToxic + physicalAppliedToFire * fireAppliedToToxic + physicalAppliedToIce * iceAppliedToToxic) / toxicTemp;
        toxicAppliedToLightning = (currentActionToxicToLightning + currentActionToxicAddedAsLightning) / lightningTemp;
        iceAppliedToLightning = (currentActionIceToLightning + currentActionIceAddedAsLightning + iceAppliedToToxic * toxicAppliedToLightning) / lightningTemp;
        fireAppliedToLightning = (currentActionFireToLightning + currentActionFireAddedAsLightning + fireAppliedToIce * iceAppliedToLightning + fireAppliedToToxic * toxicAppliedToLightning) / lightningTemp;
        physicalAppliedToLightning = (currentActionPhysicalToLightning + currentActionPhysicalAddedAsLightning + physicalAppliedToFire * fireAppliedToLightning + physicalAppliedToIce * iceAppliedToLightning + physicalAppliedToToxic * toxicAppliedToLightning) / lightningTemp;
        lightningAppliedToShadow = (currentActionLightningToShadow + currentActionLightningAddedAsShadow) / shadowTemp;
        toxicAppliedToShadow = (currentActionToxicToShadow + currentActionToxicAddedAsShadow + toxicAppliedToLightning * lightningAppliedToShadow) / shadowTemp;
        iceAppliedToShadow = (currentActionIceToShadow + currentActionIceAddedAsShadow + iceAppliedToLightning * lightningAppliedToShadow + iceAppliedToToxic * toxicAppliedToShadow) / shadowTemp;
        fireAppliedToShadow = (currentActionFireToShadow + currentActionFireAddedAsShadow + fireAppliedToIce * iceAppliedToShadow + fireAppliedToToxic * toxicAppliedToShadow + fireAppliedToLightning * lightningAppliedToShadow) / shadowTemp;
        physicalAppliedToShadow = (currentActionPhysicalToShadow + currentActionPhysicalAddedAsShadow + physicalAppliedToFire * fireAppliedToShadow + physicalAppliedToIce * iceAppliedToShadow + physicalAppliedToToxic * toxicAppliedToShadow + physicalAppliedToLightning * lightningAppliedToShadow) / shadowTemp;
        //3. remove converted numbers
        physicalDamage -= (currentActionPhysicalToFire + currentActionPhysicalToIce + currentActionPhysicalToToxic + currentActionPhysicalToLightning + currentActionPhysicalToShadow) * physicalDamage;
        fireDamage -= (currentActionFireToIce + currentActionFireToToxic + currentActionFireToLightning + currentActionFireToShadow) * fireDamage;
        iceDamage -= (currentActionIceToToxic + currentActionIceToLightning + currentActionIceToShadow) * iceDamage;
        toxicDamage -= (currentActionToxicToLightning + currentActionToxicToShadow) * toxicDamage;
        lightningDamage -= (currentActionLightningToShadow) * lightningDamage;

        //4. apply more and increased to damage
        physicalDamage *= (1 + currentIncreases + currentActionIncreasedPhysicalDamage) *
            (currentMore * morePhysicalDamage);
        fireDamage *= (1 + currentIncreases + currentActionIncreasedFireDamage + currentActionIncreasedPhysicalDamage * physicalAppliedToFire) *
            (currentMore * moreFireDamage * (1 + (morePhysicalDamage - 1) * physicalAppliedToFire));
        iceDamage *= (1 + currentIncreases + currentActionIncreasedIceDamage + currentActionIncreasedFireDamage * fireAppliedToIce + currentActionIncreasedPhysicalDamage * physicalAppliedToIce) * (currentMore * moreIceDamage * (1 + (morePhysicalDamage - 1) * physicalAppliedToIce) * (1 + (moreFireDamage - 1) * fireAppliedToIce));
        toxicDamage *= (1 + currentIncreases + currentActionIncreasedToxicDamage + currentActionIncreasedIceDamage * iceAppliedToToxic + currentActionIncreasedFireDamage * fireAppliedToToxic + currentActionIncreasedPhysicalDamage * physicalAppliedToToxic) *
            (currentMore * moreToxicDamage * (1 + (morePhysicalDamage - 1) * physicalAppliedToToxic) * (1 + (moreFireDamage - 1) * fireAppliedToToxic) * (1 + (moreIceDamage - 1) * iceAppliedToToxic));
        lightningDamage *= (1 + currentIncreases + currentActionIncreasedLightningDamage + currentActionIncreasedToxicDamage * toxicAppliedToLightning + currentActionIncreasedIceDamage * iceAppliedToLightning + currentActionIncreasedFireDamage * fireAppliedToLightning + currentActionIncreasedPhysicalDamage * physicalAppliedToLightning) *
            (currentMore * moreLightningDamage * (1 + (morePhysicalDamage - 1) * physicalAppliedToLightning) * (1 + (moreFireDamage - 1) * fireAppliedToLightning) * (1 + (moreIceDamage - 1) * iceAppliedToLightning) * (1 + (moreToxicDamage - 1) * toxicAppliedToLightning));
        shadowDamage *= (1 + currentIncreases + currentActionIncreasedShadowDamage + currentActionIncreasedLightningDamage * lightningAppliedToShadow + currentActionIncreasedToxicDamage * toxicAppliedToShadow + currentActionIncreasedIceDamage * iceAppliedToShadow + currentActionIncreasedFireDamage * fireAppliedToShadow + currentActionIncreasedPhysicalDamage * physicalAppliedToShadow) *
            (currentMore * moreShadowDamage * (1 + (morePhysicalDamage - 1) * physicalAppliedToShadow) * (1 + (moreFireDamage - 1) * fireAppliedToShadow) * (1 + (moreIceDamage - 1) * iceAppliedToShadow) * (1 + (moreToxicDamage - 1) * toxicAppliedToShadow) * (1 + (moreLightningDamage - 1) * lightningAppliedToShadow));




        //apply hit to target
        float[] physicalHit = new float[] { physicalDamage, currentActionArmorPenetration };
        float[] fireHit = new float[] { fireDamage, currentActionFirePenetration };
        float[] iceHit = new float[] { iceDamage, currentActionIcePenetration };
        float[] toxicHit = new float[] { toxicDamage, currentActionToxicPenetration };
        float[] lightningHit = new float[] { lightningDamage, currentActionLightningPenetration };
        float shadowHit = shadowDamage;
        float[,] values = new float[6, 2];
        values[0,0] = physicalHit[0];
        values[0, 1] = physicalHit[1];
        values[1, 0] = fireHit[0];
        values[1, 1] = fireHit[1];
        values[2, 0] = iceHit[0];
        values[2, 1] = iceHit[1];
        values[3, 0] = toxicHit[0];
        values[3, 1] = toxicHit[1];
        values[4, 0] = lightningHit[0];
        values[4, 1] = lightningHit[1];
        values[5, 0] = shadowHit;
       
        return values; 

    }
    public float[] critStrike(float phys, float fire, float ice, float toxic, float lightning, float shadow)
    {
        //effects that occur after critting


        float[] damages = new float[5];
        damages[0] = phys * currentActionCritDamage;
        damages[1] = fire * currentActionCritDamage;
        damages[2] = ice * currentActionCritDamage;
        damages[3] = toxic * currentActionCritDamage;
        damages[4] = lightning * currentActionCritDamage;
        damages[5] = shadow* currentActionCritDamage;
        isCrit = true;

        return damages;
    }
    //effects that occur if unit misses a hit
    public void missHit()
    {
        currentActionCritChance -= 10;
        missedHit = true;
    }
    //used after hitting enemy, applying procedures such as lifesteal and elemental effects
    //damage value is in the form of{ physical dealt, fire dealt, ice dealt, toxic dealt, lightning dealt, shadow dealt, armor penetrated (phys and shadow damage combined),fire penetrated, ice penetrated, toxic penetrated,lighnting penetrated}
    public void applyHitEffects(GameObject target, float[,] damageValues)
    {
        //lifesteal
        float stolenLife = currentActionLifeSteal * (damageValues[0,0] + damageValues[1,0] + damageValues[2,0] + damageValues[3,0] + damageValues[4,0] + damageValues[5,0]);
        stolenLife *= (1 + increasedHealing);
        heal(stolenLife);

        //elemental effects
        //ignite (fire)
        if (damageValues[1,0] > 0)
        {
            
            float igniteRoll = rand.Next(1, 101);
            if (igniteRoll < currentActionIgniteChance)
            {
                ApplyElementalEffect(ElementalEffects.Ignite, target);
            }
        }
        //chill (ice)
        if(damageValues[2,0]>0)
        {
            float chillRoll = rand.Next(1, 101);
            if(chillRoll<currentActionChillChance)
            {
                ApplyElementalEffect(ElementalEffects.Chill, target);
            }
        }
        //Toxin (toxic)
        if (damageValues[3, 0] > 0)
        {
            float toxinRoll = rand.Next(1, 101);
            if (toxinRoll < currentActionToxinChance)
            {
                ApplyElementalEffect(ElementalEffects.Toxin, target);
            }
        }
        //shock (lightning)
        if (damageValues[4,0] > 0)
        {
            float shockRoll = rand.Next(1, 101);
            if (shockRoll < currentActionShockChance)
            {
                ApplyElementalEffect(ElementalEffects.Shock, target);
            }
        }
        //Dread (shadow)
        if (damageValues[5, 0] > 0)
        {
            float dreadRoll = rand.Next(1, 101);
            if (dreadRoll < currentActionDreadChance)
            {
                ApplyElementalEffect(ElementalEffects.Dread, target);
            }
        }
    }

    //phys, fire, ice, toxic, lightning, and shadow takes a array of 2 containing number of damage and % damage penetrated, for example, {250, .3}. shadow only contains damage. also contains if the hit missed
    //returns an array with the form of {phys damage dealt, fire dealt, ice dealt, toxic dealt, lightning dealt, shadow dealt, armor penetrated(counts for shadow damage), fire ", ice ", toxic ", lightning "
    public float[,] takeHit(float[,] damageValues,bool isMiss, bool isCrit, bool isMelee, bool isProjectile, bool isAoE)
    {
        GameObject DamageText = Resources.Load("UI/DamageText") as GameObject;
        
        float reductionEffectiveness = 1;
        if (isMiss)
        {
            reductionEffectiveness += 0.3f;
        }
        float penA=currentArmor;
        float penF= currentFireResistance;
        float penI= currentIceResistance;
        float penT= currentToxicResistance;
        float penL= currentLightningResistance;
        float pPen = damageValues[0,1];
        float fPen = damageValues[1, 1];
        float iPen = damageValues[2, 1];
        float tPen = damageValues[3, 1];
        float lPen = damageValues[4, 1];

        if (float.IsNaN(pPen))
            pPen = 0;
        if (float.IsNaN(fPen))
            fPen = 0;
        if (float.IsNaN(iPen))
            iPen = 0;
        if (float.IsNaN(tPen))
            tPen = 0;
        if (float.IsNaN(lPen))
            lPen = 0;

        if (penA>0)
            penA = currentArmor * (1 - pPen);
        if (penF > 0)
            penF = currentFireResistance * (1 - fPen);
        if (penI > 0)
            penI = currentIceResistance * (1 - iPen);
        if (penT > 0)
            penT = currentToxicResistance * (1 - tPen);
        if (penL > 0)
            penL = currentLightningResistance * (1 - lPen);
        //physical damage taken converted 
        float p = damageValues[0, 0];
        float f = damageValues[1, 0];
        float i = damageValues[2, 0];
        float t = damageValues[3, 0];
        float l = damageValues[4, 0];
        float s = damageValues[5, 0];
        if (float.IsNaN(p))
            p = 0;
        if (float.IsNaN(f))
            f = 0;
        if (float.IsNaN(i))
            i = 0;
        if (float.IsNaN(t))
            t = 0;
        if (float.IsNaN(l))
            l = 0;
        if (float.IsNaN(s))
            s = 0;
        f += p * physicalTakenAsFire;
        i += p * physicalTakenAsIce;
        t += p * physicalTakenAsToxic;
        l += p * physicalTakenAsLightning;
        s += p * physicalAddedAsShadow + f * fireTakenAsShadow + i * iceTakenAsShadow + t * toxicTakenAsShadow + l * lightningTakenAsShadow;

        p -= (p * physicalTakenAsFire + p * physicalTakenAsIce + p * physicalTakenAsToxic + p * physicalTakenAsLightning + p * physicalAddedAsShadow);
        f -= (f * fireTakenAsShadow);
        i -= (i * iceTakenAsShadow);
        t -= (t * toxicTakenAsShadow);
        l -= (lightningTakenAsShadow);


        float increasedDamageTakenTotal = increasedDamageTaken;
        if(isMelee)
        {
            increasedDamageTakenTotal += increasedMeleeDamageTaken;
        }
        if(isProjectile)
        {
            increasedDamageTakenTotal += increasedProjectileDamageTaken;
        }
        if(isAoE)
        {
            increasedDamageTakenTotal += increasedAoEDamageTaken;
        }

                //reduction formula: armor/(3000+armor) as a multiplier to hits
        float physicalTaken = (1- penA*reductionEffectiveness / (3000 + penA)+ increasedDamageTakenTotal+ increasedPhysicalDamageTaken)*p;
        float fireTaken = (1-penF*reductionEffectiveness/ (3000 + penF) + increasedDamageTakenTotal + increasedFireDamageTaken)*f;
        float iceTaken = (1-penI*reductionEffectiveness / (3000 + penI) + increasedDamageTakenTotal + increasedIceDamageTaken)*i;
        float toxicTaken = (1-penT*reductionEffectiveness / (3000 + penT) + increasedDamageTakenTotal + increasedToxicDamageTaken)*t;
        float lightningTaken = (1-penL*reductionEffectiveness / (3000 + penL) + increasedDamageTakenTotal + increasedLightningDamageTaken)*l;
        float shadowTaken = (1-penA*reductionEffectiveness / (3000 + penA) + increasedDamageTakenTotal + increasedShadowDamageTaken)*s;
        //take the hit
        float totalTaken = (physicalTaken + fireTaken + iceTaken + toxicTaken + lightningTaken + shadowTaken);
        currentHealth -= totalTaken;
        //damage text: 1. list of highest damage element 2. create and set priority of damage type (lowest priority first)
        List<Elements> damageList = new List<Elements>();
        List<Elements> orderedDamageList = new List<Elements>();
        if(physicalTaken!=0)
            damageList.Add(Elements.Physical);
        if (fireTaken != 0)
            damageList.Add(Elements.Fire);
        if (iceTaken != 0)
            damageList.Add(Elements.Ice);
        if (toxicTaken != 0)
            damageList.Add(Elements.Toxic);
        if (lightningTaken != 0)
            damageList.Add(Elements.Lightning);
        if (shadowTaken != 0)
            damageList.Add(Elements.Shadow);
        if(damageList.Count!=0)
            orderedDamageList.Add(damageList[0]);
        //1. created ordered list
        for(int e = 1; e < damageList.Count; e++)
        {
            bool added = false;
            for(int o = 0;o < orderedDamageList.Count;o++)
            {
                float elementOne = 0;
                float elementTwo = 0;
                if (damageList[e] == Elements.Fire)
                    elementOne = fireTaken;
                else if (damageList[e] == Elements.Ice)
                    elementOne = iceTaken;
                else if (damageList[e] == Elements.Toxic)
                    elementOne = toxicTaken;
                else if (damageList[e] == Elements.Lightning)
                    elementOne = lightningTaken;
                else if (damageList[e] == Elements.Shadow)
                    elementOne = shadowTaken;
                if (orderedDamageList[o] == Elements.Physical)
                    elementTwo = physicalTaken;
                else if (orderedDamageList[o] == Elements.Fire)
                    elementTwo = fireTaken;
                else if (orderedDamageList[o] == Elements.Ice)
                    elementTwo = iceTaken;
                else if (orderedDamageList[o] == Elements.Toxic)
                    elementTwo = toxicTaken;
                else if (orderedDamageList[o] == Elements.Lightning)
                    elementTwo = lightningTaken;
                else if (orderedDamageList[o] == Elements.Shadow)
                    elementTwo = shadowTaken;
                if(elementOne<=elementTwo&&!added)
                {
                    orderedDamageList.Insert(o, damageList[e]);
                    added = true;
                }
            }
            if (!added)
                orderedDamageList.Add(damageList[e]);
        }
        //2. create a damageText for 
        for(int e = 0; e <orderedDamageList.Count;e++)
        {
            GameObject DamageTextInstance;
            DamageTextInstance = Instantiate(DamageText, transform.position, transform.rotation);
            DamageTextInstance.transform.SetParent(canvas.transform,false);
            DamageTextInstance.transform.position = transform.position;
            float damage = 0;
            if (orderedDamageList[e] == Elements.Physical)
                damage = physicalTaken;
            else if (orderedDamageList[e] == Elements.Fire)
                damage = fireTaken;
            else if (orderedDamageList[e] == Elements.Ice)
                damage = iceTaken;
            else if (orderedDamageList[e] == Elements.Toxic)
                damage = toxicTaken;
            else if (orderedDamageList[e] == Elements.Lightning)
                damage = lightningTaken;
            else if (orderedDamageList[e] == Elements.Shadow)
                damage = shadowTaken;
            DamageTextInstance.GetComponent<DamageText>().intializeText(damage, orderedDamageList[e]);
            DamageTextInstance.GetComponent<DamageText>().setPriority(orderedDamageList.Count - e-1);
        }

        //interrupt chance



        return new float[,] { { physicalTaken, penA * currentArmor }, { fireTaken, penF * currentFireResistance }, { iceTaken, penI * currentIceResistance },{ toxicTaken, penT * currentToxicResistance },{ lightningTaken, penL * currentLightningResistance },{shadowTaken, penA * currentArmor } };
    }
    
    public void heal (float amount)
    {
        currentHealth += amount*(1+increasedHealing);
    }


    public void ApplyDotToTarget(GameObject target, float flat, Elements element, float duration)
    {
        
        float damage = DoTDamage(flat, element, false, false);
        target.GetComponent<unitstats>().getDoT(damage, element, duration);
    }
    public void getDoT(float damage, Elements element, float duration)
    {
        DoT dot = new DoT();
        dot.InitializeDoT(damage, duration, element, gameObject);
        DoTList.Add(dot);
    }
    //gets the final number of the DoT damage after applying increases and more bonuses
    //flat damage is % flat per second
    float DoTDamage(float flat,Elements element, bool isBleed, bool isPoison)
    {
        float totalDamage = flat;
        float increasedDamageByElement = 0;
        float moreDamageByElement = 1;
        if (element == Elements.Physical)
        {
            increasedDamageByElement += increasedPhysicalDamage;
            moreDamageByElement *= morePhysicalDamage;
            if (isBleed)
            {
                increasedDamageByElement += increasedBleedDamage;
                moreDamageByElement *= moreBleedDamage;
            }
        }
        if (element==Elements.Fire)
        {
            increasedDamageByElement += increasedFireDamage + increasedElementalDamage+increasedBurnDamage;
            moreDamageByElement *= moreFireDamage * moreElementalDamage * moreBurnDamage;
        }
        else if (element == Elements.Ice)
        {
            increasedDamageByElement += increasedIceDamage + increasedElementalDamage;
            moreDamageByElement *= moreIceDamage * moreElementalDamage;
        }
        else if (element == Elements.Toxic)
        {
            increasedDamageByElement += increasedToxicDamage + increasedElementalDamage;
            moreDamageByElement *= moreToxicDamage * moreElementalDamage;
            if(isPoison)
            {
                increasedDamageByElement += increasedPoisonDamage;
                moreDamageByElement *= morePoisonDamage; 
            }
        }
        else if (element == Elements.Lightning)
        {
            increasedDamageByElement += increasedLightningDamage + increasedElementalDamage;
            moreDamageByElement *= moreLightningDamage * moreElementalDamage;
        }
        else if (element == Elements.Shadow)
        {
            increasedDamageByElement += increasedShadowDamage;
            moreDamageByElement *= moreShadowDamage;
        }
        totalDamage = totalDamage * (1+increasedDamage + increasedDoTDamage + increasedDamageByElement) * moreDamage * moreDoTDamage * moreDamageByElement;
        return totalDamage;
    }
    public float TakeDoT(float damage, Elements element, float time)
    {
        float finalDamage = damage;
        float resistToElement = 0;
        float increasedTakenToElement = 0;
        if(element==Elements.Physical)
        {
            resistToElement = currentArmor;
            increasedTakenToElement = increasedPhysicalDamageTaken;
        }
        else if (element == Elements.Fire)
        {
            resistToElement = currentFireResistance;
            increasedTakenToElement = increasedFireDamageTaken;
        }
        else if (element == Elements.Ice)
        {
            resistToElement = currentIceResistance;
            increasedTakenToElement = increasedIceDamageTaken;
        }
        else if (element == Elements.Toxic)
        {
            resistToElement = currentToxicResistance;
            increasedTakenToElement = increasedToxicDamageTaken;
        }
        else if (element == Elements.Lightning)
        {
            resistToElement = currentLightningResistance;
            increasedTakenToElement = increasedLightningDamageTaken;
        }
        else if (element == Elements.Shadow)
        {
            resistToElement = currentArmor;
            increasedTakenToElement = increasedShadowDamageTaken;
        }
        finalDamage = finalDamage * (1 - resistToElement / (3000 + resistToElement) +increasedDamageTaken+ increasedTakenToElement)*time;
        currentHealth -= finalDamage;
        return finalDamage;
    }
    void removeDoTAtPosition(int position)
    {
        DoTList.RemoveAt(position);
    }
    //types: Ignite, Shock, Chill, Toxin, Dread
    public void ApplyElementalEffect(ElementalEffects type, GameObject target)
    {
        if (type == ElementalEffects.Ignite)
        {
            ApplyEffect(EffectNames.Ignite, target, 4);
        }
        else if (type == ElementalEffects.Chill)
        {
            ApplyEffect(EffectNames.Chill, target, 4);
        }
        else if (type == ElementalEffects.Toxin)
        {
            ApplyEffect(EffectNames.Toxin, target, 4);
        }
        else if (type == ElementalEffects.Shock)
        {
            ApplyEffect(EffectNames.Shock, target, 4);
        }
        else if (type == ElementalEffects.Dread)
        {
            ApplyEffect(EffectNames.Dread, target, 4);
        }
    }

    ///name: name of effect being applied, i.e. "RecklessStrikesEffect"
    ///target: game object being applied to (can be self)
    public void ApplyEffect(EffectNames name, GameObject target, float duration)
    {
        //if name is of a skill, and buff/debuff is from an upgrade, apply it if upgrade is taken, i.e. if Savage Strike 3A is true, then apply buff/debuff
        Effect e = new Effect();
        bool isBuff = false;
        bool isDebuff = false;
        bool isPermanent = false;
        bool isImpairment = false;
        bool refreshable = false; //only applicable to stackable effects, if true, refreshes duration of buff on gaining a new stack
        float dura = duration; //only applicable to non-permanent effects
        float maxStacks = 1;
        EffectNames effectName = name;
        //description and number list size should always be the same
        List<EffectDescriptions> descriptions = new List<EffectDescriptions>();// a list that contains the description of one of the thing(s) this effect does. i.e. if it increases armor, it will be "increasedArmor" (exactly the way the stat is written in unitstats.cs). special effects will be written the same way it is recognized in updateStats() under effects;
        List<float> numbers = new List<float>(); //numbers that correspond to the descriptions on the previous list. i.e. if description[0] is increased armor, and number[0] is .3, then it would be 0.3(30%) increased armor
        //set isBuff, effectName, descriptions and numbers based on name
        //1. elemental effects

        if (name == EffectNames.Ignite)
        {
            isDebuff = true;
            maxStacks = maxIgniteStacks;
            refreshable = true;
            descriptions.Add(EffectDescriptions.TakeFireDoT);
            float dot = DoTDamage(weaponPhysicalDamage * 0.2f, Elements.Fire, false, false);
            numbers.Add(dot);
        }
        else if (name == EffectNames.Chill)
        {
            isDebuff = true;
            maxStacks = maxChillStacks;
            refreshable = true;
            isImpairment = true;
            descriptions.Add(EffectDescriptions.IncreasedSpeed);
            numbers.Add(-0.05f);
        }
        else if (name == EffectNames.Freeze)
        {
            isDebuff=true;
            refreshable = true;
            descriptions.Add(EffectDescriptions.Freeze);
            numbers.Add(0);
        }
        else if (name == EffectNames.Toxin)
        {
        }
        else if (name == EffectNames.Infect)
        {
        }
        else if (name == EffectNames.Shock)
        {
            isDebuff = true;
            maxStacks = maxShockStacks;
            refreshable = true;
            descriptions.Add(EffectDescriptions.IncreasedAccuracy);
            numbers.Add(-7.5f);
        }
        else if (name == EffectNames.Electrocute)
        {
            isDebuff = true;
            refreshable = true;
            ApplyEffect(EffectNames.Blind, gameObject, 4);
            numbers.Add(0);
            ApplyEffect(EffectNames.Silence, gameObject, 4);
            numbers.Add(0);
        }
        else if (name == EffectNames.Dread)
        {

        }


        //2. effects from skills 
        else if (name == EffectNames.RecklessStrikesEffect)
        {
            isBuff = true;
            isDebuff = true;
            isPermanent = true;
            descriptions.Add(EffectDescriptions.IncreasedAccuracy);
            numbers.Add(-20f);
            descriptions.Add(EffectDescriptions.MoreHitDamage);
            numbers.Add(1.35f);
        }
        //3. effects from passives
        //4. crowd control effects
        else if (name == EffectNames.Blind)
        {
            isDebuff = true;
            refreshable = true;
            isImpairment = true;
            descriptions.Add(EffectDescriptions.IncreasedAccuracy);
            numbers.Add(-30f);
            descriptions.Add(EffectDescriptions.CannotTarget);
            numbers.Add(0);
        }
        else if (name == EffectNames.Silence)
        {
            isDebuff = true;
            refreshable = true;
            descriptions.Add(EffectDescriptions.CannotUseSpells);
            numbers.Add(0);
        }
        e.setEffect(isBuff, isDebuff, isPermanent, isImpairment, refreshable, dura, maxStacks, effectName, descriptions, numbers, gameObject);
        target.GetComponent<unitstats>().GetEffect(e);
    }
    public void GetEffect(Effect e)
    {
        Effect temp = e;
        float currentStacks = 0;
        //if effect is on list, 
        for(int i = 0;i<effectList.Count;i++)
        {
            if(effectList[i].getName()==temp.getName())
            {
                currentStacks++;
                if (temp.getRefreshable())
                {
                    if (effectList[i].getDuration() < e.getDuration())
                    effectList[i].setDuration(e.getDuration());
                }
            }
        }
        if (currentStacks == 0)
        {
            if (e.getName() == EffectNames.Ignite)
            {
                igniteDamageTextInstance = Instantiate(igniteDamageText, transform.position, transform.rotation);
                igniteDamageTextInstance.transform.SetParent(canvas.transform, false);
                igniteDamageTextInstance.transform.position = transform.position;
                igniteDamageTextInstance.GetComponent<DamageText>().intializeText(0, Elements.Fire);
                igniteDamageTextInstance.GetComponent<DamageText>().setDot(gameObject);
                
            }
            for (int i = 0; i < temp.getDescriptions().Count; i++) {
                if (temp.getDescriptions()[i] == EffectDescriptions.TakePhysicalDoT)
                {
                    temp.initializeDamageText(Elements.Physical, gameObject);
                }
                else if (temp.getDescriptions()[i] == EffectDescriptions.TakeFireDoT)
                {
                    temp.initializeDamageText(Elements.Fire, gameObject);
                }
                else if (temp.getDescriptions()[i] == EffectDescriptions.TakeIceDoT)
                {
                    temp.initializeDamageText(Elements.Ice, gameObject);
                }
                else if (temp.getDescriptions()[i] == EffectDescriptions.TakeToxicDoT)
                {
                    temp.initializeDamageText(Elements.Toxic, gameObject);
                }
                else if (temp.getDescriptions()[i] == EffectDescriptions.TakeLightningDoT)
                {
                    temp.initializeDamageText(Elements.Lightning, gameObject);
                }
                else if (temp.getDescriptions()[i] == EffectDescriptions.TakeShadowDoT)
                {
                    temp.initializeDamageText(Elements.Shadow, gameObject);
                }
            }
                

        }
        //if effect is not at max stacks
        if (currentStacks < temp.getMaxStacks())
        {
            effectList.Add(temp);
            if (temp.getName() == EffectNames.Ignite)
            {
                currentIgniteAmount += temp.getNumbers()[0];
                //add more flames to yourself
                GetComponent<unitControl>().AddFlames();
            }
        }
        // if effect is at max stacks
        else
        {
            //if it is a shock stack, remove them and electrocute this unit
            if (temp.getName() == EffectNames.Shock)
            {
                ApplyEffect(EffectNames.Electrocute, gameObject, 4);
                RemoveEffectByAmount(EffectNames.Shock, temp.getMaxStacks());
            }
            //if it is maxed ignite stacks, remove them and cause an ignite explosion
            else if(temp.getName() == EffectNames.Ignite)
            {
                
                IgniteExplosionInstance = Instantiate(IgniteExplosion, transform.position, transform.rotation);
                IgniteExplosionInstance.GetComponent<ignite_explosion>().initialize(e.getOwner(),false, false, true,0, currentActionIncreasedAoE);
                int igniteLocation = FindEffect(EffectNames.Ignite);
                float currentIgniteDuration = 0;
                if (igniteLocation != -1)
                {
                    currentIgniteDuration = effectList[igniteLocation].getDuration();
                }
                float explosionDamage = currentIgniteAmount * currentIgniteDuration;
                e.getOwner().GetComponent<unitstats>().setIgniteExplosionDamage(explosionDamage);
                e.getOwner().GetComponent<unitstats>().setCurrentTags(Actions.IgniteExplosion);
                float currAcc = e.getOwner().GetComponent<unitstats>().currentActionAccuracy;
                float currCrit = e.getOwner().GetComponent<unitstats>().currentActionCritChance;
                float currCritMulti = e.getOwner().GetComponent<unitstats>().currentActionCritDamage;
                float currIg = e.getOwner().GetComponent<unitstats>().currentActionIgniteChance;
                float currChi = e.getOwner().GetComponent<unitstats>().currentActionChillChance;
                float currTox = e.getOwner().GetComponent<unitstats>().currentActionToxinChance;
                float currSho = e.getOwner().GetComponent<unitstats>().currentActionShockChance;
                float currDre = e.getOwner().GetComponent<unitstats>().currentActionDreadChance;
                float currLS = e.getOwner().GetComponent<unitstats>().currentActionLifeSteal;
                IgniteExplosionInstance.GetComponent<ignite_explosion>().initializeHit(e.getOwner().GetComponent<unitstats>().hitDamage(), currAcc ,currCrit,currCritMulti ,currIg, currChi,currTox, currSho, currDre, currLS);


                currentIgniteAmount = 0;
                RemoveEffectByAmount(EffectNames.Ignite, temp.getMaxStacks());
            }
            else if(temp.getName() == EffectNames.Chill)
            {
                ApplyEffect(EffectNames.Freeze, gameObject, 4);
                RemoveEffectByAmount(EffectNames.Chill, temp.getMaxStacks());
            }
            //if effect is refreshable, refresh all stacks' duration
            else if (temp.getRefreshable())
            {
                for (int i = 0; i < effectList.Count; i++)
                {
                    if (effectList[i].getName() == e.getName())
                    {
                        if (effectList[i].getDuration()<e.getDuration())
                            effectList[i].setDuration(e.getDuration());
                    }
                }
            }
            //if it is not refreshable, refresh the oldest stack
            else
            {
                //position of the stack with the least remaining duration
                int oldestPosition = 0;
                float leastDuration = temp.getDuration();
                for (int i = 0; i < effectList.Count; i++)
                {
                    if (effectList[i].getDuration() < leastDuration)
                    {
                        oldestPosition = i;
                    }
                }
                RemoveEffectAtPosition(oldestPosition);
                effectList.Add(temp);
            }
        }
        updateEffectStacks();
        updateEffectOverlays();
    }
    //
    public void RemoveEffectByAmount(EffectNames e, float amount)
    {
        
        int count = 0;
        for(int i = 0;i< effectList.Count; i++)
        {
            if (effectList[i].getName()==e&&count<amount)
            {
                
                RemoveEffectAtPosition(i);
                count++;
                i--;
            }
        }
    }
    void RemoveEffectAtPosition(int position)
    {
        if (effectList[position].getName() == EffectNames.Ignite && !HasEffect(EffectNames.Ignite))
        {
            currentIgniteAmount = 0;
        }
        effectList.RemoveAt(position);


    }
    bool HasEffect (EffectNames name)
    {
        for(int i=0; i<effectList.Count;i++)
        {
            if(effectList[i].getName()==name)
            {
                return true;
            }
        }
        return false;
    }
    int FindEffect(EffectNames name)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i].getName() == name)
            {
                return i;
            }
        }
        return -1;
    }

    //void addEffectOverlay (Sprite sprite)
    //{

    //}
    //stacks used to display effectoverlays or use elsewhere in the game
    void updateEffectStacks()
    {
        igniteStacks = 0;
        chillStacks = 0;
        shockStacks = 0;
        toxinStacks = 0;
        bleedStacks = 0;
        dreadStacks = 0;
        bool freezeDebuff = false;
        
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i].getName() == EffectNames.Ignite&&igniteStacks<maxIgniteStacks)
            {
                igniteStacks++;
            }
            else if (effectList[i].getName() == EffectNames.Chill && chillStacks < maxChillStacks)
            {
                chillStacks++;
            }
            else if (effectList[i].getName() == EffectNames.Toxin && toxinStacks < maxToxinStacks)
            {
                toxinStacks++;
            }
            else if (effectList[i].getName() == EffectNames.Shock && shockStacks < maxShockStacks)
            {
                shockStacks++;
            }
            else if (effectList[i].getName() == EffectNames.Dread&& dreadStacks < maxDreadStacks)
            {
                dreadStacks++;
            }
            if (effectList[i].getName() == EffectNames.Bleed)
            {
                bleedStacks++;
            }
            if (effectList[i].getName() == EffectNames.Freeze)
            {
                freeze();
                freezeDebuff = true;
            }
        }
        if(!freezeDebuff&&isFrozen)
        {
            unfreeze();
        }
    }
    void updateEffectOverlays()
    {
        if (igniteStacks > 0 && !effectOverlayList.Contains(EffectNames.Ignite))
        {
            
            effectOverlayList.Add(EffectNames.Ignite);
            AddEffectOverlay("FX Overlays/ignite_overlay");
            //add flames to yourself
            GetComponent<unitControl>().AddFlames();
        }
        else if (igniteStacks < 1 && effectOverlayList.Contains(EffectNames.Ignite))
        {
            effectOverlayList.Remove(EffectNames.Ignite);
            RemoveEffectOverlay("FX Overlays/ignite_overlay");
        }
        if (chillStacks>0&&!effectOverlayList.Contains(EffectNames.Chill))
        {
           effectOverlayList.Add(EffectNames.Chill);
            AddEffectOverlay("FX Overlays/chill_overlay");
        }
        else if(chillStacks<1&& effectOverlayList.Contains(EffectNames.Chill))
        {
            effectOverlayList.Remove(EffectNames.Chill);
            RemoveEffectOverlay("FX Overlays/chill_overlay");
        }
        if(isFrozen&&!effectOverlayList.Contains(EffectNames.Freeze))
        {
            effectOverlayList.Add(EffectNames.Freeze);
            AddEffectOverlay("FX Overlays/freeze_overlay");
        }
        else if(!isFrozen&&effectOverlayList.Contains(EffectNames.Freeze))
        {
            effectOverlayList.Remove(EffectNames.Freeze);
            RemoveEffectOverlay("FX Overlays/freeze_overlay");
        }
        if (shockStacks > 0 && !effectOverlayList.Contains(EffectNames.Shock))
        {
            effectOverlayList.Add(EffectNames.Shock);
            AddEffectOverlay("FX Overlays/shock_overlay");
        }
        else if (shockStacks < 1 && effectOverlayList.Contains(EffectNames.Shock))
        {
            effectOverlayList.Remove(EffectNames.Shock);
            RemoveEffectOverlay("FX Overlays/shock_overlay");
        }
        if(HasEffect(EffectNames.Electrocute)&& !effectOverlayList.Contains(EffectNames.Electrocute))
        {
            effectOverlayList.Add(EffectNames.Electrocute);
            AddEffectOverlay("FX Overlays/electrocute_overlay");
        }
        else if (!HasEffect(EffectNames.Electrocute) && effectOverlayList.Contains(EffectNames.Electrocute))
        {
            effectOverlayList.Remove(EffectNames.Electrocute);
            RemoveEffectOverlay("FX Overlays/electrocute_overlay");
        }

    }
    void AddEffectOverlay(string s)
    {
        Sprite tempSprite;
        tempSprite = Resources.Load<Sprite>(s);
        GetComponent<unitControl>().AddEffectOverlay(tempSprite);
    }
    void RemoveEffectOverlay(string s)
    {
        Sprite tempSprite;
        tempSprite = Resources.Load<Sprite>(s);
        GetComponent<unitControl>().RemoveEffectOverlay(tempSprite);
    }

    public void setIgniteExplosionDamage(float amount)
    {
        currentIgniteExplosionDamage = amount;
    }

    public GameObject TargetWithCursor()
    {
        GameObject g = new GameObject();

        return g;
    }

    public ProjectileScript InitializeProjectile(string type)
    {
        ProjectileScript p = new ProjectileScript();
        //finalize projectile's stats after increases to dmg, speed, etc.

        return p;
    }
    public void resethitEnemyList()
    {
        hitEnemyList = new List<GameObject>();
    }

    public float getFlat(float f)
    {
        return f * weaponPhysicalDamage / 100;
    }

    public bool Frozen()
    {
        return isFrozen;
    }
    public void freeze()
    {
        isFrozen = true;
    }
    public void unfreeze()
    {
        isFrozen = false;
    }
    //public void printCurrentTargetEffects()
    //{
    //    currentTarget.GetComponent<unitstats>().printEffects();
    //}
    //public void printEffects()
    //{
        
    //    for (int i = 0; i < effects.Count; i++)
    //    {
    //        print(effects[i].getName());
    //    }
    //}

}
