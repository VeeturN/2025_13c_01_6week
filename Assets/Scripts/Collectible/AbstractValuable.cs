using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractValuable : MonoBehaviour, ICollectible
{
    protected int _value;
    private bool _isCollected;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _isCollected=false;
        SetObjValue();
    }
    protected abstract void SetObjValue();
    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            if (_isCollected)
            {
                return;
            }
            BasicPlayerMovment player = other.GetComponent<BasicPlayerMovment>();
            if (player != null && _animator != null)
            {
                _isCollected=true;
                _animator.SetBool("isCollected", true);
                GameEventSystem.CollectValuable(_value);
                Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
    }
}
