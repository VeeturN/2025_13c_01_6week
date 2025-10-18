using UnityEngine;
using UnityEngine.UI;
using Enemy;
using UnityEngine.Serialization;

public class TotemScript : Enemy.EnemyBase
{
    public enum TotemType { Totem1, Totem2, Totem3 }
    public enum PartType { Body, Head }
    [Header("Kierunek Totemu")]
    [SerializeField] private bool facingRight = true; //jak zaznacze patrzy w lewo

    
    [Header("Konfiguracja Totemu (ScriptableObject)")]
    [SerializeField] private TotemConfig config;

    [Header("Kolizja Totemu")]
    [SerializeField] private BoxCollider2D bodyCollider2D;
    
    private float healthBarInitialScaleX = 1f;

    private Animator _totemAnimator;
    private float attackCountdown;
    private bool isAttacking = false;
    private bool isDyingStarted = false;
    private bool isAutoAttack;
    private float attackCooldown;
    private GameObject projectilePrefab;
    private string dieStateName;
    
    [SerializeField] private GameObject _deathPiecesPrefab;

    // dane kolizji
    private readonly Vector2[] baseColliderSizes = new Vector2[]
    {
        new Vector2(2.905996f, 1.06034f),
        new Vector2(1.06909f, 1.063114f),
        new Vector2(1.199975f, 1.112224f)
    };
    private readonly Vector2[] bodyColliderOffsets = new Vector2[]
    {
        new Vector2(-0.003210068f, -0.2032474f),
        new Vector2(-0.02392387f, -0.2477759f),
        new Vector2(0.1552958f, -0.2539251f)
    };
    
    protected override void Awake()
    {
        base.Awake();

        _totemAnimator = GetComponent<Animator>();

        //wczytuje rzeczy z scripttable object z zabezpieczeniem(config)
        if (config != null)
        {
            //tutaj przypisanie animatora i zabezpieczenie przed brakiem controllera
            if (_totemAnimator != null && config.animatorController != null)
            {
                _totemAnimator.runtimeAnimatorController = config.animatorController;
            }
            else
            {
                Debug.LogWarning($"Brak przypisanego AnimatorController w configu dla {gameObject.name}");
            }
            
            isAutoAttack = config.isAutoAttack;
            attackCooldown = _attackInterval;
            projectilePrefab = config.projectilePrefab;
            dieStateName = config.dieStateName;

            _startHP = config.startHP;
            _HP = _startHP;
            //tutaj mam to patrzenie lewo prawo
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
            transform.localScale = scale;
        }
        else
        {
            Debug.LogWarning("Brak przypisanego TotemConfig do " + gameObject.name);
        }
    }

    private void Start()
    {
        UpdateCollider();
        attackCountdown = attackCooldown;
        PlayIdle();
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
            _totemAnimator.SetBool("IsAttacking", false);
        }
    }

    //tutaż jest używana tzw kurtna do nieblokowania gry
    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;
        PlayAttack();
        //tutaj czekam po za główną grą
        yield return new WaitUntil(() =>
            !_totemAnimator.GetBool("IsAttacking"));

        isAttacking = false;
    }

    private void UpdateCollider()
    {
        int index = (int)config.totemType;
        Vector3 scale = transform.localScale;

        //specjalne zachowanie dla totem3
        if (config.totemType == TotemType.Totem3)
        {
            if (scale.x > 0) //prawo
            {
                bodyCollider2D.size = new Vector2(1.219414f * scale.x, baseColliderSizes[index].y * scale.y);
                bodyCollider2D.offset = new Vector2(0.1394997f * scale.x, bodyColliderOffsets[index].y);
            }
            else //lewo
            {
                bodyCollider2D.size = new Vector2(baseColliderSizes[index].x * Mathf.Abs(scale.x), baseColliderSizes[index].y * scale.y);
                bodyCollider2D.offset = new Vector2(bodyColliderOffsets[index].x * Mathf.Sign(scale.x), bodyColliderOffsets[index].y);
            }
        }
        else
        {
            // standardowe zachowanie dla Totem1 i Totem2
            bodyCollider2D.size = new Vector2(baseColliderSizes[index].x * Mathf.Abs(scale.x),
                baseColliderSizes[index].y * scale.y);
            bodyCollider2D.offset = new Vector2(bodyColliderOffsets[index].x * Mathf.Sign(scale.x),
                bodyColliderOffsets[index].y);
        }
    }
    #region Animacje
    public void PlayIdle()
    {
        if (_totemAnimator == null) return;

        _totemAnimator.SetBool("IsAttacking", false);
        _totemAnimator.SetBool("isHit", false);
        _totemAnimator.SetBool("IsDying", false);
    }

    public void PlayHit()
    {
        if (_totemAnimator == null) return;
        _totemAnimator.SetBool("isHit", true);
    }

    public void PlayAttack()
    {
        if (_totemAnimator == null) return;
        _totemAnimator.SetBool("IsAttacking", true);
    }

    //wywołanie w eventach
    public void DoneAttack()
    {
        if (_totemAnimator != null)
            _totemAnimator.SetBool("IsAttacking", false);
        PlayIdle();
    }
    #endregion
    private void SpawnProjectile()//wywołanie w eventach
    {
        if (projectilePrefab == null) return;//zabezpieczenie

        var obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);//brak rotacji i pozycja startowa
        var enemyBullet = obj.GetComponent<EnemyBullet>();//zabezpiezczenie jakby nic nie było ale można dać coś innego
        if (enemyBullet != null)
        {
            float dirX = transform.localScale.x < 0 ? 1f : -1f;
            enemyBullet.Init(new Vector2(dirX, 0f));
        }
        else
        {//difultowo totem bulet
            var totemBullet = obj.GetComponent<TotemBullet>();
            if (totemBullet != null)
            {
                float dirX = transform.localScale.x < 0 ? 1f : -1f;
                totemBullet.Init(new Vector2(dirX, 0f));
            }
        }
    }
    
    public override void hit(int dmg)
    {
        if (_totemAnimator != null && _totemAnimator.GetBool("IsDying"))
            return;

        _HP -=dmg;
        PlayHit();
        TakeDamage();

        if (_HP <= 0)
        {
            if (_totemAnimator != null)
                _totemAnimator.SetBool("IsDying", true);

            isAutoAttack = false;

            if (!isDyingStarted)
            {
                isDyingStarted = true;
                StartCoroutine(DieRoutine());
            }
        }
    }
    private System.Collections.IEnumerator DieRoutine()
    {
        if (_totemAnimator != null && _totemAnimator.layerCount > 0)//zabezpieczenie
        {
            var info = _totemAnimator.GetCurrentAnimatorStateInfo(0);
            while (!info.IsName(dieStateName))
            {
                yield return null;
                info = _totemAnimator.GetCurrentAnimatorStateInfo(0);
            }
            while (info.normalizedTime < 0.99f)//tutaj czekam aż animacja się skończy
            {
                yield return null;
                info = _totemAnimator.GetCurrentAnimatorStateInfo(0);
            }
        }
        Die();
    }
    public void Die()
    {
        //wyłaczenie coliderów 
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;
        
        if (_deathPiecesPrefab != null)
        {
            //tworzy kopię prefaba totempices
            var script = Instantiate(_deathPiecesPrefab, transform.position, Quaternion.identity).GetComponent<TotemPieces>();
            if (script != null)
            {
                script.Init(config.totemType, config.partType);
                script.LaunchPieces();
            }
        }

        Destroy(gameObject);
    }
}