using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterstats : unitstats {

    private monsterAI monster;
    void Start () {
		
	}

    protected override void Awake()
    {
        base.Awake();
        unitLevel = 10;
        monster = GetComponent<monsterAI>();
        maxHealth = (baseHealth + 10 * unitLevel) * (1 + increasedHealth);
        currentHealth = maxHealth;
        increasedAttackSpeed = 0;
    }
}
