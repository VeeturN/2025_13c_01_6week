using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoldCoin : AbstractValuable
{
    protected override void SetObjValue()
    {
        _value = 1;
    }
}
