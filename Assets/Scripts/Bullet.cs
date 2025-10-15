using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool _right;
    [SerializeField] int _speed;
    private float _startPositionX;

    public void Init(bool right)
    {
        _right = right;
        _startPositionX=transform.position.x;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3((_right?_speed:-_speed)*Time.fixedDeltaTime, 0);
        if (Mathf.Abs(transform.position.x-_startPositionX) > 50)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
                enemy.hit();
        }


        if(!collision.CompareTag("Player")&& !collision.CompareTag("Attack")&& !collision.CompareTag("Pickable"))
            Destroy(gameObject);        
    }
}
