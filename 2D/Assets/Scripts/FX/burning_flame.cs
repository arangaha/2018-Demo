using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burning_flame : MonoBehaviour {
    int counter = 0;

	// Use this for initialization
	void Start () {
		
    }
	
	// Update is called once per frame
	void Update () {
        counter++;
        if(counter == 5)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            counter = 0;
        }
        GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder+1;
    }
}
