using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerwaist : bodypart {
    protected override void Awake()
    {
        partfront = "waistfront";
        partback = "waistback";
        base.Awake();
    }
}
