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
            Debug.LogWarning($"[ObjectDestructionPieces] Brak sprite'ów dla {objectType}/{partType}!");

        ApplySprites();
    }


    private void ApplySprites()
    {
        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogWarning("Brak sprite'ów dla tego wariantu obiektu!(ObjectDestructionPieces)");
            return;
        }
        // dzieci na podstawie liczby spritów
        for (int i = 0; i < _sprites.Length; i++)
        {
            GameObject piece = new GameObject($"Piece_{i}");//nazwa obiektu
            piece.transform.SetParent(transform);//dziecko
            piece.transform.localPosition = Vector3.zero; 
            piece.layer = LayerMask.NameToLayer("TotemPices");//warstwa kolizji
            var sr = piece.AddComponent<SpriteRenderer>();
            sr.sprite = _sprites[i];//przypisanie sprite
            
            //odpowiedzialne żeby pojawiało się za totemem
            sr.sortingLayerName = "UI"; 
            sr.sortingOrder = 0; 
            
            var col = piece.AddComponent<BoxCollider2D>();
            col.size = sr.sprite.bounds.size * _colliderScale;
            col.offset = sr.sprite.bounds.center;
            col.enabled = true;
        }

        Debug.Log($"[ObjectDestructionPieces] Utworzono {_sprites.Length} kawałków.");
    }

    public void LaunchPieces()
    {
        //to zapewnia że nie wywołą się wiele razy(bez tego nie działa)
        if (_hasLaunched) return;
        _hasLaunched = true;

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;//zabezpieczenie

            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            rb = child.gameObject.AddComponent<Rigidbody2D>();

            //fizyka
            rb.mass = 0.25f;
            rb.drag = drag;
            rb.angularDrag = 100f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.gravityScale = gravityScale;

            Collider2D col = child.GetComponent<Collider2D>();
            if (col != null)
            {   //brak kolizji z graczem
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Collider2D playerCol = player.GetComponent<Collider2D>();
                    if (playerCol != null)
                        Physics2D.IgnoreCollision(col, playerCol);
                }
                //brak kolizji z wrogami
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var enemy in enemies)
                {
                    Collider2D enemyCol = enemy.GetComponent<Collider2D>();
                    if (enemyCol != null)
                        Physics2D.IgnoreCollision(col, enemyCol);
                }
            }

            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.2f)).normalized;
            rb.AddForce(randomDir * launchForce, ForceMode2D.Impulse);//losowy kierunek

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
        if (rb == null || piece == null) yield break; // zabezpieczenie
        Collider2D col = piece.GetComponent<Collider2D>();

        bool touched = false;
        while (!touched)
        {
            // tutaj czy został zniszczony w międzyczasie
            if (rb == null || piece == null)
                yield break;

            if (rb.IsTouchingLayers())
                touched = true;

            yield return null;
        }

        yield return new WaitForSeconds(waitOnGround);

        if (col != null)
            Destroy(col);

        // 🔹 ponowne sprawdzenie przed użyciem
        if (rb == null || piece == null)
            yield break;

        rb.gravityScale = gravityScale;
        rb.drag = drag;

        yield return new WaitForSeconds(fallLifetime);

        if (piece != null)
            Destroy(piece);
    }

}
