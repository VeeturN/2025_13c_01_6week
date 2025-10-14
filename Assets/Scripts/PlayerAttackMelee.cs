using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMelee : MonoBehaviour
{
    private BasicPlayerMovment player;
    void Start()
    {
        player = GetComponentInParent<BasicPlayerMovment>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            player.AddEnemyInRange(collision.GetComponent<IEnemy>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            player.RemoveEnemyInRange(collision.GetComponent<IEnemy>());
        }
    }
}
