using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakab_slam_wave : default_fx {

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        baseScale = 0.6f;
        maxScale = 0.6f;
        currentScale = 0;
        isAoE = true;
        opacity = 0.6f;
        canHit = true;
        decayRate = 0.02f;
        horizontalStretch = true;
    }


    // Update is called once per frame
    protected override void Update () {
        base.Update();
        if (currentScale <= maxScale)
            currentScale += 0.03f;
        else
        {
            decay = true;
        }

    }
}
