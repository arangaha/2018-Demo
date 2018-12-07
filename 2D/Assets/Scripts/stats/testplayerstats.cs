using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testplayerstats : unitstats {
    //stats
    private testplayercontrol player;
    // Use this for initialization
    void Start () {
		
	}

    protected override void Awake()
    {
        base.Awake();
        unitLevel = 10;
        player = GetComponent<testplayercontrol>();
        maxHealth = (baseHealth + 10 * unitLevel) * (1 + increasedHealth);
        currentHealth = maxHealth;
        increasedAttackSpeed = 0.2f;
        weaponPhysicalDamage = 30;
    }
    protected override void updateStats()
    {
        base.updateStats();
        //test numbers
        //physicalAddedAsFire = 0.2f;
        //addedIgniteChance = 70f;
        //physicalAddedAsIce = 0.2f;
        //addedChillChance = 70f;
        addedIgniteChance = 70f;
        addedShockChance = 70f;
        addedChillChance = 70f;
    }
    public void fireInfusion()
    {
        physicalAddedAsFire = 0.05f;

    }
    public void IceInfusion()
    {

        physicalAddedAsIce = 0.1f;

    }
    public void LightningInfusion()
    {

        physicalAddedAsLightning = 0.1f;
    }
    public void clearInfusions()
    {
        physicalAddedAsFire = 0;
        physicalAddedAsIce = 0;
        physicalAddedAsLightning = 0;
    }

}
