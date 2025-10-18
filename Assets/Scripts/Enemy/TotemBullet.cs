using UnityEngine;


[RequireComponent(typeof(EnemyBullet))]
public class TotemBullet : MonoBehaviour
{
    [SerializeField] private Animator _animator; 
    [SerializeField] private float _speed = 8f; 
    [SerializeField] private string _destroyParamName = "IsDestroy";

    private EnemyBullet _enemyBullet;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    private void Awake()
    {
        _enemyBullet = GetComponent<EnemyBullet>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        if (_enemyBullet != null)
            _enemyBullet.enabled = false;
    }

    public void Init(Vector2 dir)
    {
        // nadaje prędkość
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D != null)
            _rigidbody2D.velocity = dir.normalized * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // sprawdza czy trafił w gracza nie jego dzieci
        GameObject rootHit = collision.attachedRigidbody != null ? collision.attachedRigidbody.gameObject : collision.gameObject;
        BasicPlayerMovment player = rootHit.GetComponent<BasicPlayerMovment>();
        if (player != null)
        {
            player.hit();
        }
        //niszczy się jak trafi w cokolwiek innego
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Pickable") && !collision.CompareTag("Attack"))
        {
            BeginDestroy();
        }
    }

    private void BeginDestroy()
    {
        if (_collider2D != null) _collider2D.enabled = false;
        if (_rigidbody2D != null) _rigidbody2D.velocity = Vector2.zero;
        //zabezpieczenie jeśli jest animator wywołuje zniszczenie
        if (_animator != null && !string.IsNullOrEmpty(_destroyParamName) && AnimatorHasBool(_animator, _destroyParamName))
        {
            _animator.SetBool(_destroyParamName, true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // w animacji 
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private static bool AnimatorHasBool(Animator animator, string paramName)//zabeczpieczenie
    {
        foreach (var p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Bool && p.name == paramName)
                return true;
        }
        return false;
    }
}

