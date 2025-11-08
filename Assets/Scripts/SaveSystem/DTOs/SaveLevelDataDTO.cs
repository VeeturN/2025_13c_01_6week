using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveLevelDataDTO
{
    public int _levelId;
    public List <SaveableDTO> _saveables;
    public List<SaveableEnemyDTO>  _saveableEnemies;
    public Vector3 _position;
    public List<MapFragmentEnum> _mapFragmentEnum;
    public List<TotemDTO> _totems;
    
}