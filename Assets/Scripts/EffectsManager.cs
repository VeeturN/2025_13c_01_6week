using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private EffectScript _runEffect;
    [SerializeField] private EffectScript _jumpEffect;
    [SerializeField] private EffectScript _fallEffect;
    [SerializeField] private EffectScript[] _playerAttackEffect = new EffectScript[3];
    [SerializeField] private EffectScript[] _airPlayerAttackEffect = new EffectScript[2];
    [SerializeField] private EffectScript _fierceToothAttackEffect;
    [SerializeField] private EffectScript _pinkStarAttackEffect;
    [SerializeField] private EffectScript _potionEffect;
    [SerializeField] private EffectScript[] _waterSplash = new EffectScript[2];

    public void RunEffect(Vector3 pos, Vector3 scale)
    {
        Instantiate(_runEffect, pos, Quaternion.identity).transform.localScale = scale;
    }
    public void JumpEffect(Vector3 pos)
    {
        Instantiate(_jumpEffect, pos, Quaternion.identity);
    }
    public void FallEffect(Vector3 pos)
    {
        Instantiate(_fallEffect, pos, Quaternion.identity);
    }
    public void PlayerAttackEffect(int attackNum, Vector3 pos, Vector3 scale)
    {
        Instantiate(_playerAttackEffect[attackNum-1], pos, Quaternion.identity).transform.localScale = scale;
    }
    public void PlayerAirAttackEffect(int attackNum, Vector3 pos, Vector3 scale)
    {
        Instantiate(_airPlayerAttackEffect[attackNum-1], pos, Quaternion.identity).transform.localScale = scale;
    }
    public void FierceToothAttackEffect(Vector3 pos, Vector3 scale)
    {
        Instantiate(_fierceToothAttackEffect , pos, Quaternion.identity).transform.localScale = scale;
    }
    public void PinkStarAttackEffect(Vector3 pos, Vector3 scale)
    {
        Instantiate(_pinkStarAttackEffect, pos, Quaternion.identity).transform.localScale = scale;
    }
    public void PotionEffect(Vector3 pos, Transform parent)
    {
        Instantiate(_potionEffect, pos, Quaternion.identity).transform.SetParent(parent);
    }
    
    public EffectScript[] WaterSplashEffect(Vector3 frontPos, Vector3 backPos)
    {
        return new EffectScript[] { Instantiate(_waterSplash[0],frontPos,Quaternion.identity), Instantiate(_waterSplash[1],backPos,Quaternion.identity) };
    }
}
