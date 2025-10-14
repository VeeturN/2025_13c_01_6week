using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private Transform _patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _HP=2;
    private BasicPlayerMovment _player;
    private Rigidbody2D _rb;
    private bool _movingToB = true;
    
    private float _hitTimer = 0f;
    private Animator _animator;
    
    [SerializeField] private float _attackInterval = 2f; //predkosc ataku
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootPoint;
    
    private void Awake()
    {
        _player = FindObjectOfType<BasicPlayerMovment>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;  // Blokada rotacji przeciwnika
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Debug.Log(_shootPoint.position);
        if (_HP != 0)
        {
            _animator.SetFloat("isJumping", _rb.velocity.y);
            if (_player && IsPlayerInDetectionRange())
            {
                MoveTowardsPlayer();

                if (_player.transform.position.y > transform.position.y+1f)
                {
                    if (Random.value < 0.025f)
                    {
                        Jump();
                    }
                }
            }
            else
            {
                Patrol();
            }

            if (_player && IsPlayerInDetectionRangeShoot())
            {
                _hitTimer += Time.fixedDeltaTime;
                if (_hitTimer > _attackInterval)
                {
                    _animator.SetTrigger("shoot");
                    Shoot();
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
        else
        {
            _animator.SetBool("isDead", true);
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
    private bool IsPlayerInDetectionRangeShoot()
    {
        float playerX = _player.transform.position.x;
        float playerY = _player.transform.position.y;
        float enemyY = transform.position.y;

        bool inXRange = playerX >= _patrolPointA.position.x-5f &&
                        playerX <= patrolPointB.position.x+5f;

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _movingToB=!_movingToB;
        }
    }

    public void hit()
    {
        _HP--;
        Debug.Log("Koniec");
        _animator.SetBool("isHit", true);
    }

    public void endHiting()
    {
        _animator.SetBool("isHit", false);
    }
    
    private void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        Vector2 direction = (_player.transform.position - _shootPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * 8f; //  prędkość pocisku
    }

}