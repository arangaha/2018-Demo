using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerthighfront : bodypart {
    protected override void Awake()
    {
        partfront = "thighfront";
        partback = "thighfront";
        base.Awake();
    }
}
