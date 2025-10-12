using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to coś powoduje że unity samo doda colidera i platformeffect jeśli nie ma
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
public class JumpThroughPlatform : MonoBehaviour
{
    private PlatformEffector2D _effector;
    private Collider2D _collider;

    private void Awake()
    {
        _effector = GetComponent<PlatformEffector2D>();
        _collider = GetComponent<Collider2D>();
    }

    // Wywołujemy np. po naciśnięciu przycisku S w grze
    public void AllowPlayerToFallThrough(GameObject player, float duration = 1.3f)
    {
        //to pozwala mi zrobić akcję w czasie bez blokowania gry
        StartCoroutine(FallThrough(player, duration));
    }

    private IEnumerator FallThrough(GameObject player, float duration)
    {
        // tutaj sprawdzam czy jest kolider gracz jeśli nie ma to nic nie robi
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if(playerCollider == null)
            yield break;
        //wyłączam kolicję miedzy graczem a platformą
        Physics2D.IgnoreCollision(playerCollider, _collider, true);
        yield return new WaitForSeconds(duration);//odczekuje konkreny czas
        Physics2D.IgnoreCollision(playerCollider, _collider, false);//i znowu włączam kolizję
    }
}
