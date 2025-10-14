using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicPlayerMovment : MonoBehaviour {
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _xinput;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private int _maxJumps = 2;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootCooldown = 3;
    [SerializeField] private float _attackCooldown = 1;
    [SerializeField] private bool _allowWallJump = false;
    [SerializeField] private int _ammo = 10;
    
    private HUD _playerHUD;
    private float _attackCountdown;
    private float _shootCountdown;
    private int _jumpCount = 0;
    private bool _performJump;
    private bool _shoot = false;
    private bool _attack = false;
    private bool _isGrounded;
    private bool _goDown;
    private List<IEnemy> _enemiesInRange;
    private bool _isHittedInAir = false;

    [SerializeField] private int _HP = 3;
    [SerializeField] private int _Score = 0;
    private int _keysCollected = 0;

    private bool _isAlive = true;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemiesInRange = new List<IEnemy>();
    }

    public void Start()
    {
        _rb.freezeRotation = true;
        //nie trzeba przeciagac w edytorze i sam sobie znajduje huda.
        //jak na razie nie ma huda na scenie to wywala null pointer
        _playerHUD = FindAnyObjectByType<HUD>();
        _playerHUD.updateHealth(_HP);
        _playerHUD.updateScore(_Score);
        _playerHUD.updateAmo(_ammo);
    }

    private void Update() {
        _xinput=Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && _jumpCount<_maxJumps)
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            _goDown=true;
        }
    }

    private void FixedUpdate()
    {
        if (_isAlive)
        {
            if (!_isHittedInAir)
            {
                _rb.velocity = new Vector2(_xinput * _speed, _rb.velocity.y);




                if (_rb.velocity.x != 0)
                    transform.localScale = new Vector3(_rb.velocity.x > 0 ? 1 : -1, 1, 1);

                if (_shootCountdown > 0)
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
                    _jumpCount++;
                    _rb.velocity = new Vector2(_rb.velocity.x, 0);
                    _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
                }

                if (_shoot && _shootCountdown <= 0 && _ammo > 0)
                {
                    _animator.SetBool("isThrowingSword", true);
                    _shootCountdown = _shootCooldown;
                    _shoot = false;
                    _ammo--;
                    _playerHUD.updateAmo(_ammo);
                }
                else if (_attack && _attackCountdown <= 0)
                {
                    _animator.SetBool("isAttacking" + UnityEngine.Random.Range(1, 4), true);
                    _attackCountdown = _attackCooldown;
                    _attack = false;
                }

                if (_goDown)
                {
                    // tutaj szukam dokoło gracza czy jest coś przez co mogę spaść
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                    foreach (var col in colliders)
                    {
                        //tutaj sprawdzam czy to ten konkretny komponent i wykonuje konkretną rzecz
                        if (col.CompareTag("JumpThroughPlatform"))
                        {
                            col.GetComponent<JumpThroughPlatform>()?.AllowPlayerToFallThrough(gameObject);
                        }
                    }

                    _goDown = false;
                }
            }
        }
        else {
            _rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isGround = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Sprawdza, czy kolizja jest od dołu (czyli podłoże)
            if (Vector2.Angle(contact.normal, Vector2.up) < 45f)
            {
                isGround = true;
                break;
            }

            // Jeśli dotykamy ściany (kąt ~90°) i mamy włączony wall jump
            if (_allowWallJump && Vector2.Angle(contact.normal, Vector2.up) > 80f && Vector2.Angle(contact.normal, Vector2.up) < 100f)
            {
                isGround = true;
                break;
            }
        }

        if (isGround)
        {
            _isGrounded = true;
            _jumpCount = 0;
        }
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

    public void hit()
    {
        _isHittedInAir=true;
        
        _HP--;
        if(_HP<=0)
        _animator.SetBool("isDying", true);
        else
        _animator.SetBool("isHitted", true);

        _playerHUD.updateHealth(_HP);
    }
    public void gotHitted()
    {
        _isHittedInAir=false;   
        _animator.SetBool("isHitted", false);
    }
    public void Die()
    {
        _isAlive = false;  
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

    public void CollectAmmo()
    {
        _ammo++;
        _playerHUD.updateAmo(_ammo);
    }

    public int getHP()
    {
        return _HP;
    }

    public void setHP(int value)
    {
        _HP = value;
    }

    public void CollectGoldCoin()
    {
        _Score += 1;
        _playerHUD.updateScore(_Score);
    }

    public void CollectDiamond()
    {
        _Score += 50;
        _playerHUD.updateScore(_Score);
    }

    public bool isAttackingAnimation()
    {
        return _animator.GetBool("isAttacking1") || _animator.GetBool("isAttacking2") || _animator.GetBool("isAttacking3") || _animator.GetBool("isThrowingSword");
    }

    public void AddEnemyInRange(IEnemy enemy)
    {
        _enemiesInRange.Add(enemy);
    }

    public void RemoveEnemyInRange(IEnemy enemy)
    {
        _enemiesInRange.Remove(enemy);
    }

    public void HitEnemiesInMeleeRange()
    {
        foreach(IEnemy enemy in _enemiesInRange)
        {
            enemy.hit();
        }
    }


}