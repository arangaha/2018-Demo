using UnityEngine;
using System.Collections;

public class FollowOwner : MonoBehaviour
{
	private float offset;			// The offset at which the Health Bar follows the player.
	
    private bool exists = false;


	void Awake ()
	{
		
		
	}

	void Update ()
	{
       //transform.position = new Vector3(0, offset, 0);
    }

    public void initialize (float offset)
    {

        this.offset = offset;
        //transform.position = new Vector3(0, offset, 0);
        exists = true;
    }
    public void setOffset(float offset)
    {
        this.offset = offset;
    }
}
