using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerforearmback : bodypart {
    protected override void Awake()
    {
        partfront = "forearmfront";
        partback = "forearmback";
        base.Awake();
    }

}
