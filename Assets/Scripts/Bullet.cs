using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool _right;
    [SerializeField] int _speed;

    public void Init(bool right)
    {
        _right = right;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3((_right?_speed:-_speed)*Time.fixedDeltaTime, 0);
        if (Mathf.Abs(transform.position.x) > 50)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("trafiony");
            Enemy1 enemy = collision.gameObject.GetComponent<Enemy1>();
                enemy.hit();
        }
        if (collision.gameObject.CompareTag("Enemy2"))
        {
            Debug.Log("trafiony");
            Enemy2 enemy = collision.gameObject.GetComponent<Enemy2>();
            enemy.hit();
        }
        
        
        if(!collision.CompareTag("Player")&& !collision.CompareTag("Attack")&& !collision.CompareTag("Pickable"))
        Destroy(gameObject);
    }
}
