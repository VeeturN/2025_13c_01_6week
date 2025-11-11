using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using Enemy;
using SaveSystem;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Object = UnityEngine.Object;

public static class SaveManager
{
    public static int _fpsCap=144;
    private static bool _isLoadingLevel=true;
    private static bool _isLoadingGameState=true;
    
    public static bool _isSavingLevel=false;
    public static bool _isSavingGameState=false;
    
    
    //files functions
    public static void SaveLevelDataXML(int levelNumber, Vector3 position) {
        if (!_isLoadingLevel)
        {
            _isSavingLevel = true;
            XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
            string saveFileName = $"slot_{GetCurrentSlot()}_level_{levelNumber}.xml";
            FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Create);
            SaveLevelDataDTO data = new SaveLevelDataDTO();
            data._saveables = new List<SaveableDTO>();
            data._saveableEnemies = new List<SaveableEnemyDTO>();
            data._totems = new List<TotemDTO>();
            data._position = position;
            data._levelId = levelNumber;
            //collectibles
            var savables = Object.FindObjectsOfType<Saveable>();
            foreach (var saveable in savables)
            {
                var coinData = new SaveableDTO
                {
                    id = saveable.ID,
                    position = saveable.transform.position,
                    isOnScene = saveable._isOnScene
                };
                data._saveables.Add(coinData);
            }

            //enemies
            var savableEnemy = Object.FindObjectsOfType<SaveableEnemy>();
            foreach (var saveable in savableEnemy)
            {
                if (!saveable._isTotem)
                {
                    try
                    {
                        MovingEnemyBase backToFlying = (MovingEnemyBase)saveable;
                        if (backToFlying != null)
                        {
                            var coinData = new SaveableEnemyDTO
                            {
                                position = saveable.transform.position,
                                isOnScene = saveable._isOnScene,
                                enemyPrefabName = saveable._EnemyPrefabName,
                                A = backToFlying.getA(),
                                B = backToFlying.getB(),
                            };
                            data._saveableEnemies.Add(coinData);
                        }
                        else
                        {
                            var coinData = new SaveableEnemyDTO
                            {
                                position = saveable.transform.position,
                                isOnScene = saveable._isOnScene,
                                enemyPrefabName = saveable._EnemyPrefabName,
                                A=0,
                                B=0,
                            };
                            data._saveableEnemies.Add(coinData);
                        }
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
                else
                {
                    var coinData = new TotemDTO()
                    {
                        position = saveable.transform.position,
                        isOnScene = saveable._isOnScene,
                        enemyPrefabName = saveable._EnemyPrefabName,
                        configName = saveable._configName
                    };
                    data._totems.Add(coinData);
                }

            }

            data._mapFragmentEnum = new List<MapFragmentEnum>();

            foreach (var variable in Inventory.GetSecretMapFragment())
            {
                data._mapFragmentEnum.Add(variable);
            }

            serializer.Serialize(stream, data);
            stream.Close();
            _isSavingLevel=false;
           // Debug.Log("Data saved");
        }
    }
    public static void LoadLevelDataXML(int levelNumber, BasicPlayerMovment player)
    {
        _isLoadingLevel = true;
        string saveFileName = $"slot_{GetCurrentSlot()}_level_{levelNumber}.xml";
        string fullPath = Application.dataPath + $"/../{saveFileName}";
        if (!File.Exists(fullPath))
        {
           // Debug.Log($"Nie znaleziono zapisu poziomu ({fullPath}). Ładowanie pominięte – uruchamiam poziom w stanie domyślnym.");
            _isLoadingLevel = false;
            return;
        }
        XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
        FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Open);
        SaveLevelDataDTO data = serializer.Deserialize(stream) as SaveLevelDataDTO;
        stream.Close();
        if (data == null)
        {
           // Debug.Log($"Błąd podczas wczytywania danych poziomu: {saveFileName}");
            _isLoadingLevel = false;
            return;
        }
      
        player.transform.position = data._position;
        //loadCollectibles 
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
                    saveable.transform.position = loadedObj.position;
                }
            }
            else
            {
                saveable.RemotlyDestroy();
            }
        }
        //Enemies purge
        var enemies = Object.FindObjectsOfType<EnemyBase>();
        foreach (var enemy in enemies)
        {
            Object.Destroy(enemy.gameObject);
        }
        //load normalenemies
        foreach (var variable in data._saveableEnemies)
        {
            if (variable.isOnScene)
            {
                GameObject prefab = Resources.Load<GameObject>("NieTykac/"+variable.enemyPrefabName);
                
                MovingEnemyBase movingEnemyBase = prefab.transform.GetComponentInChildren<MovingEnemyBase>();
                if (movingEnemyBase  != null && !movingEnemyBase.IsUnityNull())
                {
                    movingEnemyBase.SetPatrolPositions(variable.A,variable.B);
                   // Debug.Log("Position Set");
                }
                Object.Instantiate(
                    prefab,
                    variable.position,
                    Quaternion.identity
                );
            }
        }
        //loadtotems
        foreach (var variable in data._totems)
        {
            if (variable.isOnScene)
            {
                GameObject prefab = Resources.Load<GameObject>("NieTykac/"+variable.enemyPrefabName);
                EnemyRangeStayOnePlaceConfig config = Resources.Load<EnemyRangeStayOnePlaceConfig>("NieTykac/Configs/"+variable.configName);
                EnemyRangeStayOnePlaceScript template = prefab.GetComponent<EnemyRangeStayOnePlaceScript>();
                template.setConfig(config);
                Object.Instantiate(
                    prefab,
                    variable.position,
                    Quaternion.identity
                );
            }
        }
        
        foreach (var mapFragmentEnum in data._mapFragmentEnum)
        {
            Inventory.LoadSecretMap(mapFragmentEnum);
        }

        _isLoadingLevel = false;
       // Debug.Log("Data loaded");
    }
    public static void SaveGameStateDataXML()
    {
        if (!_isLoadingGameState)
        {
            _isSavingGameState=true;
            string saveFileName = $"slot_{GetCurrentSlot()}_gameState.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(GameStateDTO));
            FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Create);
            GameStateDTO data = new GameStateDTO();
            //player info
            data._score = Inventory.Score;
            data._strenghtPotionCounter = Inventory.StrengthPotionCounter;
            data._hpPotionCounter = Inventory.HpPotionCounter;
            data._speedPotionCounter = Inventory.SpeedPotionCounter;
            data._keysCounter = Inventory.KeysCounter;
            data._ammoCounter = Inventory.AmmoCounter;
            data._hpCounter = Inventory.HpCounter;
            serializer.Serialize(stream, data);
            stream.Close();
            _isSavingGameState=false;
        }
        
    }
    public static void LoadGameStateDataXML()
    {
        _isLoadingGameState = true;
        string saveFileName = $"slot_{GetCurrentSlot()}_gameState.xml";
        string fullPath = Application.dataPath + $"/../{saveFileName}";
        //jak nie ma save na tym slocie
        if (!File.Exists(fullPath))
        {
           // Debug.Log($"Nie znaleziono zapisu danych gracza ({fullPath}). Ładowanie pominięte – dane gracza w stanie domyślnym.");
            _isLoadingGameState = false;
            return;
        }
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateDTO));
        FileStream stream = new FileStream(Application.dataPath + $"/../{saveFileName}", FileMode.Open);
        GameStateDTO data = serializer.Deserialize(stream) as GameStateDTO;
        stream.Close();
        if (data == null)
        {
          //  Debug.LogError("Błąd: Nie udało się zdeserializować danych!");
            _isLoadingGameState = false;
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
        _isLoadingGameState = false;
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
               // Debug.Log($"Usunięto plik zapisu: {file}");
            }
            catch (IOException e)
            {
               // Debug.LogError($"Nie udało się usunąć pliku {file}: {e.Message}");
            }
        }
        if (files.Length == 0)
        {
          //  Debug.LogWarning($"Brak plików do usunięcia dla slotu {slotNumber}");
        } 
    }
    //prefs functions
    public static void SaveCurrentSlot(int x) { PlayerPrefs.SetInt("Slot", x); }
    public static int GetCurrentSlot() { return PlayerPrefs.GetInt("Slot", 1); }
    public static void SaveCurrentUnlockedLevels(int currentSlot, int  unlockedLevels)
    {
        string key  = "unlocked_levels_on_slot_" + currentSlot;
        PlayerPrefs.SetInt(key, unlockedLevels);
    }
    public static int GetCurrentUnlockedLevels(int currentSlot)
    {
        string key  = "unlocked_levels_on_slot_" + currentSlot;
        return PlayerPrefs.GetInt(key, 1);
    }
    public static void SaveCurrentLevelIndex(int currentSlot, int currentLevelIndex)
    {
        string key  = "current_level_index_" + currentSlot;
        PlayerPrefs.SetInt(key, currentLevelIndex);
    }
    public static int GetCurrentLevel(int currentSlot)
    {
        string key  = "current_level_index_" + currentSlot;
        return PlayerPrefs.GetInt(key, 1);
    }
}
