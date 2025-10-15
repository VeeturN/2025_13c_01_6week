#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class TotemScript : MonoBehaviour ,IEnemy
{
    public enum TotemType { Totem1, Totem2, Totem3 }
    public enum PartType { Body, Head }
    public enum AnimationType { Idle, Attack, Hit, Destroyed }

    [Header("Ustawienia Totemu")]
    [SerializeField] private TotemType totemType = TotemType.Totem1;
    [SerializeField] private PartType partType = PartType.Body;
    [SerializeField] private AnimationType animationType = AnimationType.Idle;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Kolizja Totemu")]
    [SerializeField] private BoxCollider2D bodyCollider2D;
    [Header("Strzelanie Totemu")]
    [SerializeField] private bool isAutoAttack = true;
    [SerializeField] private float attackCooldown = 3f;
    private float attackCountdown = 0f;
    [Header("Animator Totemu")]
    [SerializeField] private Animator animator;
    [Header("Prefab pocisku Totemu")]
    [SerializeField] private GameObject projectilePrefab;
    [Header("Zdrowie Totemu")]
    [SerializeField] private int startHP = 3; // edytowalne w Inspectorze
    [SerializeField] private Image healthBar;  // opcjonalne, pasek nad głową (Image Type = Filled)
    [SerializeField] private RectTransform healthBarFill; // alternatywa: zielony RectTransform skalowany po X
    [Header("Animator - nazwy stanów")]
    [SerializeField] private string dieStateName = "Die";

    private bool isAttacking = false;
    private int currentHP;
    private float healthBarInitialScaleX = 1f;
    private bool isDyingStarted = false;

    // rozmiary głowy
    private readonly Vector2[] baseColliderSizes = new Vector2[]
    {
        new Vector2(2.905996f, 1.06034f),   
        new Vector2(1.06909f, 1.063114f),     
        new Vector2(1.199975f, 1.112224f)    
    };
    // ofsety głowy
    private readonly Vector2[] bodyColliderOffsets = new Vector2[]
    {
        new Vector2(-0.003210068f, -0.2032474f),   
        new Vector2(-0.02392387f, -0.2477759f),    
        new Vector2(0.1552958f, -0.2539251f)     
    };

    private void Start()
    {
        LoadSpriteFromProject();
        UpdateCollider();
        GetComponent<Rigidbody2D>().freezeRotation = true;
        attackCountdown = attackCooldown;
        currentHP = startHP;
        if (healthBarFill != null)
        {
            healthBarInitialScaleX = healthBarFill.localScale.x;
        }
        TakeDamage(); // zaktualizuj pasek na starcie
    }

    private void FixedUpdate()
    {
        UpdateCollider();
        if (isAutoAttack && !isAttacking)
        {
            attackCountdown -= Time.fixedDeltaTime;
            if (attackCountdown <= 0f)
            {
                StartCoroutine(AttackRoutine());
                attackCountdown = attackCooldown;
            }
        }
        else if (!isAutoAttack)
        {
            if (animator != null)
                animator.SetBool("IsAttacking", false);
        }
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;
        PlayAttack();
        float attackAnimLength = animator != null ? animator.GetCurrentAnimatorStateInfo(0).length : 0.5f;
        yield return new WaitForSeconds(attackAnimLength);
        if (animator != null)
            animator.SetBool("IsAttacking", false);
        PlayIdle();
        isAttacking = false;
    }

    private void UpdateCollider()
    {
        Vector3 scale = transform.localScale;
        if (bodyCollider2D != null)
        {
            bodyCollider2D.enabled = true;
            bodyCollider2D.size = new Vector2(baseColliderSizes[(int)totemType].x * scale.x, baseColliderSizes[(int)totemType].y * scale.y);
            bodyCollider2D.offset = bodyColliderOffsets[(int)totemType];
        }
    }

    public void PlayIdle()
    {
        animationType = AnimationType.Idle;
        LoadSpriteFromProject();
    }

    public void PlayHit()
    {
        animationType = AnimationType.Hit;
        if (animator != null)
        {
            animator.SetBool("isHit", true);
        }
        LoadSpriteFromProject();
    }

    public void PlayAttack()
    {
        animationType = AnimationType.Attack;
        if (animator != null)
        {
            animator.SetBool("IsAttacking", true);
        }
        LoadSpriteFromProject();
    }

    public void Attack()
    {
        animationType = AnimationType.Attack;
        if (animator != null)
        {
            animator.SetBool("IsAttacking", true);
        }
        LoadSpriteFromProject();
        SpawnProjectile();
    }

    public void DoneAttack()
    {
        animator.SetBool("IsAttacking", false);
    }

    private void SpawnProjectile()
    {
        Debug.Log("Totem strzela!");
        if (projectilePrefab != null)
        {
            var obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = obj.GetComponent<TotemBullet>();
            if (proj != null)
            {
                proj.Init(Vector2.left); //lewo
            }
        }else
        {
           // Debug.LogWarning("Brak przypisanego prefab pocisku!");
        }
    }
    
    
    public void TakeDamage()
    {
        if (startHP <= 0)
            return;

        float ratio = Mathf.Clamp01((float)currentHP / startHP);

        if (healthBar != null)
        {
            // tryb Image Filled
            healthBar.fillAmount = ratio;
        }
        else if (healthBarFill != null)
        {
            // tryb skalowania RectTransform po X (zachowując Y/Z)
            var s = healthBarFill.localScale;
            s.x = healthBarInitialScaleX * ratio;
            healthBarFill.localScale = s;
        }
    }
    
    private void LoadSpriteFromProject()
    {
#if UNITY_EDITOR
        if (spriteRenderer == null)
        {
           // Debug.LogError("Brak SpriteRenderer!");
            return;
        }
        // musze iść po ścieżce abym dotar do spritów z kodu...
        string path = $"Assets/Sprites/Totems/{totemType}/{partType}/{animationType}";

        // szukam tutaj czegokolwiek z spritem
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { path });
        if (guids.Length == 0)
        {
           // Debug.LogWarning($"Nie znaleziono sprite’a w {path}");
            return;
        }
        // wczytuje sprity
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
           // Debug.Log($"Załadowano sprite '{sprite.name}' z {assetPath}");
        }
        else
        {
            //Debug.LogWarning($"Nie udało się załadować sprite’a z {assetPath}");
        }
