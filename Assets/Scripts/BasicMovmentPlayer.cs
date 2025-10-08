    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicPlayerMovment : MonoBehaviour {
    private Rigidbody2D _rb;
    private float _xinput;
    [SerializeField] private float Speed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private int _maxJumps = 2;
    private int jumpCount = 0;
    private bool _performJump;
    private bool _isGrounded;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        _rb.freezeRotation = true;
    }

    private void Update() {
        _xinput=Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && jumpCount<_maxJumps)
        {
            _performJump=true;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_xinput * Speed, _rb.velocity.y);

        if (_performJump) {
            _performJump=false;
            jumpCount++;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isGrounded = true;
        jumpCount=0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
        
    }
}