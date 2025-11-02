using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabbyTutorial : MonoBehaviour
{
    [SerializeField] TypewriterEffect _typewriter;
    public void Say(string text)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        _typewriter.Init(text);
    }
}
