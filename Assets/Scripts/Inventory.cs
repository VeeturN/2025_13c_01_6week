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
    private static int _ammoCounter;
    private static int _hpCounter;
    private static int _secretMapsCounter;


    private void Awake()
    {
        _score = 0;
        _strenghtPotionCounter = 0;
        _hpPotionCounter = 0;
        _speedPotionCounter = 0;
        _keysCounter = 0;
        _ammoCounter = 10;
        _hpCounter = 10;
        _secretMapsCounter = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            UseItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            UseItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseItem(4);
    }

    private void Start()
    {
        GameEventSystem.OnValuableCollected += CollectValuable;
        GameEventSystem.OnAmmoAmountChanged += ChangeAmmoValue;
        GameEventSystem.OnMapFragmentCollected += CollectSecretMapFragment;
    }
    
    private void OnDestroy()
    {
        GameEventSystem.OnValuableCollected -= CollectValuable;
        GameEventSystem.OnAmmoAmountChanged -= ChangeAmmoValue;
        GameEventSystem.OnMapFragmentCollected -= CollectSecretMapFragment;
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
    private void CollectSecretMapFragment(SecretMapFragment secretMapFragment)
    {
        _secretMapsCounter++;
        if (_secretMapsCounter==4)
        {
            GameEventSystem.GivePlayerRewardForAllMapFragmentsCollected(new Color(0f, 1f, 1f, 0.5f));
        }
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
            case PotionEnum.Red:
                _hpPotionCounter++;
                GameEventSystem.UpdateHUD(_hpPotionCounter,HUDType.HpPotion);
                break;
            case PotionEnum.Blue:
                _speedPotionCounter++;
                GameEventSystem.UpdateHUD(_speedPotionCounter,HUDType.SpeedPotion);
                break;
            case PotionEnum.Green:
                _strenghtPotionCounter++;
                GameEventSystem.UpdateHUD(_strenghtPotionCounter,HUDType.StrengthPotion);
                break;
        }
    }
    private void UseItem(int item)
    {
        bool hasEnough = false;
        switch (item)
        {
            case 1:
                if (_speedPotionCounter > 0)
                {
                    hasEnough = _speedPotionCounter > 0;
                    _speedPotionCounter--;
                    GameEventSystem.UpdateHUD(_speedPotionCounter, HUDType.SpeedPotion);
                }
                break;
            case 2:
                if (_hpPotionCounter > 0)
                {
                    hasEnough = _hpPotionCounter > 0;
                    _hpPotionCounter--;
                    GameEventSystem.UpdateHUD(_hpPotionCounter,HUDType.HpPotion);
                }
                break;
            case 3:
                if (_strenghtPotionCounter > 0)
                {
                    hasEnough = _strenghtPotionCounter > 0;
                    _strenghtPotionCounter--;
                    GameEventSystem.UpdateHUD(_strenghtPotionCounter, HUDType.StrengthPotion);
                }
                break;
            case 4:
                GameEventSystem.OpenChest();
                GameEventSystem.UpdateHUD(_keysCounter, HUDType.Key);
                break;
        }
        if (hasEnough && item<=3)
        {
            GameEventSystem.UseItem(item);
        }
        else
        {
            GameEventSystem.OpenChest();
        }
    }
}
