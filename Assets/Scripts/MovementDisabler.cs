using UnityEngine;
using System.Collections;

public class MovementDisabler : MonoBehaviour
{
    [SerializeField] private float disableTime = 3f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        BasicPlayerMovment player = other.GetComponent<BasicPlayerMovment>();
        if (player != null)
        {
            StartCoroutine(DisableMovement(player));
        }
    }

    private IEnumerator DisableMovement(BasicPlayerMovment player)
    {
        player.StopMovement();
        yield return new WaitForSeconds(disableTime);
        player.ResumeMovement();
    }
}