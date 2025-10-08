using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    public Transform player; // Referencja do gracza
    public float interactionDistance = 3f; // Maksymalna odległość do interakcji

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interakcja ze skrzynką!");
        }
    }
}
