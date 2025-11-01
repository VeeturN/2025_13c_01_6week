using UnityEngine;
using System;

[DisallowMultipleComponent]
public class UniqueID : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private string uniqueID;

    public string ID => uniqueID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Jeśli ID nie istnieje, generujemy nowe (tylko w edytorze)
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }

        // Sprawdzenie duplikatów tylko w edytorze (bez EditorSceneManager)
        var all = UnityEngine.Object.FindObjectsOfType<UniqueID>();
        foreach (var other in all)
        {
            if (other == this) continue;
            if (other.uniqueID == uniqueID)
            {
                uniqueID = Guid.NewGuid().ToString();
                break;
            }
        }
    }
#endif

    private void Awake()
    {
        // Jeśli prefab został zinstancjonowany w grze (runtime) i nie ma ID — generujemy
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }
    }
}