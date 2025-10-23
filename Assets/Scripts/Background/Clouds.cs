using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _length;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition += Vector3.left * _speed * Time.deltaTime;
        if (transform.localPosition.x <= -_length)
            transform.localPosition += Vector3.right * _length*2; 
    }
}
