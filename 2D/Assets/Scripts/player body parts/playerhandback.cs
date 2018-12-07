using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerhandback : bodypart {
    protected override void Awake()
    {
        partfront = "handfront";
        partback = "handback";
        base.Awake();
    }
}
