using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        throw new NotImplementedException();
    }
}
