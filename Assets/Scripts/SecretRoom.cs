using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretRoom : MonoBehaviour
{
    Tilemap _tilemap;
    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(MakeTransparent(0.7f));
        }
    }
    private IEnumerator MakeTransparent(float value)
    {
        float a = 1;
        while (_tilemap.color.a > value)
        {
            a -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            _tilemap.color = new Color(1, 1, 1, a);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(MakeNontransparent());
        }
    }
    private IEnumerator MakeNontransparent()
    {
        float a = _tilemap.color.a;
        while (_tilemap.color.a < 1)
        {
            a += 0.01f;
            yield return new WaitForSeconds(0.01f);
            _tilemap.color = new Color(1, 1, 1, a);
        }
    }
}
