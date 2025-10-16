using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEventSystem
{
    public static event Action<int> OnValuableCollected;
    public static event Action<int> OnAmmoAmountChanged;
    public static event Action<int, HUDType> OnHUDParameterChanged;
    
    public static void CollectValuable(int scorePoints)
    {
        OnValuableCollected?.Invoke(scorePoints);
    }
    public static void CollectAmmo(int amount)
    {
        OnAmmoAmountChanged?.Invoke(amount);
    }

    public static void DecreseAmmo(int amount)
    {
        OnAmmoAmountChanged?.Invoke(-amount);
    }

    public static void UpdateHUD(int hudValue, HUDType hudType)
    {
        OnHUDParameterChanged?.Invoke(hudValue, hudType);
    }
}
public enum HUDType
{
    Score,
    Hp,
    Ammo,
    StrengthPotion,
    HpPotion,
    SpeedPotion,
    Key,
    SecretMap
}