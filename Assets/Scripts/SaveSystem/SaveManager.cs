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
        //coiny
        data._coins = new List<CoinDTO>();
        data._levelId = 1;
        var coins = Object.FindObjectsOfType<GoldCoin>();
        
        foreach (var coin in coins)
        {
            var coinData = new CoinDTO
            {
                id = coin.ID, 
                position = new Vector2(coin.transform.position.x, coin.transform.position.y),
                collected = coin._isCollected
            };
            data._coins.Add(coinData);
        }
        serializer.Serialize(stream, data);
        stream.Close();
    }
    public static void LoadXML() {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveLevelDataDTO));
        FileStream stream = new FileStream(Application.dataPath + "/../save.xml", FileMode.Open);
        SaveLevelDataDTO data = serializer.Deserialize(stream) as SaveLevelDataDTO;
        stream.Close();
        
        var coins = Object.FindObjectsOfType<GoldCoin>();
        foreach (var coin in coins)
        {   
            var savedCoin = data._coins.Find(c => c.id == coin.ID);
            if (savedCoin != null)
            {
                coin._isCollected = false;
                coin.transform.position = new Vector3(savedCoin.position.X,  savedCoin.position.Y);
            }
            else
            {
                coin.RemotlyDestroy();
            }
        }
    }
}
