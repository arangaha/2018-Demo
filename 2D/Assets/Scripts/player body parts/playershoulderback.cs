using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playershoulderback : bodypart {
    protected override void Awake()
    {
        partfront = "shoulderback";
        partback = "shoulderback";
        base.Awake();
    }
}
