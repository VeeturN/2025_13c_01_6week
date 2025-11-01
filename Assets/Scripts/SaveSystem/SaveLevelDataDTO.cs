using System;
using System.Collections.Generic;

[Serializable]
public class SaveLevelDataDTO
{
    public int _levelId;
    public List <SaveableDTO> _saveables;
    public List<SaveableEnemyDTO>  _saveableEnemies;
    
}