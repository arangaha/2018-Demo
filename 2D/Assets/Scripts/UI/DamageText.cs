using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    System.Random rand = new System.Random();
    int damage = 0;
    float unroundedDamage = 0;
    float opacity = 1;
    bool isDoT = false;
    GameObject owner;
    float xforce=0;
    float yforce=0;
    // Use this for initialization
    void Start () {
		
	}
	public void intializeText (float number, Elements e)
    {

        unroundedDamage = number;
        //if the unit is healed off the amount
        if(number<0)
        {
            GetComponent<UnityEngine.UI.Text>().color = new Color32(70, 250, 10, 255);
        }
        else if (e == Elements.Physical)
        {
            GetComponent<UnityEngine.UI.Text>().color = new Color32(200, 200, 200, 255);
        }
        else if (e == Elements.Fire)
        {
            GetComponent<UnityEngine.UI.Text>().color = new Color32(255, 190, 50, 255);
        }
        else if (e == Elements.Ice)
        {
            GetComponent<UnityEngine.UI.Text>().color = new Color32(150, 255, 255, 255);
        }
        else if (e == Elements.Toxic)
        {
            GetComponent<UnityEngine.UI.Text>().color = new Color32(120, 190, 70, 255);
        }
        else if (e == Elements.Lightning)
        {
            GetComponent<UnityEngine.UI.Text>().color = new Color32(255, 255, 0, 255);
        }
        else if (e == Elements.Shadow)
        {
            GetComponent<UnityEngine.UI.Text>().color = new Color32(45, 45, 45, 255);
        }
        xforce = rand.Next(-3, 3);
        yforce = rand.Next(5, 10);
        GetComponent<Rigidbody2D>().velocity = new Vector2(xforce, yforce);
    }
    public void addAmount(float number)
    {
        unroundedDamage += number;
        opacity = 1;
    }
    //used if a hit deals more than 1 damage type. Largest damage type has priority one, and is placed in front of other damage types
    //subsequent damage types are behind the 1st damage type a little to the right and up(+ x and + y per order)
    public void setPriority(int order)
    {
        transform.position = new Vector2(transform.position.x+(0.3f*order),transform.position.y+ (0.1f * order));
        if(GetComponent<UnityEngine.UI.Text>().fontSize>0)
            GetComponent<UnityEngine.UI.Text>().fontSize -= 2*order;
    }
    //used if damage text is of a dot, making it not move after created and would continue to update over time
    public void setDot(GameObject owner)
    {
        isDoT = true;
        this.owner = owner;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<UnityEngine.UI.Text>().fontSize = 12;
    }
	// Update is called once per frame
	void Update () {
        damage = Mathf.RoundToInt(unroundedDamage);
        if (damage < 0)
        {
            GetComponent<UnityEngine.UI.Text>().text = "+" + Mathf.Abs(damage);
        }
        else if (damage == 0)
        {
            GetComponent<UnityEngine.UI.Text>().text = "";
        }
        else
        {
            GetComponent<UnityEngine.UI.Text>().text = "" + damage;
        }
        if (isDoT)
            transform.position = new Vector3(owner.transform.position.x+xforce/3, owner.transform.position.y+yforce/4-1);
        float r = GetComponent<UnityEngine.UI.Text>().color.r;
        float g = GetComponent<UnityEngine.UI.Text>().color.g;
        float b = GetComponent<UnityEngine.UI.Text>().color.b;
        //if text should start fading
        opacity -= 0.03f;
        GetComponent<UnityEngine.UI.Text>().color = new Color(r, g, b, opacity);
        if (opacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
