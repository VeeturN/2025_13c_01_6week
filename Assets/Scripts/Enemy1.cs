using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private BasicPlayerMovment player;
    [SerializeField] private Animator _animator;
    private bool _isAttack = false;
    private bool _inFight = false;
    private float _attackTimer = 0;
    [SerializeField] private float _attackDelay; 
    private void Awake()
    {
        player = FindObjectOfType<BasicPlayerMovment>();
    }

    private void FixedUpdate()
    {
        _attackTimer += Time.fixedDeltaTime;
        
    }
    public void StartAttack()
    {
        Debug.Log("Enemy1: Atak rozpoczęty!");
        _inFight = true;
        _animator.SetBool(IsAttack, true);
    }
    
    public void StopAttack()
    {
        Debug.Log("Enemy1: Atak rozpoczęty!");
        _animator.SetBool(IsAttack,false);
    }
    
    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
