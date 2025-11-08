
using UnityEngine;

public class SaveableEnemy :  MonoBehaviour
{
    public bool _isOnScene;
    public string _EnemyPrefabName;
    public bool _isTotem;
    public string _configName;
    protected void Start()
    {
        _isOnScene = true;
        _isTotem = false;
    }
}
