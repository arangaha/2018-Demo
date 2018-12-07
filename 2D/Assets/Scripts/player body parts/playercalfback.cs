using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercalfback : bodypart {
    protected override void Awake()
    {
        partfront = "calffront";
        partback = "calfback";
        base.Awake();
    }

}
