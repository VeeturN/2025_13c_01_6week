using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Saveable,  ICollectible
{
    private Animator _animator;
    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = other.gameObject.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                Inventory.SetKeysCollected(Inventory.GetKeysCollected()+1);
                _animator.SetBool("isCollected", true);
            }
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
