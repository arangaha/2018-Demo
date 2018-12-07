using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shock_sparks : MonoBehaviour {
    int count=0;
    System.Random rand = new System.Random();
    Sprite first;
    Sprite second;
    bool loadedSprite = false;
    int spriteCounter = 0;
    // Use this for initialization
    void Start () {
        spriteCounter =Mathf.RoundToInt( Mathf.Abs(GetComponent<Transform>().position.x%2));
        if (spriteCounter == 1)
        {
            GetComponent<SpriteRenderer>().sprite = first;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = second;
        }
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
    }
	
	// Update is called once per frame
	void Update () {
        if(!loadedSprite)
        {
            first = Resources.Load<Sprite>("FX Overlays/shock_sparks1");
            second = Resources.Load<Sprite>("FX Overlays/shock_sparks2");
            loadedSprite = true;
        }
        GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
        count++;
        if(count==5)
        {
            count = 0;
            rand = new System.Random();
            int rota = rand.Next(0, 360)+ Mathf.RoundToInt( GetComponent<Transform>().position.x*20000000);
            GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, rota);
            if(spriteCounter ==1)
            {
                GetComponent<SpriteRenderer>().sprite = second;
                spriteCounter = 2;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = first;
                spriteCounter = 1;
            }

        }

    }
}
