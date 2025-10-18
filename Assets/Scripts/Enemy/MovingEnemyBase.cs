// csharp
using Enemy;
using UnityEngine;
using UnityEngine.UI;

public abstract class MovingEnemyBase : EnemyBase
{
    [SerializeField] protected Transform _patrolPointA;
    [SerializeField] protected Transform patrolPointB;
    [SerializeField] protected float _speed = 2f;
    
    protected bool _movingToB = true;

    protected virtual void FixedUpdate()
    {
        if (_HP <= 0)
        {
            _animator.SetBool("isDead", true);
            _animator.SetBool("isAttacking", false);
            _animator.SetBool("isRunning", false);
            return;
        }
        if (_isHittedInAir) return;

        _animator.SetFloat("isJumping", _rb.velocity.y);

        if (_player != null && IsPlayerInDetectionRange())
        {
            MoveTowardsPlayer();
            if (ShouldJump())
                Jump();
        }
        else
            Patrol();
        
        HandleAttackTimer();

        UpdateRunAnimation();
        
    }



    protected virtual void Patrol()
    {
        float targetX = _movingToB ? patrolPointB.position.x : _patrolPointA.position.x;
        float direction = Mathf.Sign(targetX - transform.position.x);
        _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);

        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
            _movingToB = !_movingToB;
    }

    protected virtual void Jump()
    {
        if (Mathf.Abs(_rb.velocity.y) < 0.01f)
        {
            float jumpForce = Random.Range(3f, 6f);
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    protected virtual void UpdateRunAnimation()
    {
        if (_animator.GetBool("isAttacking")) return;

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

    // nowa metoda decydująca o skoku - można nadpisać w pochodnych
    protected virtual bool ShouldJump()
    {
        return Random.value < 0.025f;
    }

    protected abstract void MoveTowardsPlayer();
    protected abstract void HandleAttackTimer();
    

    public void endDead()
    {
        Destroy(GetComponent<CircleCollider2D>());
    }

    public void OnBecameInvisible()
    {
        if (GetComponent<CircleCollider2D>()==null )
        {
            Destroy(transform.parent.gameObject);
        }
    }
    // domyślne implementacje IEnemy - mogą być nadpisane
    public virtual void madeAttack()
    {
        _animator.SetBool("isAttacking", false);
    }
    protected virtual bool IsPlayerInDetectionRange()
    {
        if (_player == null) return false;
        float playerX = _player.transform.position.x;
        float playerY = _player.transform.position.y;
        float enemyY = transform.position.y;

        bool inXRange = playerX >= Mathf.Min(_patrolPointA.position.x, patrolPointB.position.x) &&
                        playerX <= Mathf.Max(_patrolPointA.position.x, patrolPointB.position.x);

        bool inYRange = Mathf.Abs(playerY - enemyY) <= 10f;
        return inXRange && inYRange;
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        bool isDashing = _animator.GetBool("isAttacking");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _movingToB = !_movingToB;
        }
        else if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            collision.gameObject.GetComponent<BasicPlayerMovment>().hit();
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.AddForce(new Vector2((collision.transform.position.x - transform.position.x<0 ? -1 : 1)*3,2), ForceMode2D.Impulse);
        }
    }
}
