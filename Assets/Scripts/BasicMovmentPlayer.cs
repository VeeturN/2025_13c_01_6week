using Enemy;
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
    [SerializeField] private float _dashDistance = 1;
    [SerializeField] private float _dashSpeed = 1;
    [SerializeField] private int _potionsDuration=15;
    [SerializeField] private int _healValue=1;
    [SerializeField] private int _damage = 1;
    private EffectsManager _effectsManager;
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
    private bool _dash = false;
    private bool _inDash = false;
    private bool _canDash = true;
    private float _endDashPosX;
    private float _lastDashPosX;
    private bool _isGrounded;
    private bool _goDown;
    private List<EnemyBase> _enemiesInRange;
    private bool _isHittedInAir = false;
    private bool _isAlive = true;
    private float _grav;
    private  SpriteRenderer[] _renderers;
    public bool IsSpeedPotionInUse { get; set; } = false;
    public bool IsStrengthPotionInUse { get; set; } = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemiesInRange = new List<EnemyBase>();
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        _playerHalfWidth = col.bounds.extents.x;
        _playerHalfHeight = col.bounds.extents.y;
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _rb.freezeRotation = true;


        GameObject obj = GameObject.FindGameObjectWithTag("EffectsManager");
        if (obj != null)
            _effectsManager = obj.GetComponent<EffectsManager>();
        else
            Debug.LogWarning("Brakuje EffectsManager na scenie");
    }
    public void Start()
    {
        GameEventSystem.OnUseItem += UseItem;
        GameEventSystem.OnAllMapFragmentCollected += SetPlayerCustomSkin;
    }

    private void OnDestroy()
    {
        GameEventSystem.OnUseItem -= UseItem;
        GameEventSystem.OnAllMapFragmentCollected -= SetPlayerCustomSkin;
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
        if (Input.GetButtonDown("Fire3") && !_isGrounded && _canDash)
        {
            _dash = true;
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

                if (_rb.velocity.x != 0 && !_inDash)
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
                    if(_isGrounded)
                    _effectsManager.JumpEffect(transform.position + Vector3.down * _playerHalfHeight / 5);
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
                    int chosenAttack = UnityEngine.Random.Range(1, 4);
                    _animator.SetBool("isAttacking" + chosenAttack, true);
                    _effectsManager.PlayerAttackEffect(chosenAttack, transform.position+Vector3.right*(transform.localScale.x>0?_playerHalfWidth:-_playerHalfWidth)*3, transform.localScale);
                    _attackCountdown = _attackCooldown;
                    _attack = false;
                }

                if (_dash)
                {
                    _endDashPosX = transform.position.x + (transform.localScale.x>=0?_dashDistance:-_dashDistance);
                    _inDash = true;
                    _dash=false;
                    _canDash = false;
                    Debug.Log(_endDashPosX);
                }
                else if(_inDash)
                {
                    if (_rb.gravityScale != 0)
                        _grav = _rb.gravityScale;
                    _rb.gravityScale = 0;
                    if (((transform.localScale.x > 0 && transform.position.x < _endDashPosX)
                        || (transform.localScale.x < 0 && transform.position.x > _endDashPosX))
                        && Math.Abs(transform.position.x - _lastDashPosX) > 0.02f)
                        _rb.velocity = new Vector2(transform.localScale.x > 0 ? _dashSpeed : -_dashSpeed, 0);
                    else
                    {
                        _rb.gravityScale = _grav;
                        _inDash = false;
                    }
                    _lastDashPosX = transform.position.x;
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
        bool toPlayFallEffect = !_isGrounded;
        Debug.DrawRay(transform.position + Vector3.right * _playerHalfWidth/2, Vector2.down * _playerHalfHeight, Color.red);
        Debug.DrawRay(transform.position + Vector3.left * _playerHalfWidth/2, Vector2.down * _playerHalfHeight, Color.red);
        if ((Physics2D.Raycast(transform.position+Vector3.right*_playerHalfWidth/2, Vector2.down, _playerHalfHeight, LayerMask.GetMask("Ground"))||
            Physics2D.Raycast(transform.position + Vector3.left * _playerHalfWidth/2, Vector2.down, _playerHalfHeight, LayerMask.GetMask("Ground")))
            && _rb.velocity.y <= 0)
        {
            _isGrounded = true;
            _canDash = true;
            _jumpCount = 0;
        }
        if(_isGrounded && toPlayFallEffect)
            _effectsManager.FallEffect(transform.position+Vector3.down*_playerHalfHeight/5);
        
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
    public void AddEnemyInRange(EnemyBase enemy)
    {
        _enemiesInRange.Add(enemy);
    }
    //polaczone z animacja
    public void RemoveEnemyInRange(EnemyBase enemy)
    {
        _enemiesInRange.Remove(enemy);
    }
    //animacja nie tykać
    public void HitEnemiesInMeleeRange()
    {
        foreach(EnemyBase enemy in _enemiesInRange)
        {
            if (enemy == null) { continue; }
            enemy.hit(_damage);
        }
    }
    private void SetPlayerCustomSkin(Color color)
    {
        foreach (var r in _renderers)
        {
            r.color = color;
        }
    }
    public void UseItem(int item)
    {
        switch (item)
        {
            case 1:
                StartCoroutine(UseSpeedPotionCoroutine(_potionsDuration));
                break;
            case 2:
                Inventory.SetHp(Inventory.GetHp() + _healValue);
                break;
            case 3:
                StartCoroutine(UseStrengthPotionCorutine(_potionsDuration));
                break;
        }
        if (item <= 3)
            _effectsManager.PotionEffect(transform.position + Vector3.up * _playerHalfHeight, transform);
    }

    public IEnumerator UseSpeedPotionCoroutine(int time)
    {
        IsSpeedPotionInUse = true;
        float t = time;
        _speed *= 1.5f;
        while(t > 0)
        {
            GameEventSystem.UpdateHUDPotionTimer(t/time, PotionEnum.Blue);
            yield return new WaitForSeconds(0.01f);
            t -= 0.01f;
        }
        _speed /= 1.5f;
        IsSpeedPotionInUse = false;
    }
    public IEnumerator UseStrengthPotionCorutine(int time)
    {
        IsStrengthPotionInUse = true;
        float t = time;
        _damage *= 2;
        while (t > 0)
        {
            GameEventSystem.UpdateHUDPotionTimer(t / time, PotionEnum.Green);
            yield return new WaitForSeconds(0.01f);
            t -= 0.01f;
        }
        _damage /=2;
        IsStrengthPotionInUse = false;
    }

    public void RunEffect()
    {
        _effectsManager.RunEffect(transform.position + Vector3.down * _playerHalfHeight / 5, transform.localScale);
    }
}