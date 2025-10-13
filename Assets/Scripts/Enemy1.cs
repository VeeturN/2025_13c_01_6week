using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private Transform _patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _detectionRange = 0f; // Zasięg wykrywania gracza
    private BasicPlayerMovment _player;
    private Rigidbody2D _rb;
    private bool _movingToB = true;

    public bool IsPlayerInAttackRange { get; set; } = false;
    private float _hitTimer = 0f;
    private Animator _animator;

    private float _attackAnimDuration = 0.5f; // czas trwania animacji ataku
    private float _attackAnimTimer = 0f;
    private float _attackAnimLeadTime = 0.3f; // ile wcześniej ma się zacząć animacja
    [SerializeField] private float _attackInterval = 1f; //predkosc ataku

    private void Awake()
    {
        _player = FindObjectOfType<BasicPlayerMovment>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true; // Blokada rotacji przeciwnika
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_player && IsPlayerInDetectionRange())
        {
            MoveTowardsPlayer();
        }
        else
        {
            Patrol();
        }

        if (IsPlayerInAttackRange)
        {
            _hitTimer += Time.fixedDeltaTime;

            // Animacja startuje wcześniej
            if (_hitTimer >= _attackInterval - _attackAnimLeadTime && _attackAnimTimer <= 0f)
            {

                _animator.SetBool("isAttacking", true);
                _attackAnimTimer = _attackAnimDuration + _attackAnimLeadTime;
            }

            // Napis HIT pojawia się później
            if (_hitTimer >= _attackInterval)
            {
                Debug.Log("HIT");
                _hitTimer = 0f;
            }
        }

        // if (_attackAnimTimer > 0f)
        // {
        //     _attackAnimTimer -= Time.fixedDeltaTime;
        //     if (_attackAnimTimer <= 0f)
        //          _animator.SetBool("isAttacking", false);
        // }
        else if (!IsPlayerInAttackRange)
        {
             _animator.SetBool("isAttacking", false);
        }

        if (!_animator.GetBool("isAttacking"))
        {
            if (_rb.velocity.x != 0)
            {
                _animator.SetBool("isRunning", true);
                if (_rb.velocity.x > 0)
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                else if (_rb.velocity.x < 0)
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                _animator.SetBool("isRunning", false);
            }
        }
    }

    private bool IsPlayerInDetectionRange()
    {
        float playerX = _player.transform.position.x;
        return playerX >= Mathf.Min(_patrolPointA.position.x, patrolPointB.position.x) && //-detectionRange 
               playerX <= Mathf.Max(_patrolPointA.position.x,
                   patrolPointB.position.x); //+detectionRange XD Nawet jak jest 0 to szuka dalej beka
    }

    private void MoveTowardsPlayer()
    {
        float direction = Mathf.Sign(_player.transform.position.x - transform.position.x);
        _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);
    }

    private void Patrol()
    {
        float targetX = _movingToB ? patrolPointB.position.x : _patrolPointA.position.x;
        float direction = Mathf.Sign(targetX - transform.position.x);
        _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);

        // Sprawdzenie czy przeciwnik dotarł do celu (z tolerancją)
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            _movingToB = !_movingToB;
        }
    }

    public void madeAttack()
    {
        _animator.SetBool("isAttacking", false);
    }

    public void tryToHitPlayer()
    {
        
        
    }

}