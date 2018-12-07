using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerforearmfront:bodypart{

   protected override void Awake()
    {
        partfront = "forearmback";
        partback = "forearmfront";
        base.Awake();
    }


  }
