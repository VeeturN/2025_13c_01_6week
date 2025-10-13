using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private float _destroyTime;
    float timer;
    
    public void Init(float destroyTime)
    {
        _destroyTime = destroyTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > _destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = collision.collider.GetComponent<BasicPlayerMovment>();
            if (player != null)
            {
                player.hit();
                Destroy(gameObject);
            }
        }
    }
}
