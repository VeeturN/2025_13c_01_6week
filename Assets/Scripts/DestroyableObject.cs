using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IHitable
{
    [SerializeField] private int _HP;
    [SerializeField] PhysicsMaterial2D _bouncyMat;
    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    private float _halfHeight;
    public void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _col = GetComponent<BoxCollider2D>();
        _halfHeight = _col.bounds.extents.y;
    }
    public void hit(int damage, float xPos)
    {
        _col.sharedMaterial = _bouncyMat;
        transform.position += Vector3.up * 0.1f;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.AddForce(new Vector2((xPos < transform.position.x ? 1 : -1) * 100, 150));
        _HP -= damage;
    }
    private void FixedUpdate()
    {
        CheckGround();
    }
    private void CheckGround()
    {
        Debug.DrawRay(transform.position + Vector3.down * _halfHeight / 10, Vector2.down * _halfHeight, Color.red);
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            transform.position + Vector3.down * _halfHeight / 10,
            Vector2.down,
            _halfHeight,
            LayerMask.GetMask("Ground")
        );
        foreach (var h in hits)
        {
            if (h.collider != _col)
            {
                _rb.bodyType = RigidbodyType2D.Kinematic;
                _rb.velocity = Vector2.zero;
                _col.sharedMaterial = null;
                break;
            }
        }
    }
}
