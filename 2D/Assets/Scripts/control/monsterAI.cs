using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class monsterAI : unitControl {
    [SerializeField] protected float AggroRange = 30;
    [SerializeField] protected Actions queuedAction = Actions.Default;
    [SerializeField] protected float queuedActionDistance;
    [SerializeField] protected GameObject TrackedUnit;
    [SerializeField] protected bool needToFaceTrackedUnit = false;
    [SerializeField] protected bool moving = false;
    [SerializeField] protected bool moveRight = true;
    [SerializeField] protected bool tracking = false;
    [SerializeField] protected float actionTimer = 0;
    protected bool initializedTargetDetection = false;
    protected GameObject targetDetection;
    protected GameObject targetDetectionInstance;
	// Use this for initialization

    protected override void Update()
    {
        base.Update();
        if(!initializedTargetDetection)
        {
            initializedTargetDetection = true;
            targetDetection = Resources.Load("TargetDetection") as GameObject;
            targetDetectionInstance = Instantiate(targetDetection,transform);
        
        }
        if (!inAction)
        {
            if (!isFrozen)
            {
                if (Mathf.Abs(velocity) > 0)
                    anim.SetFloat("Speed", 1);
                else
                    anim.SetFloat("Speed", 0);
                if(moving)
                {
                    Walk();
                }
            }
            else
            {
                stopMoving();
            }
            //if completely idle (not in action and ready for next action)
            if (!(grounded && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) != 0))
            {
                
                anim.SetBool("isIdle", true);
                
            }
            if(tracking)
            {
                
                //actionTimer++;
                //if (actionTimer >= 10)
                //{
                //    TrackUnit();
                //    actionTimer = 0;
                //}
                TrackUnit();
            }
            else
            {
                pickAction();
            }


        }
    }
    public override abstract void AddEffectOverlay(Sprite sprite);

    public override abstract void RemoveEffectOverlay(Sprite sprite);

    public override abstract void AddFlames();

    protected abstract void pickAction();

    protected abstract void startTriggeredAction(Actions a);


    protected void MoveToLocation(Transform t)
    {

    }

    //sets direction towards a target
    protected void setDirectionTarget(GameObject g)
    {
        float direction = transform.position.x - g.transform.position.x;
     
        if(direction< 0)
        {
            moveRight = true;

        }
        else
        {
            moveRight = false;
        }
    }
    //g : unit to track
    //actionDistance : minimum distance before queued action is used
    protected void SetTrackUnit(GameObject g, float actionDistance)
    {
        tracking = true;
        TrackedUnit = g;
        queuedActionDistance = actionDistance;
    }
    protected void TrackUnit()
    {
        
        float trackingDistance = Mathf.Abs(getUnitDistance(TrackedUnit));


        if (trackingDistance <= AggroRange)
        {
            setDirectionTarget(TrackedUnit);
            if (!moving)
            {
                
                startMoving();

            }
        }


        if(trackingDistance<= queuedActionDistance&&!queuedAction.Equals(Actions.Default))
        {

            stopMoving();

            if (!inAction)
            {

                startTriggeredAction(queuedAction);

                pickAction();
            }
        }
    }

    protected void QueueAction(Actions a, bool needToFace)
    {
        queuedAction = a;
        needToFaceTrackedUnit = needToFace;
    }


    protected void startMoving()
    {
        
        moving = true;
    }
    protected void stopMoving()
    {
      
        moving = false;
    }
    public void Walk()
    {

        if (!inAction)
        {
            if (moveRight)
            {
                if(!facingRight)
                {
                    FlipDirection();
                }
                GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                if (facingRight)
                {
                    FlipDirection();
                }

                GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
        }
    }
    public override void updateNearbyEnemies(List<GameObject> enemies)
    {
        base.updateNearbyEnemies(enemies);
        //SetTrackUnit(closestUnit(getUnitsByDistance(false, AggroRange)),5);
    }
}
