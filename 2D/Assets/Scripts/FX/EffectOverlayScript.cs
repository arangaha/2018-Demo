using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOverlayScript : MonoBehaviour {

    System.Random rand = new System.Random();

    // Use this for initialization
    void Start () {

       // float angle = rand.Next(0, 360);
        //transform.localEulerAngles = new Vector3(0, 0, angle);
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder+1;
    }
    public void changeSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;

        if(sprite.name.Equals("ignite_overlay"))
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
        }
        else if(sprite.name.Equals("shock_overlay"))
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
            addShockSparks();
        }
        else if (sprite.name.Equals("chill_overlay"))
        {
            GetComponentInParent<bodypart>().chill();
        }
        else if(sprite.name.Equals("electrocute_overlay"))
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
            addElectrocuteSparks();
        }
        else { GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f); }

    }
    public void addFlames(int x, int y)
    {
        float xcount = x;
        float ycount = y;
        float tempx=0;
        float tempy = 0;
        float tempSize = x + y;
        float partSize = x + y;
        int repeatCount = Mathf.RoundToInt(partSize / 1000);
        GameObject flames = Resources.Load("FX/burning_flame") as GameObject;
        GameObject flamesInstance;

        rand = new System.Random();
        while(partSize>1500)
        {
            int rota = Mathf.RoundToInt(GetComponent<Transform>().localPosition.x);
            float rotax = rand.Next(-rota, rota);
            tempx = (rand.Next(-x/2,x/2));
            rota = Mathf.RoundToInt(GetComponent<Transform>().localPosition.y);
            float rotay = rand.Next(-rota, rota);
            tempy = (rand.Next(-y/2,y/2));
            tempx /= 500 + rota/10;
            tempy /= 600+rota/10;
            flamesInstance = Instantiate(flames, transform.position, transform.rotation);
            flamesInstance.transform.parent = gameObject.transform;
            flamesInstance.gameObject.layer = gameObject.layer;
            flamesInstance.transform.position = new Vector3(tempx+ flamesInstance.transform.position.x, tempy+ flamesInstance.transform.position.y, 0);
            partSize -= 1500;
            repeatCount -= 1;
        }

    }
    public void addShockSparks()
    {
        GameObject sparks = Resources.Load("FX/shock_sparks") as GameObject;
        GameObject sparksInstance = Instantiate(sparks, transform.position, transform.rotation);
        sparksInstance.transform.parent = gameObject.transform;
        sparksInstance.gameObject.layer = gameObject.layer;
        sparksInstance.GetComponent<Transform>().position= GetComponent<Transform>().position;
    }
    public void addElectrocuteSparks()
    {
        GameObject sparks = Resources.Load("FX/electrocute_sparks") as GameObject;
        GameObject sparksInstance = Instantiate(sparks, transform.position, transform.rotation);
        sparksInstance.transform.parent = gameObject.transform;
        sparksInstance.gameObject.layer = gameObject.layer;
        sparksInstance.GetComponent<Transform>().position = GetComponent<Transform>().position;
    }
}
