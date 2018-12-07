using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
    public bool isDead = true;
    public GameObject player;
    GameObject playerIni;
    public GameObject playerInstance;
	// Use this for initialization
	void Start () {
		
	}
    void Awake()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player");
        playerIni = Resources.Load("Prefabs/player 2h") as GameObject;
        playerInstance = Instantiate(playerIni);
        isDead = false;
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKey("r"))
        {
            if(isDead)
            {
                playerInstance = Instantiate(playerIni);
               
                isDead = false;

            }
        }
	}

    public void deadPlayer()
    {
        isDead = true;
    }
}
