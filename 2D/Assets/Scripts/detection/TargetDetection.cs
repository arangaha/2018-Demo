using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour {
    [SerializeField] List<GameObject> Allies = new List<GameObject>();
    [SerializeField] List<GameObject> Enemies = new List<GameObject>();
   

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Enemies.Count<=0||Allies.Count<=0)
        {
            updateNearby();
        }
	}
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(transform.parent.CompareTag("Player")&& collision.gameObject.CompareTag("Enemy")|| transform.parent.CompareTag("Enemy") && collision.gameObject.CompareTag("Player"))
        {
            if(!Enemies.Contains(collision.gameObject))
            {
                Enemies.Add(collision.gameObject);
            }
        }

        else if(transform.parent.CompareTag("Player") && collision.gameObject.CompareTag("Player") || transform.parent.CompareTag("Enemy") && collision.gameObject.CompareTag("Enemy"))
        {
            if (!Allies.Contains(collision.gameObject)&&collision.gameObject.Equals(transform.parent))
            {
                Allies.Add(collision.gameObject);
            }
        }
        updateNearby();

    }
    protected void OnTriggerStay2D(Collider2D collision)
    {

        if (!Enemies.Contains(collision.gameObject) && !(Allies.Contains(collision.gameObject)))
            {
           
            if (transform.parent.CompareTag("Player") && collision.gameObject.CompareTag("Enemy") || transform.parent.CompareTag("Enemy") && collision.gameObject.CompareTag("Player"))
            {
               
                if (!Enemies.Contains(collision.gameObject))
                {
                    Enemies.Add(collision.gameObject);
                }
            }

            else if (transform.parent.CompareTag("Player") && collision.gameObject.CompareTag("Player") || transform.parent.CompareTag("Enemy") && collision.gameObject.CompareTag("Enemy"))
            {
                if (!Allies.Contains(collision.gameObject) && collision.gameObject.Equals(transform.parent))
                {
                    Allies.Add(collision.gameObject);
                }
            }
            updateNearby();
        }

    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (Enemies.Contains(collision.gameObject))
        {
            Enemies.Remove(collision.gameObject);
        }
        else if (Allies.Contains(collision.gameObject))
        {
            Allies.Remove(collision.gameObject);
        }
        updateNearby();
    }
    protected void  updateNearby()
    {
        transform.parent.GetComponent<unitControl>().updateNearbyAllies(Allies);
        transform.parent.GetComponent<unitControl>().updateNearbyEnemies(Enemies);
    }


}
