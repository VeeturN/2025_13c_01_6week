using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingTrap : MonoBehaviour
{
    private float impactSpeed = 10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = collision.collider.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(Vector2.up * impactSpeed, ForceMode2D.Impulse);
            }
        }
    }
}
