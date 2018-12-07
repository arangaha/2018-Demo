using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerfeetback : bodypart {
    protected override void Awake()
    {
        partfront = "feetfront";
        partback = "feetfront";
        base.Awake();
    }
}
