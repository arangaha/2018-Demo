using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerhead : bodypart {

    protected override void Awake()
    {
        partfront = "headfront";
        partback = "headback";
        base.Awake();
    }
}
