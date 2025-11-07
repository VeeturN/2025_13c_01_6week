using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ScoreDTO
{
    public string name;
    public int score;
}

public class LoadGlobalScoreboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;

    public void LoadGlobal()
    {
        StartCoroutine(GetScores(scores =>
        {
            if (scores == null)
            {
                textField.text = "CONNECTION ERROR";
                return;
            }

            if (scores.Count == 0)
            {
                textField.text = "EMPTY";
                return;
            }

            var sorted = scores.OrderByDescending(s => s.score).ToList();
            string result = "";
            foreach (var s in sorted)
                result += $"{s.name} {s.score}\n";

            textField.text = result;
        }));
    }

    public static IEnumerator GetScores(System.Action<List<ScoreDTO>> callback)
    {
        using (var req = UnityWebRequest.Get("http://185.142.163.172:9001/api/score"))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                callback(null);
                yield break;
            }

            string json = req.downloadHandler.text;
            try
            {
                var array = JsonHelper.FromJson<ScoreDTO>(json);
                callback(array == null ? new List<ScoreDTO>() : new List<ScoreDTO>(array));
            }
            catch
            {
                callback(null);
            }
        }
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
