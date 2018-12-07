using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakabhandfront : bodypart {
    public bool canDetect = false;
    public List<GameObject> collidingEnemies;
    public List<GameObject> hitEnemies;

    protected override void Awake()
    {
        partfront = "rakabhandfront";
        partback = "rakabhandfront";
        base.Awake();
    }
    void Update()
    {
        if (canDetect)
        {
            for (int i = 0; i < collidingEnemies.Count; i++)
            {
                if (!hitEnemies.Contains(collidingEnemies[i]))
                {
                    transform.parent.GetComponent<rakabstats>().setCurrentTags(transform.parent.GetComponent<rakabAI>().getAction());
                    transform.parent.GetComponent<rakabstats>().hitTarget(collidingEnemies[i].gameObject);
                    hitEnemies.Add(collidingEnemies[i]);

                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

            if (collision.gameObject.CompareTag("Player")&&!collidingEnemies.Contains(collision.gameObject))
            {
                collidingEnemies.Add(collision.gameObject);

            }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collidingEnemies.Contains(collision.gameObject))
        {
            collidingEnemies.Remove(collision.gameObject);
        }
    }
    public void DetectOn()
    {
        canDetect = true;
    }
    public void DetectOff()
    {
        canDetect = false;
        hitEnemies.Clear();
    }


}
