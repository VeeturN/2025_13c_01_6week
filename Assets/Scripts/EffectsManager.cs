using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject _runEffect;
    [SerializeField] private GameObject _jumpEffect;
    [SerializeField] private GameObject _fallEffect;
    [SerializeField] private GameObject[] _playerAttackEffect = new GameObject[3];
    [SerializeField] private GameObject[] _airPlayerAttackEffect = new GameObject[2];
    [SerializeField] private GameObject _fierceToothAttackEffect;
    [SerializeField] private GameObject _pinkStarAttackEffect;
    [SerializeField] private GameObject _potionEffect;

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
    public void PlayerAirAttackEffect(int i)
    {

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
}
