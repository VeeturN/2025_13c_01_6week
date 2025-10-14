using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector2 _direction;
    [SerializeField] int _speed;
    
    public void Init(Vector2 direction)
    {
        _direction = direction.normalized;
        GetComponent<Rigidbody2D>().velocity = _direction * _speed; // stała prędkość pocisku
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("trafiony");
            BasicPlayerMovment player = collision.gameObject.GetComponent<BasicPlayerMovment>();
                player.hit();
        }
        
        
        if(!collision.CompareTag("Enemy") && !collision.CompareTag("Pickable")&& !collision.CompareTag("Attack"))
            Destroy(gameObject);
    }
}
