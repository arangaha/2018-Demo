using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodycollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            transform.parent.GetComponent<testplayercontrol>().isCollidingWithEnemy();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
            transform.parent.GetComponent<testplayercontrol>().notCollidingWithEnemy();
    }
}
