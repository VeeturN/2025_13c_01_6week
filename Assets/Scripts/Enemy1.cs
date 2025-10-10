using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private BasicPlayerMovment player;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _firstAttackDelay;
    private bool _isAttacking;
    private bool _isInFight;
    private float _attackDelayTimer;
    private float _firstAttackTimer;
    private bool _attacked;
    private void Awake()
    {
        player = FindObjectOfType<BasicPlayerMovment>();
        _animator = GetComponent<Animator>();
        _attackDelayTimer = 0;
        _firstAttackTimer = 0;
    }

    private void FixedUpdate()
    {
        if (_isInFight)
            _firstAttackTimer += Time.deltaTime;
        if(!_isAttacking)
            _attackDelayTimer += Time.deltaTime;
        if (_attacked?_attackDelayTimer > _attackDelay: _firstAttackTimer > _firstAttackDelay && _isInFight)
        {
            _firstAttackTimer = 0;
            _attackDelayTimer = 0;
            _animator.SetBool("isAttacking", true);
            _isAttacking = true;
            _attacked = true;
        }
    }
    public void StartAttack()
    {
        _isInFight = true;
        _attacked = false;
    }

    public void DoneAttacking()
    {
        _isAttacking = false;
        _animator.SetBool("isAttacking", false);
    }
    
    public void StopAttack()
    {
        _isInFight = false;
    }
}
