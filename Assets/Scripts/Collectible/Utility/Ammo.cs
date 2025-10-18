using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, ICollectible
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = other.gameObject.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                GameEventSystem.CollectAmmo(1);
                Destroy(gameObject);
            }
        }
    }
}
