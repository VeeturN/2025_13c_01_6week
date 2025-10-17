using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractValuable : MonoBehaviour, ICollectible
{
    protected int _value;
    private bool _isCollected;
    private ValuableAnimationScript _animationScript;
    private void Awake()
    {
        _isCollected=false;
        _animationScript = GetComponent<ValuableAnimationScript>();
        SetObjValue();
    }
    protected abstract void SetObjValue();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isCollected)
        {
            return;
        }
        _isCollected=true;
        GameEventSystem.CollectValuable(_value);
        _animationScript.ChangeParaInAmimationIsCollected();
        Destroy(gameObject, 1f);
    }
}
