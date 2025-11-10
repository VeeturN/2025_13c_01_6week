using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingTrap : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    [SerializeField] private float impactSpeed = 10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _animator.SetBool("isThrowing", true);
            BasicPlayerMovment player = collision.collider.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(Vector2.up * impactSpeed, ForceMode2D.Impulse);
            }
        }
    }
    public void EndThrowing()
    {
        _animator.SetBool("isThrowing", false);
    }
}
