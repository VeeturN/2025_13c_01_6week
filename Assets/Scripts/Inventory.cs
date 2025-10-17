using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static int _score=0;
    private static int _strenghtPotionCounter;
    private static int _hpPotionCounter;
    private static int _speedPotionCounter;
    private static int _keysCounter;
    private static int _secretMapCounter;
    private static int _ammoCounter=10;
    private static int _hpCounter;
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
    private void CollectValuable(AbstractValuable x)
    {
        _score += x.GetValue();
        GameEventSystem.UpdateHUD(_score, HUDType.Score);
    }
    private void ChangeAmmoValue(int x)
    {
        _ammoCounter =(_ammoCounter + x);
        GameEventSystem.UpdateHUD(_ammoCounter, HUDType.Ammo);
    }
    //dodac collect key na eventach i w klasie key 
    //to samo dla usekey i klasy chest
    //to samo dla collect potion i klasy potion
    public static int GetKeysCollected()
    {
        return _keysCounter;
    }
    public static int GetAmmo()
    {
        return _ammoCounter;
    }
    public static int GetHp()
    {
        return _hpCounter;
    }
    public static void SetHp(int value)
    {
        _hpCounter = value;
        GameEventSystem.UpdateHUD(_hpCounter, HUDType.Hp);
    }
}
