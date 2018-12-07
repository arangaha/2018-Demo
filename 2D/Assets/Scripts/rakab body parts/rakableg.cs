using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakableg : bodypart{
    protected override void Awake()
    {
        partfront = "rakableg";
        partback = "rakableg";
        base.Awake();
    }
}
