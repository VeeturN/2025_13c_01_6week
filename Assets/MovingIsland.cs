using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingIsland : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _PlatformSpeed;
    [SerializeField] private float _Distance = 200;
    
    private Vector2 _startPosition;
    private int _direction = 1;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPosition = _rb.position;
    }

    private void FixedUpdate()
    {
        float newPositionX = _rb.position.x + _direction * _PlatformSpeed * Time.fixedDeltaTime;
        
        if (Mathf.Abs(newPositionX - _startPosition.x) >= _Distance)
        {
            _direction *= -1; 
            newPositionX = _rb.position.x + _direction * _PlatformSpeed * Time.fixedDeltaTime;
        }
        
        _rb.MovePosition(new Vector2(newPositionX, _rb.position.y));
    }
}
