using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextZone : MonoBehaviour
{
    [SerializeField] string _text;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CrabbyTutorial crab = GameObject.FindGameObjectWithTag("TutorialCrab").GetComponent<CrabbyTutorial>();
            if(!crab.transform.GetChild(0).gameObject.activeInHierarchy)
            crab.Say(_text);
        }
    }
}
