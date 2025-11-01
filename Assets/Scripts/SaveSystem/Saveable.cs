using UnityEngine;
using System;

[DisallowMultipleComponent]
public class Saveable : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private string uniqueID;

    public bool _isOnScene;
    
    public string ID => uniqueID;

    private void Start()
    {
        _isOnScene = true;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Jeśli ID nie istnieje, generujemy nowe (tylko w edytorze)
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }

        // Sprawdzenie duplikatów tylko w edytorze (bez EditorSceneManager)
        var all = UnityEngine.Object.FindObjectsOfType<Saveable>();
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
    public void RemotlyDestroy()
    {
        Destroy(gameObject);
    }
}