

using System;
using Enemy;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public abstract class MovingEnemyBase : EnemyBase
{
    [SerializeField] protected Transform _patrolPointA;
    [SerializeField] protected Transform _patrolPointB;
    [SerializeField] protected float _speed = 3f;
    private bool _isAlive = true;
    private bool _isTouching;
    private bool _isGrounded;
    protected bool _isStaying=false;
    EffectsManager _effectsManager;
    protected bool _canJump = true;




    protected bool _movingToB = true;
    private float _enemyColiderRadius;

    protected override void Awake()
    {
        base.Awake();

        _effectsManager = GameObject.FindGameObjectWithTag("EffectsManager").GetComponent<EffectsManager>();
        _enemyColiderRadius = GetComponent<CircleCollider2D>().radius;
    }


    protected virtual void FixedUpdate()
    {
        if (_isAlive)
        {
            _isTouching =
                Physics2D.Raycast(
                    transform.position + Vector3.down * _enemyColiderRadius / 1.3f +
                    Vector3.right * (_enemyColiderRadius * 2.3f), Vector2.left, 2f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(
                transform.position + Vector3.down * _enemyColiderRadius / 1.3f +
                Vector3.right * (float)(_enemyColiderRadius * 2.3), Vector3.left * 2f, Color.red);
            CheckGround();
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
                if (_isTouching)
                {
                    Jump();
                }

                if (ShouldJump())
                    Jump();
            }
            else
            {
                Patrol();
                if (_isTouching)
                {
                    Jump();
                }
            }

            HandleAttackTimer();

            UpdateRunAnimation();

        }
        else
        {
            CheckGround();
            if (_isGrounded)
            {
                _animator.SetBool("isDeadGround", true);
                
            }
        }
    }



    protected virtual void Patrol()
    {
        float targetX = _movingToB ? _patrolPointB.position.x : _patrolPointA.position.x;
        float direction = Mathf.Sign(targetX - transform.position.x);
        if(!_isStaying)_rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);
        else _rb.velocity = new Vector2(0, _rb.velocity.y);
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            _movingToB = !_movingToB;
            StartCoroutine(SayPatrol());
        }
    }

    protected virtual IEnumerator SayPatrol()
    {
        _isStaying = true;
        _effectsManager.InterrogationDialogue(transform.position + Vector3.up * _enemyColiderRadius * 1.5f + Vector3.right * _enemyColiderRadius, transform);
        yield return new WaitForSeconds(2f);
        _isStaying = false;
    }

    public void RunEffect()
    {
       _effectsManager.RunEffect(transform.position + Vector3.down * _enemyColiderRadius / 4, new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z));
    }

    protected virtual void Jump()
    {
        if (Mathf.Abs(_rb.velocity.y) < 0.01f && _canJump)
        {
            _effectsManager.JumpEffect(transform.position + Vector3.down * _enemyColiderRadius / 4);
            _rb.AddForce(Vector2.up * Random.Range(3f, 6f), ForceMode2D.Impulse);
            StartCoroutine(WaitForJumpCoroutine());
        }
    }

    protected IEnumerator WaitForJumpCoroutine()
    {
        _canJump = false;
        yield return new WaitForSeconds(1f);
        _canJump = true;
    }

    protected virtual void UpdateRunAnimation()
    {
        if (_animator.GetBool("isAttacking")) return;

        if (_rb.velocity.x != 0)
        {
            _animator.SetBool("isRunning", true);
            if (_rb.velocity.x > 0 && !_isStaying)
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            else if (_rb.velocity.x < 0 && !_isStaying)
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
        Instantiate(_reward,transform.position, Quaternion.identity);
        Destroy(GetComponent<CircleCollider2D>());
    }

    public void OnBecameInvisible()
    {
        if (GetComponent<CircleCollider2D>()==null )
        {
            Destroy(transform.parent.gameObject);
        }
    }
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

        bool inXRange = playerX >= Mathf.Min(_patrolPointA.position.x, _patrolPointB.position.x) &&
                        playerX <= Mathf.Max(_patrolPointA.position.x, _patrolPointB.position.x);

        bool inYRange = Mathf.Abs(playerY - enemyY) <= 10f;
        return inXRange && inYRange;
    }
    
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _movingToB = !_movingToB;
        }
        else if (collision.gameObject.CompareTag("Player") && _animator.GetBool("isAttacking"))
        {
            collision.gameObject.GetComponent<BasicPlayerMovment>().hit();
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.AddForce(new Vector2((collision.transform.position.x - transform.position.x<0 ? -1 : 1)*3,2), ForceMode2D.Impulse);
        }
    }

    private void CheckGround()
    {
        bool toPlayFallEffect = !_isGrounded;
        Debug.DrawRay(transform.position + Vector3.right * _enemyColiderRadius / 2, Vector2.down * _enemyColiderRadius, Color.red);
        Debug.DrawRay(transform.position + Vector3.left * _enemyColiderRadius / 2, Vector2.down * _enemyColiderRadius, Color.red);
        if ((Physics2D.Raycast(transform.position + Vector3.right * _enemyColiderRadius / 2, Vector2.down, _enemyColiderRadius, LayerMask.GetMask("Ground")) ||
            Physics2D.Raycast(transform.position + Vector3.left * _enemyColiderRadius / 2, Vector2.down, _enemyColiderRadius, LayerMask.GetMask("Ground")))
            && _rb.velocity.y <= 0)
        {
            _isGrounded = true;
        }
        if (_isGrounded && toPlayFallEffect)
            _effectsManager.FallEffect(transform.position + Vector3.down * _enemyColiderRadius / 5);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
    }

    private void Die()
    {
        _isAlive= false;
        _isOnScene=false;
        Debug.Log("Enemy died");
    }

    public void SayDead()
    {
        _effectsManager.DeadDialogue(transform.position + Vector3.up * _enemyColiderRadius + Vector3.right * _enemyColiderRadius, transform);
    }
}
