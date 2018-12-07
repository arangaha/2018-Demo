using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakabhead : bodypart{
    protected override void Awake()
    {
        partfront = "rakabhead";
        partback = "rakabhead";
        base.Awake();
    }
}
