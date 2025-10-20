using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAirMelee : MonoBehaviour
{
    private BasicPlayerMovment player;
    void Start()
    {
        player = GetComponentInParent<BasicPlayerMovment>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponentInParent<EnemyBase>();
            player.AddEnemyInAirRange(enemy);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponentInParent<EnemyBase>();
            player.RemoveEnemyInAirRange(enemy);
        }
    }
}
