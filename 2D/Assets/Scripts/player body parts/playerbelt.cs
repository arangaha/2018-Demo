using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbelt : bodypart {

    protected override void Awake()
    {
        partfront = "beltfront";
        partback = "beltback";
        base.Awake();
    }
}

