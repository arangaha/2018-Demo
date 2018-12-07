using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemplayerbody : equipable {
    protected string attribute;
    protected ModType[] prefixList = new ModType[6]; // rollable stats for all chests
    protected ModType[] prefixListHeavy = new ModType[6];
    protected ModType[] prefixListMedium = new ModType[7];
    protected ModType[] prefixListLight = new ModType[6];
    protected ModType[] suffixList = new ModType[15]; // rollable stats for all chests

    // Use this for initialization
    void Start() {
        for(int i = 0;  i<prefixList.Length; i++)
        {
            prefixList[i] = new ModType();
        }
        prefixList[0].modName = "Health";
        prefixList[0].tiers = new float[2, 6] { {10, 20, 30, 40, 50, 60},{20,35,50,65,80,100}};

        prefixList[1].modName = "Cooldown Reduction";
        prefixList[1].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 2, 3, 4, 5 } };
        for (int i = 0; i < prefixListHeavy.Length; i++)
        {
            prefixListHeavy[i] = new ModType();
        }
        prefixListHeavy[0].modName = "Block Amount";
        prefixListHeavy[0].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 20, 40, 60, 90, 120, 150 } };
        prefixListHeavy[1].modName = "Armor";
        prefixListHeavy[1].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 50, 100, 150, 200 } };
        prefixListHeavy[2].modName = "Vitality";
        prefixListHeavy[2].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 10, 20, 30, 40, 50, 60 } };
        prefixListHeavy[3].modName = "Strength";
        prefixListHeavy[3].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 10, 20, 30, 40, 50, 60 } };
        prefixListHeavy[4].modName = "Health Regen";
        prefixListHeavy[4].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 8, 16, 24, 30, 36, 45 } };

        for (int i = 0; i < prefixListLight.Length; i++)
        {
            prefixListLight[i] = new ModType();
        }

        prefixListLight[0].modName = "Dodge Chance";
        prefixListLight[0].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 2,3,4,5 } };
        prefixListLight[1].modName = "Dodge Speed and Recovery";
        prefixListLight[1].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 3, 5, 8, 10 } };
        prefixListLight[2].modName = "Dexterity";
        prefixListLight[2].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 10, 20, 30, 40, 50, 60 } };
        prefixListLight[3].modName = "Focus";
        prefixListLight[3].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 10, 20, 30, 40, 50, 60 } };
        prefixListLight[4].modName = "Armor";
        prefixListLight[4].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 30, 60, 90, 120 } };

        for (int i = 0; i < prefixListMedium.Length; i++)
        {
            prefixListMedium[i] = new ModType();
        }
        prefixListMedium[0].modName = "Dexterity And Focus";
        prefixListMedium[0].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 5, 10, 15, 20, 25, 30 } };
        prefixListMedium[1].modName = "Strength and Vitality";
        prefixListMedium[1].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 5, 10, 15, 20, 25, 30 } };
        prefixListMedium[2].modName = "Block Amount";
        prefixListMedium[2].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 20, 40, 60, 90, 120, 150 } };
        prefixListMedium[3].modName = "Dodge Chance";
        prefixListMedium[3].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 2, 3, 4, 5 } };
        prefixListMedium[4].modName = "Health Regen";
        prefixListMedium[4].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 8, 16, 24, 30, 36, 45 } };
        prefixListMedium[5].modName = "Dodge Speed and Recovery";
        prefixListMedium[5].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 3, 5, 8, 10 } };
        prefixListMedium[6].modName = "Armor";
        prefixListMedium[6].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 40, 80, 120, 160 } };
        for (int i = 0; i < suffixList.Length; i++)
        {
            suffixList[i] = new ModType();
        }
        suffixList[0].modName = "Max Stamina";
        suffixList[0].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 3, 6, 9, 12, 15, 18 } };
        suffixList[1].modName = "Max Resource"; 
        suffixList[1].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 2, 4, 6, 8, 10, 12 } };
        suffixList[2].modName = "Life Gained on Resource Spent";
        suffixList[2].tiers = new float[2, 6] { { 10, 20, 30, 40, 50, 60 }, { 1, 2, 3, 4, 5, 6 } };
        suffixList[3].modName = "Stamina Regenerated per Second";
        suffixList[3].tiers = new float[2, 5] { { 10, 20, 30, 40, 50}, { 0.2F, 0.4F, 0.6F, 0.8F , 1 }};
        suffixList[4].modName = "Reduced Damage taken from Melee Hits";
        suffixList[4].tiers = new float[2, 3] { {20, 30, 50 }, { 2,3,4 } };
        suffixList[5].modName = "Reduced Damage taken from Projectile Hits";
        suffixList[5].tiers = new float[2, 3] { { 20, 30, 50 }, { 2, 3, 4 } };
        suffixList[6].modName = "Tenacity";
        suffixList[6].tiers = new float[2, 5] { { 10, 20, 30, 40, 50 }, { 2, 4, 6, 8, 10 } };
        suffixList[7].modName = "Fire Resist";
        suffixList[7].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 50, 100, 150, 200 } };
        suffixList[8].modName = "Cold Resist";
        suffixList[8].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 50, 100, 150, 200 } };
        suffixList[9].modName = "Lightning Resist";
        suffixList[9].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 50, 100, 150, 200 } };
        suffixList[10].modName = "Toxic Resist";
        suffixList[10].tiers = new float[2, 4] { { 15, 30, 45, 60 }, { 50, 100, 150, 200 } };




    }
	 
	// Update is called once per frame
	void Update () {
		
	} 

    public itemplayerbody (int rarity, string type, string itemBase, string attribute) : base(rarity, type, itemBase)
    {
        this.attribute = attribute;
        if(name!="")
        {

        }
    }
    protected override void setMods()
    {
        modList = new Mod[suffixes + prefixes];

    }
}
 