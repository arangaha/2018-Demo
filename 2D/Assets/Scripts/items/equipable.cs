using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mod
{
    public string modName { get; set; }
    public float value { get; set; }
}
public class ModType
{
    public string modName { get; set; }
    public float[,] tiers { get; set; }
    // tiers are displayed as
    //[level requirement][values]
}

public abstract class equipable : MonoBehaviour {
    protected int rarity = 1; // 1 = normal, 2 = uncommon, 3 = rare, 4 = unique, 5 = legendary (legendary contains set bonuses, unique has unique rolls)
    protected int suffixes =0;
    protected int prefixes =0;
    protected string itemName = "";
    protected bool isAccessory;
    System.Random rand = new System.Random();
    protected Mod[] modList;
    protected string type; //type of equipable
    protected abstract void setMods();
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public equipable(int rarity,string type, string itemBase) //itemBase is base it is dropped on if item is not unique or legendary (i.e. borak). if they are, then name is used to implement unique power
    {
        this.rarity = rarity;
        this.type = type;
        if(rarity >3)
            this.itemName = itemBase;
        setModNumber();
        setMods();
    }

    void setModNumber()
    {
        if (!isAccessory)
        {
            if (rarity == 2)
            {
                prefixes = rand.Next(0, 3);
                if (prefixes == 0)
                {
                    suffixes = rand.Next(1, 3);
                }
                else if (prefixes == 2)
                {
                    suffixes = rand.Next(0, 2);
                }
                else
                {
                    suffixes = rand.Next(1, 3);
                }
            }
            else if (rarity == 3)
            {
                prefixes = rand.Next(1, 4);
                if (prefixes == 1)
                {
                    suffixes = 3;
                }
                else if (prefixes == 2)
                {
                    suffixes = rand.Next(2, 4);
                }
                else
                {
                    suffixes = rand.Next(1, 4);
                }
            }
        }
    }

}
