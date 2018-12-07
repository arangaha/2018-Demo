using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakab : MonoBehaviour {
    public int hp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void takeDamage(int amount, string element, int percent)
    {
        hp = hp - amount;
    }
}
