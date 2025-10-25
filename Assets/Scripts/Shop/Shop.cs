using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shop
{

    public static ShopItem Key = new ShopItem(ShopItemName.Key, 100, 10, "");
    public static ShopItem Ammo = new ShopItem(ShopItemName.Ammo, 100, 10, "");
    public static ShopItem StrengthPotion = new ShopItem(ShopItemName.StrengthPotion, 100, 10, "");
    public static ShopItem HpPotion = new ShopItem(ShopItemName.HpPotion, 100, 10, "");
    public static ShopItem SpeedPotion = new ShopItem(ShopItemName.SpeedPotion, 100, 10, "");
    public static ShopItem SecretMapFragment = new ShopItem(ShopItemName.SecretMap, 100, 10, "");
    //note dla Dawida 
    //jak chcesz w którymś przycisku dodawać kupowanie to przez " public static void BuyItem(ShopItem itemToBuy)" 
    //i bierzesz co chcesz ze statycznego pola 
    //sa obiekty typu ShopItem z opisem, cena i ile ich zostalo w sklepie no i maja getery do tego wiec to tez po polach
    //robisz
    public static void BuyItem(ShopItem itemToBuy)
    {
        if (itemToBuy.getItemAmountInShop() > 0)
        {
            Debug.Log("Oskubałeś już kraba z tych przedmiotów");
            return;
        }
        
        if (Inventory.GetScore() >= itemToBuy.getPrice())
        {
            Inventory.SetScore(Inventory.GetScore()-itemToBuy.getPrice());
            itemToBuy.setItemAmountInShop(itemToBuy.getItemAmountInShop()-1);
        }
        else
        {
            Debug.Log("Nie masz wystarczająco złota!");
        }

        switch (itemToBuy.getName())
        {
            case ShopItemName.HpPotion:
                Inventory.CollectPotion(PotionEnum.Red);
                break;
            case ShopItemName.SpeedPotion:
                Inventory.CollectPotion(PotionEnum.Blue);
                break;
            case ShopItemName.StrengthPotion:
                Inventory.CollectPotion(PotionEnum.Green);
                break;
            case ShopItemName.Ammo:
                Inventory.CollectAmmo(1);
                break;
            case ShopItemName.Key:
                Inventory.SetKeysCollected(Inventory.GetKeysCollected()+1);
                break;
            case ShopItemName.SecretMap:
                Inventory.CollectSecretMapFragment();
                break;
        }
    }
}
