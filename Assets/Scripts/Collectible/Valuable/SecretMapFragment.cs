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
        Destroy(gameObject);
    }
    public MapFragmentEnum GetMapFragmentEnum()
    {
        return _mapFragmentEnum;
    }

    public void setMapFragmentEnum(MapFragmentEnum mapFragmentEnum)
    {
        _mapFragmentEnum = mapFragmentEnum;
    }
}
