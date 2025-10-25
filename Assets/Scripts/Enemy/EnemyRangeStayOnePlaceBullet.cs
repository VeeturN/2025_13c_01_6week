using UnityEngine;

public class EnemyRangeStayOnePlaceBullet : MonoBehaviour
{
    private EnemyRangeStayOnePlaceBulletConfig _config;
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _flyRight;

    public void Init(bool flyRight, EnemyRangeStayOnePlaceBulletConfig config)
    {
        

        _config = config;
        _flyRight = flyRight;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        // przesunięcie pozycji pocisku według configu
        if (_config.spawnOffset != Vector2.zero)
        {
            Vector3 offset = _config.spawnOffset;
            // uwzględnij kierunek lotu (lewo/prawo)
            offset.x *= _flyRight ? 1 : -1;
            transform.position += offset;
        }

        // przypisz AOC jeśli istnieje
        if (_config.animatorController != null && _animator != null)
            _animator.runtimeAnimatorController = _config.animatorController;

        // tutaj obrót sprita aby było ładniej
        Vector3 scale = transform.localScale;
        scale.x = !_flyRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
        
        UpdateCollider();

        // predkość
        _rb.velocity = new Vector2((_flyRight ? 1 : -1) * _config.speed, 0f);

        // zniszczenie po czasie aby nie leciało w nieskończoność
        Destroy(gameObject, _config.lifetime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            BasicPlayerMovment player = collision.gameObject.GetComponent<BasicPlayerMovment>();
            player.hit();
        }
        
        
        if(!collision.CompareTag("Enemy") && !collision.CompareTag("Pickable")&& !collision.CompareTag("Attack"))
            Destroy(gameObject);
    }

    private void UpdateCollider()
    {
        if (_collider == null || _spriteRenderer == null || _spriteRenderer.sprite == null || _config == null)
        {
            Debug.Log("Brakuje komponentów do aktualizacji kolizji w pocisku EnemySTAYINOEPLACE.");
            return;
        }

        var adj = _config.colliderAdjust;
        Vector2 localSize = _spriteRenderer.sprite.rect.size / _spriteRenderer.sprite.pixelsPerUnit;

        //poprawa aby colidery były dobre
        float newWidth = localSize.x * (1f - adj.trimLeftPercent - adj.trimRightPercent);
        float newHeight = localSize.y * (1f - adj.trimTopPercent - adj.trimBottomPercent);
        float offsetX = (adj.trimRightPercent - adj.trimLeftPercent) * localSize.x / 2f;
        float offsetY = (adj.trimBottomPercent - adj.trimTopPercent) * localSize.y / 2f;

        _collider.size = new Vector2(newWidth, newHeight);
        _collider.offset = new Vector2(offsetX, offsetY);
    }
    
    
    
}