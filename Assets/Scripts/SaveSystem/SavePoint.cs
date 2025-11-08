using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class SavePoint : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (SaveManager.GetCurrentLevel(SaveManager.GetCurrentSlot()) != 420213767)
        {
            SaveManager.SaveGameStateDataXML();
            SaveManager.SaveLevelDataXML(SaveManager.GetCurrentLevel(SaveManager.GetCurrentSlot()), other.transform.position);
        }
        else
        {
            Debug.Log("Save na tutorialu wiec tak naprawde sie nie zapisuje ale dziala");
        }
    }
}
