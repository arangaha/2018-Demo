using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playershoulderfront : bodypart {

    protected override void Awake()
    {
        partfront = "shoulderfront";
        partback = "shoulderfront";
        base.Awake();
    }
}
