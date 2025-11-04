using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class SavePoint : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("ZAPIS GRY SIĘ WYKONAŁ");
        SaveManager.SaveGameStateDataXML();
        SaveManager.SaveLevelDataXML(SaveManager._currentLevelIndex, other.transform.position);
    }
}
