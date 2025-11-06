
using UnityEngine;

public class SaveableEnemy :  MonoBehaviour
{
    public bool _isOnScene;
    public string _EnemyPrefabName;

    protected void Start()
    {
        _isOnScene = true;
    }
}
