using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableAnimationScript : MonoBehaviour
{
    private void Awake()
    {
        GameEventSystem.OnValuableCollected += ChangeParaInAmimationIsCollected;   
    }
    private void ChangeParaInAmimationIsCollected(AbstractValuable coin)
    {
        Animator myAnimator = coin.GetComponent<Animator>();
        myAnimator.SetBool("isCollected", true);
    }
}
