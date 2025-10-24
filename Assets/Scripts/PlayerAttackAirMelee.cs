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
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hittable"))
        {
            IHitable obj = collision.GetComponentInParent<IHitable>();
            player.AddHittableInAirRange(obj);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hittable"))
        {
            IHitable obj = collision.GetComponentInParent<IHitable>();
            player.RemoveHittableInAirRange(obj);
        }
    }
}
