using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicPlayerMovment : MonoBehaviour {
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _xinput;
    [SerializeField] private float Speed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private int _maxJumps = 2;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootCooldown = 3;
    [SerializeField] private float _attackCooldown = 1;
    private float _shootCountdown;
    private float _attackCountdown;
    private int jumpCount = 0;
    private bool _performJump;
    private bool _shoot = false;
    private bool _attack = false;
    private bool _isGrounded;

    [SerializeField] private int _HP = 10;
    private int _keysCollected = 0;

    private Enemy1 _enemy1;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy1 = gameObject.GetComponent<Enemy1>();
    }

    public void Start()
    {
        _rb.freezeRotation = true;
    }

    private void Update() {
        _xinput=Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && jumpCount<_maxJumps)
        {
            _performJump=true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _shoot=true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            _attack = true;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_xinput * Speed, _rb.velocity.y);
        if(_rb.velocity.x!=0)
        transform.localScale = new Vector3(_rb.velocity.x>0?1:-1, 1, 1);

        if(_shootCountdown>0)
        _shootCountdown -= 0.05f;
        if (_attackCountdown > 0)
            _attackCountdown -= 0.05f;

        _animator.SetBool("isRunning", _xinput != 0 && _isGrounded);
        _animator.SetBool("isFalling", !_isGrounded && _rb.velocity.y < 0f);
        _animator.SetBool("isJumping", !_isGrounded && _rb.velocity.y > 0f);

        if (isAttackingAnimation())
        {
            _attack = false;
            _shoot = false;
        }
        if (_performJump)
        {
            _performJump = false;
            jumpCount++;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
        if (_shoot && _shootCountdown <= 0)
        { 
            _animator.SetBool("isThrowingSword", true);
            _shootCountdown = _shootCooldown;
            _shoot = false;
        }
        else if (_attack && _attackCountdown <= 0)
        {
            _animator.SetBool("isAttacking" + UnityEngine.Random.Range(1, 4), true);
            _attackCountdown = _attackCooldown;
            _attack = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isGrounded = true;
        jumpCount=0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
        
    }

    public void SpawnSword()
    {
        Instantiate(_bulletPrefab, transform.position, transform.rotation)
            .GetComponent<Bullet>().Init(transform.localScale.x > 0);
        _animator.SetBool("isThrowingSword", false);
    }

    public void Attack()
    {
        for(int i=1;i<4;i++)
        _animator.SetBool("isAttacking" + i, false);
    }


    public void CollectKey()
    {
        _keysCollected++;
    }

    public void UseKey()
    {
        _keysCollected--;
    }

    public int getKeysCollected()
    {
        return _keysCollected;
    }

    public int getHP()
    {
        return _HP;
    }

    public void setHP(int value)
    {
        _HP = value;
    }

    public void hit()
    {
        
    }

    public bool isAttackingAnimation()
    {
        return _animator.GetBool("isAttacking1") || _animator.GetBool("isAttacking2") || _animator.GetBool("isAttacking3") || _animator.GetBool("isThrowingSword");
    }
    
}