using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszedł w attack trigger!");
            var enemy = GetComponentInParent<Enemy1>();
            enemy.StartAttack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszedł w attack trigger!");
            var enemy = GetComponentInParent<Enemy1>();
            enemy.StopAttack();
        }
    }
}