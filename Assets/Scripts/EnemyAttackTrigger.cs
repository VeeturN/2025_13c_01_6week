using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy1 enemy = GetComponentInParent<Enemy1>();
            if (enemy != null)
                enemy.isPlayerInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy1 enemy = GetComponentInParent<Enemy1>();
            if (enemy != null)
                enemy.isPlayerInAttackRange = false;
        }
    }
}