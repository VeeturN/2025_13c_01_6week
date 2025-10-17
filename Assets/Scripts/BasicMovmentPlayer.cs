using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class BasicPlayerMovment : MonoBehaviour {
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private int _maxJumps = 2;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootCooldown = 3;
    [SerializeField] private float _attackCooldown = 1;
    [SerializeField] private bool _allowWallJump = false;
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _xinput;
    private float _yinput;
    private float _playerHalfHeight;
    private float _playerHalfWidth;
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
    private bool _isAlive = true;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemiesInRange = new List<IEnemy>();
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        _playerHalfWidth = col.bounds.extents.x;
        _playerHalfHeight = col.bounds.extents.y;
    }
    public void Start()
    {
        _rb.freezeRotation = true;
    }
    private void Update() {
        _xinput=Input.GetAxis("Horizontal");
        _yinput = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump") && _jumpCount<_maxJumps)
        {
            _performJump=true;
        }
        if (Input.GetButtonDown("Range"))
        {
            _shoot=true;
        }
        if (Input.GetButtonDown("Melee"))
        {
            _attack = true;
        }
        if (_yinput<0)
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


                CheckGround();

                if (_rb.velocity.x != 0)
                    transform.localScale = new Vector3(_rb.velocity.x > 0 ? 1 : -1, 1, 1);

                if (_shootCountdown > 0)
                    _shootCountdown -= 0.05f;
                if (_attackCountdown > 0)
                    _attackCountdown -= 0.05f;

                _animator.SetBool("isRunning", _rb.velocity.x!=0 && _isGrounded);
                _animator.SetBool("isFalling", !_isGrounded && _rb.velocity.y < -0.05f);
                _animator.SetBool("isJumping", !_isGrounded && _rb.velocity.y > 0.05f);

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

                if (_shoot && _shootCountdown <= 0 && Inventory.GetAmmo() > 0)
                {
                    _animator.SetBool("isThrowingSword", true);
                    _shootCountdown = _shootCooldown;
                    _shoot = false;
                    GameEventSystem.DecreseAmmo(1);
                    
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
    private void CheckGround()
    {
        Debug.DrawRay(transform.position + Vector3.right * _playerHalfWidth/2, Vector2.down * _playerHalfHeight, Color.red);
        Debug.DrawRay(transform.position + Vector3.left * _playerHalfWidth/2, Vector2.down * _playerHalfHeight, Color.red);
        if ((Physics2D.Raycast(transform.position+Vector3.right*_playerHalfWidth/2, Vector2.down, _playerHalfHeight, LayerMask.GetMask("Ground"))||
            Physics2D.Raycast(transform.position + Vector3.left * _playerHalfWidth/2, Vector2.down, _playerHalfHeight, LayerMask.GetMask("Ground")))
            && _rb.velocity.y <= 0)
        {
            _isGrounded = true;
            _jumpCount = 0;
        }
        
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
    }
    //animacja nie tykać
    public void SpawnSword()
    {
        Instantiate(_bulletPrefab, transform.position, transform.rotation)
            .GetComponent<Bullet>().Init(transform.localScale.x > 0);
        _animator.SetBool("isThrowingSword", false);
    }
    //animacja nie tykać
    public void Attack()
    {
        for(int i=1;i<4;i++)
        _animator.SetBool("isAttacking" + i, false);
    }
    //animacja nie tykać
    public void hit()
    {
        _isHittedInAir=true;
        Inventory.SetHp(Inventory.GetHp()-1);
        if(Inventory.GetHp()<=0)
        _animator.SetBool("isDying", true);
        else
        _animator.SetBool("isHitted", true);
    }
    //animacja nie tykać
    public void gotHitted()
    {
        _isHittedInAir=false;   
        _animator.SetBool("isHitted", false);
    }
    //animacja nie tykać
    public void Die()
    {
        _isAlive = false;  
    }
    //animacja nie tykać
    public bool isAttackingAnimation()
    {
        return _animator.GetBool("isAttacking1") || _animator.GetBool("isAttacking2") || _animator.GetBool("isAttacking3") || _animator.GetBool("isThrowingSword");
    }
    //polaczone z animacja
    public void AddEnemyInRange(IEnemy enemy)
    {
        _enemiesInRange.Add(enemy);
    }
    //polaczone z animacja
    public void RemoveEnemyInRange(IEnemy enemy)
    {
        _enemiesInRange.Remove(enemy);
    }
    //animacja nie tykać
    public void HitEnemiesInMeleeRange()
    {
        foreach(IEnemy enemy in _enemiesInRange)
        {
            if (enemy == null) { continue; }
            enemy.hit();
        }
    }
}