using UnityEngine;
using System.Collections;

public class ObjectDestructionPieces : MonoBehaviour
{
    [Header("Fizyka kawałków")]
    [SerializeField] private float launchForce = 1.5f;
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float drag = 1f;
    [SerializeField] private float waitOnGround = 2f;
    [SerializeField] private float fallLifetime = 5f;

    [Header("Konfiguracja sprite'ów")]
    [SerializeField] private ObjectDestructionConfig _config;
    [SerializeField, Range(0.1f, 1f)] private float _colliderScale = 0.8f;

    private bool _hasLaunched = false;
    private Sprite[] _sprites;

    public void Init(string objectType, string partType)
    {
        Debug.Log($"[ObjectDestructionPieces] Init for objectType={objectType}, partType={partType}");
        if (_config != null)
            _sprites = _config.GetSprites(objectType, partType);
        else
            Debug.LogWarning("[ObjectDestructionPieces] Brak przypisanego configu!");

        if (_sprites == null || _sprites.Length == 0)
    - beczka         Debug.LogWarning($"[ObjectDestructionPieces] Brak sprite'ów dla {objectType}/{partType}!");

        ApplySprites();
    }


    private void ApplySprites()
    {
        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogWarning("Brak sprite'ów dla tego wariantu obiektu!(ObjectDestructionPieces)");
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
            index++;
        }
    }
    public void LaunchPieces()
    {
        //to zapewnia że nie wywołą się wiele razy(bez tego nie działa)
        if (_hasLaunched) return;
        _hasLaunched = true;

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            rb = child.gameObject.AddComponent<Rigidbody2D>();

            rb.mass = 0.25f;
            rb.drag = drag;
            rb.angularDrag = 100f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.gravityScale = gravityScale;

            Collider2D col = child.GetComponent<Collider2D>();
            if (col != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Collider2D playerCol = player.GetComponent<Collider2D>();
                    if (playerCol != null)
                        Physics2D.IgnoreCollision(col, playerCol);
                }
            }

            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.2f)).normalized;
            rb.AddForce(randomDir * launchForce, ForceMode2D.Impulse);

            StartCoroutine(FallAfterGround(rb, child.gameObject));
        }
        StartCoroutine(DestroyAfterPiecesLaunched());
    }
    private IEnumerator DestroyAfterPiecesLaunched()
    {
        // tutaj czekamy aby dzieci się zniszczyły
        yield return new WaitForSeconds(waitOnGround + fallLifetime + 0.1f);
        Destroy(gameObject);
    }

    private IEnumerator FallAfterGround(Rigidbody2D rb, GameObject piece)
    {
        Collider2D col = piece.GetComponent<Collider2D>();

        bool touched = false;
        while (!touched)
        {
            if (rb.IsTouchingLayers())
                touched = true;
            yield return null;
        }

        yield return new WaitForSeconds(waitOnGround);
        //usunięcie kolizji aby mogły spaść
        if (col != null)
            Destroy(col);

        rb.gravityScale = gravityScale;
        rb.drag = drag;

        yield return new WaitForSeconds(fallLifetime);
        Destroy(piece);
    }
}
