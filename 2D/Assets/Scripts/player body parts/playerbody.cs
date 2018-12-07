using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbody :bodypart{
    protected override void Awake()
    {
        partfront = "bodyfront";
        partback = "bodyback";
        base.Awake();
    }

}
