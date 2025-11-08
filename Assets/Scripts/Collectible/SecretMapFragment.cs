using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretMapFragment : Saveable, ICollectible
{
    private bool _isCollected;
    [SerializeField] private MapFragmentEnum _mapFragmentEnum;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isCollected)
        {
            return;
        }
        _isCollected=true;
        GameEventSystem.CollectSecretMapFragment(this);
    }
    public MapFragmentEnum GetMapFragmentEnum()
    {
        return _mapFragmentEnum;
    }
}
