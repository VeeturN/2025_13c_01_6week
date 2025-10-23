using UnityEngine;
using System.Collections;

public class TotemPieces : MonoBehaviour
{
    [Header("Fizyka kawałków")]
    [SerializeField] private float launchForce = 5f;      // siła wyrzutu
    [SerializeField] private float gravityScale = 3f;     // grawitacja
    [SerializeField] private float drag = 5f;             // opór powietrza
    [SerializeField] private float waitOnGround = 2f;   // czas na ziemi przed spadkiem
    [SerializeField] private float fallLifetime = 2f;     // czas spadania przed zniszczeniem

    [Header("Konfiguracja sprite'ów")]
    [SerializeField] private TotemPiecesConfig _config;
    [SerializeField, Range(0.1f, 1f)] private float _colliderScale = 0.8f;

    private bool _hasLaunched = false;
    private Sprite[] _sprites;

    private void ApplySprites()
    {
        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogWarning("Brak sprite'ów dla tego wariantu totemu!");
            return;
        }

        int index = 0;
        foreach (Transform child in transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                if (index < _sprites.Length)
                {
                    sr.sprite = _sprites[index];
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                    continue;
                }

                var col = child.GetComponent<BoxCollider2D>();
                if (col == null)
                    col = child.gameObject.AddComponent<BoxCollider2D>();

                col.size = sr.sprite.bounds.size * _colliderScale;
                col.offset = sr.sprite.bounds.center;
                col.enabled = true;
            }
            else
            {
                Debug.Log("brakujesprite renderer na " + child.name);
                child.gameObject.SetActive(false);
            }
            index++;
        }
    }

    public void Init(TotemScript.TotemType totemType, TotemScript.PartType partType)
    {
        if (_config != null)
            _sprites = _config.GetSprites(totemType, partType);

        ApplySprites();
    }

    public void LaunchPieces()
    {
        //aby nie było wiele razy
        if (_hasLaunched) return;
        _hasLaunched = true;

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = child.gameObject.AddComponent<Rigidbody2D>();

            rb.mass = 0.25f;
            rb.drag = drag;
            rb.angularDrag = 100f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.gravityScale = gravityScale;

            //brak kolizji gracza z kawałkiem
            Collider2D col = child.GetComponent<Collider2D>();
            if (col != null)
            {
                // ignorowanie gracza
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Collider2D playerCol = player.GetComponent<Collider2D>();
                    if (playerCol != null)
                        Physics2D.IgnoreCollision(col, playerCol);
                }
            }

            // losowy wyrzut
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.2f)).normalized;
            rb.AddForce(randomDir * launchForce, ForceMode2D.Impulse);

            // startujemy kontrole upadku i niszczenia w korutynie
            StartCoroutine(FallAfterGround(rb, child.gameObject));
        }
    }

    private IEnumerator FallAfterGround(Rigidbody2D rb, GameObject piece)
    {
        Collider2D col = piece.GetComponent<Collider2D>();

        // czekamy aż kawałek dotknie czegoś
        bool touched = false;
        while (!touched)
        {
            if (rb.IsTouchingLayers()) // jeśli dotyka jakiejkolwiek warstwy
                touched = true;
            yield return null;
        }

        // czas na ziemi
        yield return new WaitForSeconds(waitOnGround);

        // i spadek
        if (col != null)
            Destroy(col);

        // ale grawitacja i opór powietrza
        rb.gravityScale = gravityScale;
        rb.drag = drag;

        // niszczenie w trakcie spadania
        yield return new WaitForSeconds(fallLifetime);
        Destroy(piece);
    }
    public void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }
}
