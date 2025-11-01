using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractValuable : UniqueID, ICollectible
{
    protected int _value;
    public bool _isCollected;
    private void Awake()
    {
        _isCollected=false;
        SetObjValue();
    }
    protected abstract void SetObjValue();
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isCollected)
        {
            return;
        }
        _isCollected=true;
        GameEventSystem.CollectValuable(this);
        Destroy(gameObject, 1f);
    }
    public int GetValue()
    {
        return _value;
    }

    public void RemotlyDestroy()
    {
        Destroy(gameObject);
    }
}
