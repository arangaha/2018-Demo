using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbicepfront : bodypart {
    protected override void Awake()
    {
        partfront = "bicepfront";
        partback = "bicepback";
        base.Awake();
    }
}
