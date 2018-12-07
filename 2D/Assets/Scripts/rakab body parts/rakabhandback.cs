using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakabhandback : bodypart{

    protected override void Awake()
    {
        partfront = "rakabhandback";
        partback = "rakabhandback";
        base.Awake();
    }
}
