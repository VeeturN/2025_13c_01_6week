#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TotemScript : MonoBehaviour
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

    private bool isAttacking = false;

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
}