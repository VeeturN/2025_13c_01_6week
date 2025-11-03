using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Enemy;
using SaveSystem;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public static class SaveManager
{
    public static Vector2 _playerPosiotion=new Vector2(0,0);
    public static int _currentLevelIndex=1;
    public static int _fpsCap=144;
    public static void SaveLevelDataXML(int levelNumber) {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
        string saveFileName = $"slot_{GetCurrentSlot()}_level_{levelNumber}.xml";
        FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Create);
        SaveLevelDataDTO data = new SaveLevelDataDTO();
        
        data._saveables = new List<SaveableDTO>();
        data._saveableEnemies = new List<SaveableEnemyDTO>();
        data._levelId = 1;
        
        //collectibles
        var savables = Object.FindObjectsOfType<Saveable>();
        foreach (var saveable in savables)
        {
            var coinData = new SaveableDTO
            {
                id = saveable.ID, 
                position = new Vector2(saveable.transform.position.x, saveable.transform.position.y),
                isOnScene = saveable._isOnScene
            };
            data._saveables.Add(coinData);
        }
        
        //enemies
        var savableEnemy = Object.FindObjectsOfType<SaveableEnemy>();
        foreach (var saveable in savableEnemy)
        {
            var coinData = new SaveableEnemyDTO
            {
                position = new Vector2(saveable.transform.position.x, saveable.transform.position.y),
                isOnScene = saveable._isOnScene,
                enemyPrefabName = saveable._EnemyPrefabName
            };
            data._saveableEnemies.Add(coinData);
        }
        
        
        serializer.Serialize(stream, data);
        stream.Close();
    }
    public static void LoadLevelDataXML(int levelNumber) {
        string saveFileName = $"slot_{GetCurrentSlot()}_level_{levelNumber}.xml";
        string fullPath = Application.dataPath + $"/../{saveFileName}";
        
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"Nie znaleziono zapisu poziomu ({fullPath}). Ładowanie pominięte – uruchamiam poziom w stanie domyślnym.");
            return;
        }
        
        XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
        FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Open);
        SaveLevelDataDTO data = serializer.Deserialize(stream) as SaveLevelDataDTO;
        stream.Close();
        
        if (data == null)
        {
            Debug.LogError($"Błąd podczas wczytywania danych poziomu: {saveFileName}");
            return;
        }
        
        var saveables = Object.FindObjectsOfType<Saveable>();
        foreach (var saveable in saveables)
        {   
            var loadedObj = data._saveables.Find(c => c.id == saveable.ID);
            if (loadedObj != null)
            {
                if (!loadedObj.isOnScene)
                {
                    saveable.RemotlyDestroy();
                }
                else
                {
                    saveable._isOnScene = true;
                    saveable.transform.position = new Vector3(loadedObj.position.X,  loadedObj.position.Y);
                }
            }
            else
            {
                saveable.RemotlyDestroy();
            }
        }
        
        var enemies = Object.FindObjectsOfType<EnemyBase>();
        foreach (var enemy in enemies)
        {
            Object.Destroy(enemy.gameObject);
        }

        foreach (var VARIABLE in data._saveableEnemies)
        {
            GameObject prefab = Resources.Load<GameObject>("NieTykac/"+VARIABLE.enemyPrefabName);
            Object.Instantiate(
                prefab,
                new Vector3(VARIABLE.position.X, VARIABLE.position.Y),
                Quaternion.identity
            );
            
        }
    }
    public static void SaveGameStateDataXML(Vector2 playerPosition)
    {
        string saveFileName = $"slot_{GetCurrentSlot()}_gameState.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateDTO));
        FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Create);
        GameStateDTO data = new GameStateDTO();
        data._shopItems = new List<ShopItemDTO>();
        
        //shopstate
        var allShopItems = new List<ShopItem>
        {
            Shop.Key,
            Shop.Ammo,
            Shop.StrengthPotion,
            Shop.HpPotion,
            Shop.SpeedPotion,
            Shop.SecretMapFragment
        };

        foreach (var item in allShopItems)
        {
            data._shopItems.Add(new ShopItemDTO
            {
                _price = item.getPrice(),
                _name = item.getName(),
                _amount = item.getItemAmountInShop()
            });
        }
        //player info
        data._score = Inventory.Score;
        data._strenghtPotionCounter = Inventory.StrengthPotionCounter;
        data._hpPotionCounter = Inventory.HpPotionCounter;
        data._speedPotionCounter = Inventory.SpeedPotionCounter;
        data._keysCounter = Inventory.KeysCounter;
        data._ammoCounter = Inventory.AmmoCounter;
        data._hpCounter = Inventory.HpCounter;
        data._secretMapsCounter = Inventory.SecretMapsCounter;
        //globals
       // data._unlockedLevels = SaveManager._unlockedLevels;
        data._currentLevelIndex = SaveManager._currentLevelIndex;
        data._playerPosiotion = playerPosition;
        
        //zamknij stream, zapisz
        serializer.Serialize(stream, data);
        stream.Close();
    }
    public static void LoadGameStateDataXML()
    {
        string saveFileName = $"slot_{GetCurrentSlot()}_gameState.xml";
        string fullPath = Application.dataPath + $"/../{saveFileName}";
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"Nie znaleziono zapisu danych gracza ({fullPath}). Ładowanie pominięte – dane gracza w stanie domyślnym.");
            return;
        }
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateDTO));
        FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Open);
        GameStateDTO data = serializer.Deserialize(stream) as GameStateDTO;
        stream.Close();
        if (data == null)
        {
            Debug.LogError("Błąd: Nie udało się zdeserializować danych!");
            return;
        }
        Inventory.Score = data._score;
        GameEventSystem.UpdateHUD(Inventory.Score, HUDType.Score);

        Inventory.StrengthPotionCounter = data._strenghtPotionCounter;
        GameEventSystem.UpdateHUD(Inventory.StrengthPotionCounter, HUDType.StrengthPotion);

        Inventory.HpPotionCounter = data._hpPotionCounter;
        GameEventSystem.UpdateHUD(Inventory.HpPotionCounter, HUDType.HpPotion);

        Inventory.SpeedPotionCounter = data._speedPotionCounter;
        GameEventSystem.UpdateHUD(Inventory.SpeedPotionCounter, HUDType.SpeedPotion);

        Inventory.KeysCounter = data._keysCounter;
        GameEventSystem.UpdateHUD(Inventory.KeysCounter, HUDType.Key);

        Inventory.AmmoCounter = data._ammoCounter;
        GameEventSystem.UpdateHUD(Inventory.AmmoCounter, HUDType.Ammo);

        Inventory.HpCounter = data._hpCounter;
        GameEventSystem.UpdateHUD(Inventory.HpCounter, HUDType.Hp);

        Inventory.SecretMapsCounter = data._secretMapsCounter;
        GameEventSystem.UpdateHUD(Inventory.SecretMapsCounter, HUDType.SecretMap);
        
      //  SaveManager._unlockedLevels = data._unlockedLevels;
        SaveManager._currentLevelIndex = data._currentLevelIndex;
        SaveManager._playerPosiotion = data._playerPosiotion;
        if (data._shopItems != null)
        {
            foreach (var item in data._shopItems)
            {
                switch (item._name)
                {
                    case ShopItemName.Key:
                        Shop.Key.setItemAmountInShop(item._amount);
                        break;
                    case ShopItemName.Ammo:
                        Shop.Ammo.setItemAmountInShop(item._amount);
                        break;
                    case ShopItemName.StrengthPotion:
                        Shop.StrengthPotion.setItemAmountInShop(item._amount);
                        break;
                    case ShopItemName.HpPotion:
                        Shop.HpPotion.setItemAmountInShop(item._amount);
                        break;
                    case ShopItemName.SpeedPotion:
                        Shop.SpeedPotion.setItemAmountInShop(item._amount);
                        break;
                    case ShopItemName.SecretMap:
                        Shop.SecretMapFragment.setItemAmountInShop(item._amount);
                        break;
                    default:
                        Debug.LogWarning($"Nieznany przedmiot w sklepie: {item._name}");
                        break;
                }
            }
        }
    }
    public static void DeleteSaveSlot(int slotNumber)
    {
        string folderPath = Application.dataPath + "/../"; 
        string searchPattern = $"slot_{slotNumber}_*"; 
        string[] files = Directory.GetFiles(folderPath, searchPattern);
        foreach (var file in files)
        {
            try
            {
                File.Delete(file);
                Debug.Log($"Usunięto plik zapisu: {file}");
            }
            catch (IOException e)
            {
                Debug.LogError($"Nie udało się usunąć pliku {file}: {e.Message}");
            }
        }
        if (files.Length == 0)
        {
            Debug.LogWarning($"Brak plików do usunięcia dla slotu {slotNumber}");
        } 
    }
    public static void SaveCurrentSlot(int x) { PlayerPrefs.SetInt("Slot", x); }
    public static int GetCurrentSlot() { return PlayerPrefs.GetInt("Slot", 1); }

    public static void SaveCurrentUnlockedLevels(int currentSlot)
    {
        string key  = "unlocked_levels_on_slot_" + currentSlot;
    }
    public static int GetCurrentUnlockedLevels(int currentSlot)
    {
        string key  = "unlocked_levels_on_slot_" + currentSlot;
        return PlayerPrefs.GetInt(key, 1);
    }


}
