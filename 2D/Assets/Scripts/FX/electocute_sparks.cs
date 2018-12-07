using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electocute_sparks : MonoBehaviour {
    int count = 0;
    System.Random rand = new System.Random();
    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
        count++;
        if (count == 5)
        {
            count = 0;
            rand = new System.Random();
            int rota = rand.Next(0, 360) + Mathf.RoundToInt(GetComponent<Transform>().position.x * 20000000);
            GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, rota);
        }
        }
    }
