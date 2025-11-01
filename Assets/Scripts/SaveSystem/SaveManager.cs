using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public static class SaveManager
{
    public static void SaveXML() {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
        FileStream stream = new FileStream(Application.dataPath + "/../save.xml", FileMode.Create);
        SaveLevelDataDTO data = new SaveLevelDataDTO();
        
        data._saveables = new List<SaveableDTO>();
        
        data._levelId = 1;
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
    }
}
