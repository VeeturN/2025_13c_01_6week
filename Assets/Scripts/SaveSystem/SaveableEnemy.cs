
using UnityEngine;

public class SaveableEnemy :  MonoBehaviour
{
    public bool _isOnScene;
    public string _EnemyPrefabName;
    private void Start()
    {
        _isOnScene = true;
    }
}