#endif
    }

    public void hit()
    {
        // tylko animacja otrzymania obrażeń – bez ruchu/odrzutu
        if (animator != null && animator.GetBool("IsDying"))
            return; // już martwy

        currentHP = Mathf.Max(0, currentHP - 1);
        PlayHit();
        TakeDamage();
        if (currentHP <= 0)
        {
            if (animator != null)
            {
                animator.SetBool("IsDying", true);
            }
            isAutoAttack = false;
            if (!isDyingStarted)
            {
                isDyingStarted = true;
                StartCoroutine(DieRoutine());
            }
        }
    }

    public void endHiting()
    {
        if (animator != null)
        {
            animator.SetBool("isHit", false);
        }
    }

    public void Die()
    {
        // Docelowo podpinane z Animation Event w klipie Die
        // Możesz tu dodać np. Destroy(gameObject) albo wyłączenie kolizji
        // Na razie zostawiamy puste, abyś mógł sam zdecydować o zachowaniu
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
        // opcjonalnie ukryj pasek
        if (healthBar != null) healthBar.enabled = false;
        if (healthBarFill != null) healthBarFill.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator DieRoutine()
    {
        // Czekaj aż animator wejdzie w stan "Die"
        if (animator != null)
        {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            while (!info.IsName(dieStateName))
            {
                yield return null;
                info = animator.GetCurrentAnimatorStateInfo(0);
            }
            // Odczekaj do końca klipu
            while (info.normalizedTime < 0.99f)
            {
                yield return null;
                info = animator.GetCurrentAnimatorStateInfo(0);
            }
        }
        Die();
    }
}