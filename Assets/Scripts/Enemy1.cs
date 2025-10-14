using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private Transform _patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private float _speed = 2f;
    private BasicPlayerMovment _player;
    private Rigidbody2D _rb;
    private bool _movingToB = true;

    public bool IsPlayerInAttackRange { get; set; } = false;
    private float _hitTimer = 0f;
    private Animator _animator;
    
    [SerializeField] private float _attackInterval = 2f; //predkosc ataku

    private void Awake()
    {
        _player = FindObjectOfType<BasicPlayerMovment>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;  // Blokada rotacji przeciwnika
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_player && IsPlayerInDetectionRange())
        {
            MoveTowardsPlayer();
            
            if (_player.transform.position.y > transform.position.y)
            {
                if (Random.value < 0.025f) // 60% szans na skok
                {
                    Jump();
                }
            }
        }
        else
        {
            Patrol();
        }

        if (IsPlayerInAttackRange)
        {
            _hitTimer += Time.fixedDeltaTime;
            if (_hitTimer > _attackInterval)
            {
                _animator.SetBool("isAttacking", true);
                _hitTimer = 0f;
            }
            
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
        float playerY = _player.transform.position.y;
        float enemyY = transform.position.y;

        bool inXRange = playerX >= Mathf.Min(_patrolPointA.position.x, patrolPointB.position.x) &&
                        playerX <= Mathf.Max(_patrolPointA.position.x, patrolPointB.position.x);

        bool inYRange = Mathf.Abs(playerY - enemyY) <= 10f;

        return inXRange && inYRange;
    }

    private void MoveTowardsPlayer()
    {
        float deltaX = _player.transform.position.x - transform.position.x;
        if (Mathf.Abs(deltaX) > 0.2f) // tolerancja, gdy gracz jest "nad" przeciwnikiem
        {
            float direction = Mathf.Sign(deltaX);
            _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y); // zatrzymaj ruch w osi x
        }
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
    
    private void Jump()
    {
        if (Mathf.Abs(_rb.velocity.y) < 0.01f) // tylko gdy stoi na ziemi
        {
            float jumpForce = Random.Range(3f, 6f);
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void madeAttack()
    {
        _animator.SetBool("isAttacking", false);
        Debug.Log("Koniec");
    }

    public void tryToHitPlayer()
    {
        if (IsPlayerInAttackRange)
        {
            _player.hit();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _movingToB=!_movingToB;
        }
    }

}