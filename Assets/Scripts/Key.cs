using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = other.gameObject.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                //player.CollectKey();
                Destroy(gameObject);
            }
        }
    }
}
