using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambleZone : MonoBehaviour
{
    private bool _isPlayerInZone = false;

    ShopScript _shop;
    public void Start()
    {
        _shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopScript>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isPlayerInZone = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isPlayerInZone = false;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isPlayerInZone)
        {
            _shop.DownShow();
        }
    }
}
