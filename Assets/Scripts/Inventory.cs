using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int _score=0;
    private int _strenghtPotionCounter;
    private int _hpPotionCounter;
    private int _speedPotionCounter;
    private int _keysCounter;
    private int _secretMapCounter;
    private int _ammoCounter=10;
    private int _hpCounter;
    private void Start()
    {
        GameEventSystem.OnValuableCollected += CollectValuable;
        GameEventSystem.OnAmmoAmountChanged += ChangeAmmoValue;
    }
    
    private void OnDestroy()
    {
        GameEventSystem.OnValuableCollected -= CollectValuable;
        GameEventSystem.OnAmmoAmountChanged -= ChangeAmmoValue;
    }
    private void CollectValuable(int x)
    {
        _score += x;
        GameEventSystem.UpdateHUD(_score, HUDType.Score);
    }
    private void ChangeAmmoValue(int x)
    {
        _ammoCounter =(_ammoCounter + x);
        GameEventSystem.UpdateHUD(_ammoCounter, HUDType.Ammo);
    }
}
