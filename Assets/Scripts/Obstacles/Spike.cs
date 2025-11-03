using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float maxKnockback = 10f;
    [SerializeField] private float minKnockback = 5f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = collision.collider.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                player.hit();
                float impactSpeed = collision.relativeVelocity.magnitude;
                Vector2 direction = (player.transform.position - transform.position).normalized;

                if (impactSpeed > maxKnockback)
                {
                    impactSpeed =  maxKnockback;
                } else if (impactSpeed < minKnockback)
                {
                    impactSpeed = minKnockback;
                }

                Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(direction * impactSpeed, ForceMode2D.Impulse);
            }
        }
    }
}
