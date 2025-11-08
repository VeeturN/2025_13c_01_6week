using UnityEngine;
using System;

[DisallowMultipleComponent]
public class Saveable : MonoBehaviour
{
    [SerializeField]
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
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }
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