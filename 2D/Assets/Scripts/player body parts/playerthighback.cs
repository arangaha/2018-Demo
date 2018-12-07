using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerthighback : bodypart {
    protected override void Awake()
    {
        partfront = "thighback";
        partback = "thighback";
        base.Awake();
    }
}
