using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
	
    private Animator _animator;
    private bool _isCollected = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    //po wejsciu w diamenty
    private void OnTriggerEnter2D(Collider2D other){
        //jak coliduje z player tagiem to usun coina i dodaj punkty	
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
                player.CollectGoldCoin();
			
                Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
		
		
    }
	
 
}