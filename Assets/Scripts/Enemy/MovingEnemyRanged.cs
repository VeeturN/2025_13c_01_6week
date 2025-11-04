// csharp
using UnityEngine;

public class MovingEnemyRanged : MovingEnemyBase
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootPoint;
    private bool _mele = false;

    private CircleCollider2D _col;

    protected override void Awake()
    {
        base.Awake();
        _col = GetComponent<CircleCollider2D>();
    }
    protected void Start()
    {
        _EnemyPrefabName = "Enemy2";
    }
    protected override void MoveTowardsPlayer()
    {
        float deltaX = _player.transform.position.x - transform.position.x;
        float distance = Mathf.Abs(deltaX);

        if (distance > 4f)
        {
            _mele = false;
            float direction = Mathf.Sign(deltaX);
            _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);
        }
        else
        {
            if (!_mele) _rb.velocity = new Vector2(0, _rb.velocity.y);
            _mele = true;
        }
    }

    protected override void HandleAttackTimer()
    {
        if (_player != null && IsPlayerInDetectionRangeShoot())
        {
            _hitTimer += Time.fixedDeltaTime;
            if (_hitTimer > 2f)
            {
                if (_mele && !_animator.GetBool("isShooting"))
                {
                    _animator.SetBool("isAttacking", true);
                    _hitTimer = 0f;
                }
                else if(!_animator.GetBool("isAttacking"))
                {
                    _animator.SetBool("isShooting", true);
                    Shoot();
                    _hitTimer = 0f;
                }
            }
        }
        else
        {
            _hitTimer = 0f;
        }
    }

    private bool IsPlayerInDetectionRangeShoot()
    {
        if (_player == null) return false;
        float playerX = _player.transform.position.x;
        float playerY = _player.transform.position.y;
        float enemyY = transform.position.y;

        bool inXRange = playerX >= _patrolPointA.position.x - 5f &&
                        playerX <= patrolPointB.position.x + 5f;
        bool inYRange = Mathf.Abs(playerY - enemyY) <= 10f;
        return inXRange && inYRange;
    }

    private void Shoot()
    {
        if (_bulletPrefab != null && _shootPoint != null && _player != null)
        {
            Debug.Log("CFEL");
            Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity)
                .GetComponent<EnemyBullet>()
                .Init(_player.transform.position - _shootPoint.position);
        }
    }

    public void endShooting()
    {
        _animator.SetBool("isShooting", false);
    }
    
    protected override bool ShouldJump()
    {
        float dy = _player.transform.position.y - transform.position.y;
        if (dy > 1f && Mathf.Abs(_rb.velocity.y) < 0.01f)
        {
            return base.ShouldJump();
        }
        return false;
    }

    public void CreateAttackEffect()
    {
        float distance = _col.radius*0.8f;
        Vector3 effectPos = transform.position + new Vector3(0f, -distance, 0f);
        GameObject.FindGameObjectWithTag("EffectsManager").GetComponent<EffectsManager>().PinkStarAttackEffect(effectPos, transform.localScale);
    }

    public void StartRolling()
    {
        _rb.velocity = new Vector2(Mathf.Sign(_player.transform.position.x - transform.position.x) * (_speed * 4f), _rb.velocity.y);
    }
}
