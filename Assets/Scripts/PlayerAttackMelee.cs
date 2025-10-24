using Enemy;
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
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hittable"))
        {
            IHitable obj = collision.GetComponentInParent<IHitable>();
            player.AddHittableInRange(obj);
        } 
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hittable"))
        {
            IHitable obj = collision.GetComponentInParent<IHitable>();
            player.RemoveHittableInRange(obj);
        }
    }
}
