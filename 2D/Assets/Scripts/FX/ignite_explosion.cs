    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignite_explosion : default_fx {


	// Use this for initialization
	protected override void Start () {
        base.Start();
        baseScale = 0.5f; //baseAoE
        maxScale = 0.5f;
        currentScale = 0;
        isAoE = true;
        opacity = 0.8f;
        canHit = true;
        decayRate = 0.02f;
        horizontalStretch = true;
        verticalStretch = true;
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + 1);
        if(currentScale<=maxScale)
            currentScale += 0.02f;
        else
        {
            decay = true;
        }
    }
}
