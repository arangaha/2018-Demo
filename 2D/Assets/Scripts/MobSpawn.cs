using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawn : MonoBehaviour {
    GameObject rakab;
    GameObject rakabInstance;
    public float spawnTimer = 0;
	// Use this for initialization
	void Start () {
        rakab = Resources.Load("Prefabs/rak") as GameObject;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("f"))
        {if (spawnTimer <= 0)
            {
                SpawnRakab();
                spawnTimer = 0.5f;
            }
        }
        spawnTimer -= Time.deltaTime;
    }
    
    public void SpawnRakab()
    {
        
         rakabInstance = Instantiate(rakab);

    }
}
