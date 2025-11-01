using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Enemy;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public static class SaveManager
{
    
    public static void SaveXML() {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
        FileStream stream = new FileStream(Application.dataPath + "/../save.xml", FileMode.Create);
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
    public static void LoadXML() {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
        FileStream stream = new FileStream(Application.dataPath + "/../save.xml", FileMode.Open);
        SaveLevelDataDTO data = serializer.Deserialize(stream) as SaveLevelDataDTO;
        stream.Close();
        
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
            GameObject prefab = Resources.Load<GameObject>("Prefabs/MapDesign/"+VARIABLE.enemyPrefabName);
            Object.Instantiate(
                prefab,
                new Vector3(VARIABLE.position.X, VARIABLE.position.Y),
                Quaternion.identity
            );
            
        }
    }
}
