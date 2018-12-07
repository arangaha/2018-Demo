using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAtwoh : MonoBehaviour {
    float opacity = 0.25f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        opacity -= 0.005f;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
        if(opacity <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void initialize(bool facingRight, Transform t)
    {
        if(!facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        transform.rotation = t.rotation;
        
        transform.position = new Vector3(transform.position.x, t.position.y, transform.position.z );

    }
    public void changeScale (Transform t)
    {
        transform.localScale = new Vector3(2, t.localScale.y/2, 0);
    }

}
