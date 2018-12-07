using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2hweap : bodypart {
    public bool canDetect = false;
	// Use this for initialization
	void Start () {
		
	}
    protected override void Awake()
    {
        partfront = "2h";
        partback = "2hwarped";
        base.Awake();
    }
    void Update()
    {
        transform.Find("tip").GetComponent<twohtip>().update(transform);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDetect)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                
                transform.parent.GetComponent<testplayerstats>().setCurrentTags(transform.parent.GetComponent<testplayercontrol>().getAction());
                transform.parent.GetComponent<testplayerstats>().hitTarget(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       // Debug.Log("exit");
    }
    public void DetectOn()
    {
        canDetect = true;
    }
    public void DetectOff()
    {
        canDetect = false;
    }
}
