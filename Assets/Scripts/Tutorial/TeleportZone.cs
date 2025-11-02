using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleportZone : MonoBehaviour
{
    [SerializeField] Transform _teleportPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CrabbyTutorial crab = GameObject.FindGameObjectWithTag("TutorialCrab").GetComponent<CrabbyTutorial>();
            crab.transform.GetChild(0).gameObject.SetActive(false);
            crab.transform.position = _teleportPoint.position;
        }
    }
}
