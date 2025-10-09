using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int _shootCooldown = 3;
    private int jumpCount = 0;
    private bool _performJump;
    private bool _shoot = false;
    private bool _isGrounded;
    private float _shootCountdown;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
        if (Input.GetMouseButtonDown(0))
        {
            _shoot=true;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_xinput * Speed, _rb.velocity.y);
        if(_rb.velocity.x!=0)
        transform.localScale = new Vector3(_rb.velocity.x>0?1:-1, 1, 1);

        if(_shootCountdown>0)
        _shootCountdown -= 0.05f;

        if (_xinput != 0)
        _animator.SetBool("isRunning", true);
        else
        _animator.SetBool("isRunning", false);

        if (_performJump)
        {
            _performJump = false;
            jumpCount++;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
        if (_shoot && _shootCountdown<=0)
        {
            _shootCountdown = _shootCooldown;
            _shoot = false;
            Instantiate(_bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>().Init(transform.localScale.x>0);
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
}