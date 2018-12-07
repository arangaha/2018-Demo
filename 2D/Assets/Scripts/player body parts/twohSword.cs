using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twohSword : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Find("tip").GetComponent<twohtip>().update(transform);

	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter" + collision);
        collision.gameObject.GetComponent<rakabstats>().takeDamage(20);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
    }
}
