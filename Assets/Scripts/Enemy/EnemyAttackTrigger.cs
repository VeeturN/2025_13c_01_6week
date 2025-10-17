using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MovingEnemyBase movingEnemy = GetComponentInParent<MovingEnemyBase>();
            if (movingEnemy != null)
                movingEnemy.IsPlayerInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MovingEnemyBase movingEnemy = GetComponentInParent<MovingEnemyBase>();
            if (movingEnemy != null)
                movingEnemy.IsPlayerInAttackRange = false;
        }
    }
}