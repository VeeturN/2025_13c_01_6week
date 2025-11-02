using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : Saveable
{
    [SerializeField] private GameObject[] _itemsToDrop;
    private Transform _player; 
    private float _interactionDistance = 3f; 
    private BasicPlayerMovment _playerObj;
    private Animator _animator;
    private bool _isOpened = false;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        GameEventSystem.OnChestOpen += OpenChest;
        _isOnScene = true;
    }
    private void OnDestroy()
    {
        GameEventSystem.OnChestOpen -= OpenChest;
    }

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerObj=_player.GetComponent<BasicPlayerMovment>();
    }
    private void OpenChest()
    {
        Debug.Log("Otwieranie");
        float distance = Vector2.Distance(_player.position, transform.position);

        if (distance <= _interactionDistance && Inventory.GetKeysCollected() > 0)
        {
            if (_isOpened) return;


            _animator.SetBool("IsOpen", true);
            Inventory.SetKeysCollected(Inventory.GetKeysCollected() - 1);
            _isOpened = true;
            _isOnScene = false;
        }
    }

    private void DestroyAfterAnimation()
    {
        StartCoroutine(WaitForToCollectCoroutine());
    }

    private IEnumerator WaitForToCollectCoroutine()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        float chestWidth = GetComponent<BoxCollider2D>().bounds.size.x;

        Vector3 dropPos1 = transform.position + new Vector3(-chestWidth * 0.3f, 0, 0);
        Vector3 dropPos2 = transform.position + new Vector3(chestWidth * 0.3f, 0, 0);

        BoxCollider2D box1 = Instantiate(_itemsToDrop[UnityEngine.Random.Range(0, _itemsToDrop.Length)], dropPos1, Quaternion.identity).GetComponent<BoxCollider2D>();
        BoxCollider2D box2 = Instantiate(_itemsToDrop[UnityEngine.Random.Range(0, _itemsToDrop.Length)], dropPos2, Quaternion.identity).GetComponent<BoxCollider2D>();

        box1.enabled = false;
        box2.enabled = false;

        yield return new WaitForSeconds(1);

        box1.enabled = true;
        box2.enabled = true;

        Destroy(gameObject);
    }
}
