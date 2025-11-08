using System.Collections.Generic;
using System.Numerics;
using SaveSystem;

public class GameStateDTO
{
    public int _unlockedLevels;
    public int _currentLevelIndex;
    //eq
    public int _score;
    public int _strenghtPotionCounter;
    public int _hpPotionCounter;
    public int _speedPotionCounter;
    public int _keysCounter;
    public int _ammoCounter;
    public int _hpCounter;
    //shopstate
    public List<ShopItemDTO> _shopItems;
    
    
}
