using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    private int _price;
    private ShopItemName _name;
    private int _itemAmountInShop;
    private string _description;
    
    public int getPrice() { return _price; }
    public ShopItemName getName() { return _name; }
    public int getItemAmountInShop() { return _itemAmountInShop; }
    public string getDescription() { return _description; }
    public void setItemAmountInShop(int amount) { _itemAmountInShop = amount; }

    public ShopItem( ShopItemName name,int  price, int  itemAmountInShop, string description)
    {
        _name = name;
        _price = price;
        _itemAmountInShop = itemAmountInShop;
        _description = description;
    }
}
