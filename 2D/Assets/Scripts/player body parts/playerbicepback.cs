using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbicepback : bodypart{
    protected override void Awake()
    {
        partfront = "bicepback";
        partback = "bicepfront";
        base.Awake();
    }
}
