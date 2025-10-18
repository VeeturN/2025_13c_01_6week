using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, ICollectible
{
    private PotionEnum _potionType;
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _potionType = (PotionEnum)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(PotionEnum)).Length);
        switch (_potionType)
        {
            case PotionEnum.Red:
                _animator.SetBool("Red", true);
                break;
            case PotionEnum.Green:
                _animator.SetBool("Green",true);
                 break;
            case PotionEnum.Blue:
                _animator.SetBool("Blue", true);
                 break;
        }
    } 

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = other.gameObject.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                Inventory.CollectPotion(_potionType);
                Destroy(gameObject);
            }
        }
    }
}
