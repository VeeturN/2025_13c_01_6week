using Enemy;
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
    void FixedUpdate()
    {
        transform.position += new Vector3((_right?_speed:-_speed)*Time.fixedDeltaTime, 0);
        if (Mathf.Abs(transform.position.x-_startPositionX) > 50)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hittable"))
        {
            IHitable obj = collision.GetComponentInParent<IHitable>();
            if (obj != null) obj.hit(1,transform.position.x);
        }


        if(!collision.CompareTag("Player")&& !collision.CompareTag("Attack")&& !collision.CompareTag("Pickable"))
            Destroy(gameObject);        
    }
}
