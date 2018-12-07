using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakabbicep : bodypart{
    protected override void Awake()
    {
        partfront = "rakabbicep";
        partback = "rakabbicep";
        base.Awake();
    }
}
