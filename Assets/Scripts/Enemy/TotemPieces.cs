using UnityEngine;
using System.Collections;

public class TotemPieces : MonoBehaviour
{
    [SerializeField] private float _launchForce = 3f;
    [SerializeField] private float _lifetime = 4f;
    //[SerializeField] private string _playerTag = "Player";

    [Header("Konfiguracja części totemu")]
    [SerializeField] private TotemPiecesConfig _config;

    [SerializeField, Range(0.1f, 1f)]
    private float _colliderScale = 0.8f; // stała skala collidera, ustawiana raz w prefabie

    private bool _hasLaunched = false;
    private Sprite[] _sprites;

    public void Init(TotemScript.TotemType totemType, TotemScript.PartType partType)
    {
        if (_config != null)
            _sprites = _config.GetSprites(totemType, partType);

        ApplySprites();
        
    }

    private void ApplySprites()
    {
        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogWarning("Brak sprite'ów dla tego wariantu totemu!");//zabezpieczenie
            return;
        }

        
        int index = 0;
        foreach (Transform child in transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (index < _sprites.Length)
            {
                sr.sprite = _sprites[index];
                child.gameObject.SetActive(true);

                // ustawiam box collider2D raz dla każdego aktywnego dziecka
                var col = child.GetComponent<BoxCollider2D>();
                if (col == null)
                    col = child.gameObject.AddComponent<BoxCollider2D>();

                col.size = Vector2.one * _colliderScale;
                col.offset = Vector2.zero;
                col.enabled = false; // wyłączony na start
            }
            else
            {
                child.gameObject.SetActive(false); // wyłącz nadmiarowe dzieci (domyślnie 4)
            }

            index++;
        }
    }

    public void LaunchPieces()
    {
        if (_hasLaunched) return;
        _hasLaunched = true;

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = child.gameObject.AddComponent<Rigidbody2D>();//zabezpieczenie

            rb.gravityScale = 1.5f;
            rb.mass = 0.3f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // niemożliwy obrót
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            Collider2D col = child.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;

                // ignorowanie kolizji z graczem
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Collider2D playerCol = player.GetComponent<Collider2D>();
                    if (playerCol != null)
                    {
                        Physics2D.IgnoreCollision(col, playerCol);
                    }
                }
            }

            // losowy wyrzut 
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.2f)).normalized;
            rb.AddForce(randomDir * _launchForce, ForceMode2D.Impulse);
        }

        StartCoroutine(DestroyAfterDelay());//tutaj jest ta kurtyna(aby działo się równolegle)
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
}
