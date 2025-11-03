using System;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class SavePoint : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("ZAPIS GRY SIĘ WYKONAŁ");
        SaveManager.SaveGameStateDataXML(new Vector2(other.transform.position.x, other.transform.position.y));
        SaveManager.SaveLevelDataXML(SaveManager._currentLevelIndex);
    }
}
