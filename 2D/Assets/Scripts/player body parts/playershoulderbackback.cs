using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playershoulderbackback : bodypart {
    protected override void Awake()
    {
        partfront = "shoulderbackback";
        partback = "shoulderbackback";
        base.Awake();
    }
}
