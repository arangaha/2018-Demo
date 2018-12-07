using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twohtip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void update(Transform t)
    {
        transform.rotation = t.rotation;
        transform.position = transform.parent.transform.position;
        
            //Quaternion.Euler(new Vector3(t.rotation.x, t.rotation.y, t.rotation.z));
    }
}
