// csharp
using UnityEngine;

public class MovingEnemyMelee : MovingEnemyBase
{

    protected override void MoveTowardsPlayer()
    {
        float deltaX = _player.transform.position.x - transform.position.x;
        if (Mathf.Abs(deltaX) > 0.2f)
        {
            float direction = Mathf.Sign(deltaX);
            _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    protected override void HandleAttackTimer()
    {
        if (IsPlayerInAttackRange)
        {
            _hitTimer += Time.fixedDeltaTime;
            if (_hitTimer > _attackInterval)
            {
                _animator.SetBool("isAttacking", true);
                _hitTimer = 0f;
            }
        }
        else
        {
            _hitTimer = 0f;
        }
    }

    public void tryToHitPlayer()
    {
        if (IsPlayerInAttackRange && _player != null)
        {
            _player.hit();
        }
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
}