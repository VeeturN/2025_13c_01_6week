using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static int _score;
    private static int _strenghtPotionCounter;
    private static int _hpPotionCounter;
    private static int _speedPotionCounter;
    private static int _keysCounter;
    private static int _secretMapCounter;
    private static int _ammoCounter;
    private static int _hpCounter;

    private void Awake()
    {
        _score = 0;
        _strenghtPotionCounter = 0;
        _hpPotionCounter = 0;
        _speedPotionCounter = 0;
        _keysCounter = 0;
        _secretMapCounter = 0;
        _ammoCounter = 10;
        _hpCounter = 10;
    }

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
    public static int GetKeysCollected()
    {
        return _keysCounter;
    }
    public static void SetKeysCollected(int keys)
    {
        _keysCounter = keys;
        GameEventSystem.UpdateHUD(_keysCounter, HUDType.Key);
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

    public static void CollectPotion(PotionEnum potion)
    {
        switch (potion)
        {
            //tu w zaleznosci ktore to ktora pota, na tej zasadzie jak z hppotion
            case PotionEnum.Red:
                GameEventSystem.UpdateHUD(_hpPotionCounter,HUDType.HpPotion);
                break;
            case PotionEnum.Blue:
                break;
            case PotionEnum.Green:
                break;
        }
    }
}
