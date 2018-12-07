using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakabstats : monsterstats {
    [SerializeField] protected GameObject RakabSlam;
    [SerializeField] protected GameObject RakabSlamInstance;
    // Use this for initialization
    void Start () {
		
	}
    protected override void Awake()
    {
        base.Awake();
        RakabSlam = Resources.Load("FX/rakab_slam_wave") as GameObject;
        baseMovementSpeed = 5;
    }

    public void rakabSlam()
    {
        RakabSlamInstance = Instantiate(RakabSlam, transform.position, transform.rotation);
        RakabSlamInstance.GetComponent<rakab_slam_wave>().initialize(gameObject, true, false, true, 0, currentActionIncreasedAoE);
        setCurrentTags(Actions.rakabSlam);
        RakabSlamInstance.GetComponent<rakab_slam_wave>().initializeHit(hitDamage(), currentActionAccuracy, currentActionCritChance, currentActionCritDamage, currentActionIgniteChance, currentActionChillChance, currentActionToxinChance, currentActionShockChance, currentActionDreadChance, currentActionLifeSteal);

    }
}

