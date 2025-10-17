using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableAnimationScript : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void ChangeParaInAmimationIsCollected()
    {
        _animator.SetBool("isCollected", true);
    }
}
