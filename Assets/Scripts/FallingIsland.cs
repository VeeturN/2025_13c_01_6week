using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingIsland : MonoBehaviour
{
    [FormerlySerializedAs("fallDelay")]
    [Header("Ustawienia platformy")]
    [SerializeField] private float _fallDelay = 1.5f;     // po jakim czasie zacznie spadać
    [SerializeField] private float _respawnDelay = 2f;    // po jakim czasie się zrespi
    [SerializeField] private float _destroyHeight = -10f; // wysokość na jakiej znika
    [SerializeField] private Rigidbody2D _rb;//to jest nasza fizyka w tym obiekcie
    
    //te dwie linijki odpowiadają za pozycję startową platformy
    private Vector3 _startPosition; 
    private Quaternion _startRotation;
    
    private bool _isFalling = false;//platforma spada?
    private bool _isRespawning = false;//platforma respi się?

    void Start()
    {
        //tutaj zabezpieczenie aby na pewno było przypisywane Rigidbody2D
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        _rb.bodyType = RigidbodyType2D.Kinematic;//tutaj aby stan początkowy był nieruchomy(kinematic)
        //po ustawieniu zostały przypisane pozycje aby w tym miejscy się respawnowała
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    void Update()
    {
        // tutaj jak minie wyznaczony pułap to dopiero zaczyna się czar restartu
        if (!_isRespawning && transform.position.y < _destroyHeight)
        {
            StartCoroutine(RespawnPlatform());//tutaj jest czerowne bo rider mówi "jak będzie w lubi będzie bardzo kosztowne dla kompa" ale w tym przypdaku raz na spadek to robimy wiec ok
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ten kto udeżył to gracz? i czy platforma nie spadłą
        if (!_isFalling && collision.gameObject.CompareTag("Player"))
        {
            // tutaj robię "listę" gdzie jest kolizja między graczem a tym obiektem
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f) // tutaj sprawdzomy czy od góry dotyka platformy jeśli tak idz dalej
                {
                    StartCoroutine(FallAfterDelay());//to jest opóźnienie spadania
                    break;
                }
            }
        }
    }

    //to jest procedura spadania
    private IEnumerator FallAfterDelay()
    {
        _isFalling = true;//flaga on
        yield return new WaitForSeconds(_fallDelay);//czeka określony czas
        GetComponent<Collider2D>().enabled = false;
        _rb.bodyType = RigidbodyType2D.Dynamic; // Platforma zaczyna spadać bo zmieniam na dynamic
        _rb.AddForce(new Vector2(0, -500));
    }

    //to jest proces respawnowania
    private IEnumerator RespawnPlatform()
    {
        _isRespawning = true;
        GetComponent<Collider2D>().enabled = true;

        // fizyka i widoczność "on"
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        GetComponent<SpriteRenderer>().enabled = false;//tutaj tak samo jak respawn platform
        GetComponent<Collider2D>().enabled = false;

        // zanim się odrodzi poczekać konkretny czas
        yield return new WaitForSeconds(_respawnDelay);

        // tutaj włączam ponowne początkowe ustawienia platformy
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        //restart flag
        _isFalling = false;
        _isRespawning = false;
    }
}
