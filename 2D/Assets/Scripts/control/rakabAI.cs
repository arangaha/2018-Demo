using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakabAI : monsterAI{
    System.Random rand = new System.Random();
    //attack enablers
    public bool iniNA = false;
    public bool iniSlam = false;
    
    //bodyparts
    public GameObject head;
    public GameObject thorax;
    public GameObject abdomen;
    public GameObject handfront;
    public GameObject handback;
    public GameObject bicepfront;
    public GameObject bicepback;
    public GameObject legfront1;
    public GameObject legfront2;
    public GameObject legback1;
    public GameObject legback2;


    protected override void Awake()
    {
        base.Awake();
        head = transform.Find("rakab head").gameObject;
        thorax = transform.Find("rakab thorax").gameObject;
        abdomen = transform.Find("rakab abdomen").gameObject;
        handfront = transform.Find("rakab hand front").gameObject;
        handback = transform.Find("rakab hand back").gameObject;
        bicepfront = transform.Find("rakab bicep front").gameObject;
        bicepback = transform.Find("rakab bicep back").gameObject;
        legfront1 = transform.Find("rakab leg front 1").gameObject;
        legfront2 = transform.Find("rakab leg front 2").gameObject;
        legback1 = transform.Find("rakab leg back 1").gameObject;
        legback2 = transform.Find("rakab leg back 2").gameObject;

        head.GetComponent<bodypart>().setEquip("_rakab");
        thorax.GetComponent<bodypart>().setEquip("_rakab");
        abdomen.GetComponent<bodypart>().setEquip("_rakab");
        handfront.GetComponent<bodypart>().setEquip("_rakab");
        handback.GetComponent<bodypart>().setEquip("_rakab");
        bicepfront.GetComponent<bodypart>().setEquip("_rakab");
        bicepback.GetComponent<bodypart>().setEquip("_rakab");
        legfront1.GetComponent<bodypart>().setEquip("_rakab");
        legfront2.GetComponent<bodypart>().setEquip("_rakab");
        legback1.GetComponent<bodypart>().setEquip("_rakab");
        legback2.GetComponent<bodypart>().setEquip("_rakab");
    }
    public override void AddEffectOverlay(Sprite sprite)
    {
        head.GetComponent<bodypart>().applyOverlay(sprite);
        thorax.GetComponent<bodypart>().applyOverlay(sprite);
        abdomen.GetComponent<bodypart>().applyOverlay(sprite);
        handfront.GetComponent<bodypart>().applyOverlay(sprite);
        handback.GetComponent<bodypart>().applyOverlay(sprite);
        bicepfront.GetComponent<bodypart>().applyOverlay(sprite);
        bicepback.GetComponent<bodypart>().applyOverlay(sprite);
        legfront1.GetComponent<bodypart>().applyOverlay(sprite);
        legfront2.GetComponent<bodypart>().applyOverlay(sprite);
        legback1.GetComponent<bodypart>().applyOverlay(sprite);
        legback2.GetComponent<bodypart>().applyOverlay(sprite);
    }
    public override void RemoveEffectOverlay(Sprite sprite)
    {
        head.GetComponent<bodypart>().removeOverlay(sprite);
        thorax.GetComponent<bodypart>().removeOverlay(sprite);
        abdomen.GetComponent<bodypart>().removeOverlay(sprite);
        handfront.GetComponent<bodypart>().removeOverlay(sprite);
        handback.GetComponent<bodypart>().removeOverlay(sprite);
        bicepfront.GetComponent<bodypart>().removeOverlay(sprite);
        bicepback.GetComponent<bodypart>().removeOverlay(sprite);
        legfront1.GetComponent<bodypart>().removeOverlay(sprite);
        legfront2.GetComponent<bodypart>().removeOverlay(sprite);
        legback1.GetComponent<bodypart>().removeOverlay(sprite);
        legback2.GetComponent<bodypart>().removeOverlay(sprite);
    }
    public override void AddFlames()
    {
        head.GetComponent<bodypart>().AddFlames();
        thorax.GetComponent<bodypart>().AddFlames();
        abdomen.GetComponent<bodypart>().AddFlames();
        handfront.GetComponent<bodypart>().AddFlames();
        handback.GetComponent<bodypart>().AddFlames();
        bicepfront.GetComponent<bodypart>().AddFlames();
        bicepback.GetComponent<bodypart>().AddFlames();
        legfront1.GetComponent<bodypart>().AddFlames();
        legfront2.GetComponent<bodypart>().AddFlames();
        legback1.GetComponent<bodypart>().AddFlames();
        legback2.GetComponent<bodypart>().AddFlames();
    }
    public override void ChangeOpacity(float opacity)
    {
        head.GetComponent<bodypart>().ChangeOpacity(opacity);
        thorax.GetComponent<bodypart>().ChangeOpacity(opacity);
        abdomen.GetComponent<bodypart>().ChangeOpacity(opacity);
        handfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        handback.GetComponent<bodypart>().ChangeOpacity(opacity);
        bicepfront.GetComponent<bodypart>().ChangeOpacity(opacity);
        bicepback.GetComponent<bodypart>().ChangeOpacity(opacity);
        legfront1.GetComponent<bodypart>().ChangeOpacity(opacity);
        legfront2.GetComponent<bodypart>().ChangeOpacity(opacity);
        legback1.GetComponent<bodypart>().ChangeOpacity(opacity);
        legback2.GetComponent<bodypart>().ChangeOpacity(opacity);
    }

    
    //action weights
    int normalAttackWeight;
    int slamAttackWeight;
    int totalWeight;

    protected override void Start()
    {
        base.Start();
        normalAttackWeight = 2;
        slamAttackWeight = normalAttackWeight + 2;
        totalWeight = slamAttackWeight+1;
    }

    protected override void Update()
    {
        base.Update();
        if(dying)
        {
            opacity -= decayRate;
            ChangeOpacity(opacity);
            if(opacity<=0)
            {
                Destroy(gameObject);
            }
        }

    }

    //targeting skills
    protected override void pickAction()
    {
        //throw new System.NotImplementedException();
        int actionRoll = rand.Next(1, totalWeight);
        if(getUnitsByDistance(false, AggroRange).Count>0)
        {
           
            if (actionRoll <= normalAttackWeight)
            {
                TargetNormalAttack();
            }
            else if (actionRoll <= slamAttackWeight)
            {
                TargetSlamAttack();
            }
        }
    }

    protected override void startTriggeredAction(Actions a)
    {
       
        if(a == Actions.rakabNA)
        {
            NormalAttack();
        }
        else if(a == Actions.rakabSlam)
        {
            SlamAttack();
        }
        else
        {
            pickAction();
        }
    }

    //target a unit and track it, and set action queue for target
    void TargetNormalAttack()
    {
        GameObject target = closestUnit(getUnitsByDistance(false,AggroRange));
        SetTrackUnit(target, 5);
        QueueAction(Actions.rakabNA, true);

    }
    void TargetSlamAttack()
    {
        if (getUnitsByDistance(false, 5).Count > 0)
        {
            SlamAttack();
        }
        else
        {
            GameObject target = closestUnit(getUnitsByDistance(false, AggroRange));
            SetTrackUnit(target, 8);
            QueueAction(Actions.rakabSlam, false);
        }
    }








    void NormalAttack()
    {
        anim.SetFloat("AttackNumber", 1); 
        setAnimation("TriggerAttack");

    }
    public void startNormalAttack()
    {
        iniNA = false;
        attacking = true;

    }
    public void inNormalAttack()
    {
        setAction(Actions.rakabNA);
        //start hit detection
        SetNAHitOn();
    }
    public void finishNormalAttack()
    {
        attacking = false;
        //stop hit detection
        setNAHitOff();
        setAction(Actions.Default);
    }

    void SetNAHitOn()
    {
        handfront.GetComponent<rakabhandfront>().DetectOn();
    }
    
    void setNAHitOff()
    {
        handfront.GetComponent<rakabhandfront>().DetectOff();
    }

    void SlamAttack()
    {
        anim.SetFloat("AttackNumber", 2);
        setAnimation("TriggerAttack");

    }
    public void startSlamAttack()
    {
        iniSlam = false;
        attacking = true;

    }
    public void inSlamAttack()  
    {
        setAction(Actions.rakabSlam);
        GetComponent<rakabstats>().rakabSlam();

    }
    public void finishSlamAttack()
    {
        setAction(Actions.Default);
        attacking = false;
    }

    public override void TriggerDeath()
    {
        inAction = true;
        dying = true;
        setAnimation("TriggerDeath");
        //print("yeet");
    }

}
