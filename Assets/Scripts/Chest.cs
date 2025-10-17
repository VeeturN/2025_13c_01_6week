using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    private Transform _player; 
    private float _interactionDistance = 3f; 
    private BasicPlayerMovment _playerObj;
    [SerializeField] private Animator _animator;
    private bool _isOpened = false;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerObj=_player.GetComponent<BasicPlayerMovment>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(_player.position, transform.position);

        if (distance <= _interactionDistance && Input.GetButtonDown("OpenChest") && Inventory.GetKeysCollected() > 0)
        {
            if (_isOpened) return;
            
            
            _animator.SetBool("IsOpen", true);
            //_playerObj.UseKey();
            _isOpened = true;
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
