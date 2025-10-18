using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour,  ICollectible
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = other.gameObject.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                Inventory.SetKeysCollected(Inventory.GetKeysCollected()+1);
                Destroy(gameObject);
            }
        }
    }
}
