using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEventSystem
{
    public static event Action<int> OnAmmoAmountChanged;
    public static event Action<int, HUDType> OnHUDParameterChanged;
    public static event Action<int> OnUseItem;
    public static event Action OnChestOpen;
    public static event Action<AbstractValuable> OnValuableCollected;
    public static event Action<float, PotionEnum> OnPotionTimeChaged;
    public static event Action<SecretMapFragment> OnMapFragmentCollected;
    public static event Action<Color> OnAllMapFragmentCollected;
    
    public static void GivePlayerRewardForAllMapFragmentsCollected(Color color)
    {
        OnAllMapFragmentCollected?.Invoke(color);
    }
    
    public static void CollectSecretMapFragment(SecretMapFragment secretMapFragment)
    {
        OnMapFragmentCollected?.Invoke(secretMapFragment);
    }

    public static void CollectValuable(AbstractValuable valuable)
    {
        OnValuableCollected?.Invoke(valuable);
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
    public static void UpdateHUDPotionTimer(float completionPercentage, PotionEnum potionType)
    {
        OnPotionTimeChaged?.Invoke(completionPercentage, potionType);
    }
    public static void UseItem(int item)
    {
        OnUseItem?.Invoke(item);
    }
    public static void OpenChest()
    {
        OnChestOpen?.Invoke();
    }
}
