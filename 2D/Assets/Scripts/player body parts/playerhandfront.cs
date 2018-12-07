using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerhandfront : bodypart {
    protected override void Awake()
    {
        partfront = "handback";
        partback = "handfront";
        base.Awake();
    }
}
