using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoldCoin : MonoBehaviour
{
	private Animator _animator;
	private bool _isCollected = false;
	private void Awake()
	{
			_animator = GetComponent<Animator>();
	}
	private void OnTriggerEnter2D(Collider2D other){
	if(other.CompareTag("Player")){
		if (_isCollected)
		{
			return;
		}
		BasicPlayerMovment player = other.GetComponent<BasicPlayerMovment>();
		if (player != null && _animator != null)
		{
			_isCollected=true;
			_animator.SetBool("isCollected", true);
			GameEventSystem.CollectValuable(1);
			Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
		}
}
}
}
