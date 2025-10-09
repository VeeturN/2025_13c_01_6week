using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    public Transform player; 
    public float interactionDistance = 3f; 
    private BasicPlayerMovment _playerObj;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerObj=player.GetComponent<BasicPlayerMovment>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (_playerObj.keysCollected>0)
            {
                _playerObj.keysCollected--;
                Destroy(gameObject);
            }
        }
    }
}
