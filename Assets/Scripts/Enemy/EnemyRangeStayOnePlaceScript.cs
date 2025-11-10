using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class EnemyRangeStayOnePlaceScript : Enemy.EnemyBase
{
    // Typ przeciwnika (Totem / Armata / Małża)
    public enum EnemyRangeType { Totem, Armata, Malza }
    public enum TotemType { Totem1, Totem2, Totem3 }
    public enum PartType { Body, Head }

    [Header("Rodzaj przeciwnika stojącego w miejscu")]
    [SerializeField] private EnemyRangeType enemyRangeType = EnemyRangeType.Totem;

    [Header("Kierunek Przeciwnika")]
    [SerializeField] private bool lookLeft = true; //jak zaznacze patrzy w lewo

    [Header("Konfiguracja przeciwnika (ScriptableObject)")]
    [SerializeField] private EnemyRangeStayOnePlaceConfig config;

    [Header("Kolizja Preciwnika")]
    [SerializeField] private BoxCollider2D bodyCollider2D;
    
    private Animator _totemAnimator;
    private float attackCountdown;
    private bool isAttacking = false;
    private bool isDyingStarted = false;
    private bool isAutoAttack;
    private float attackCooldown;
    private GameObject projectilePrefab;
    private string dieStateName;
    
    [Header("Kawałki śmierci")]
    [SerializeField] private GameObject _deathPiecesPrefab;
    
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
            scale.x = Mathf.Abs(scale.x) * (lookLeft ? 1 : -1);
            transform.localScale = scale;
            
            UpdateCollider();
        }
        else
        {
            Debug.LogWarning("Brak przypisanego EnemyRangeStayOnePlaceConfig do " + gameObject.name);//zabezpieczenie
        }
    }

    private void Start()
    {
        base.Start();
        attackCountdown = attackCooldown;
        PlayIdle();
        _EnemyPrefabName = "TotemTemplate";
        _isTotem = true;
        switch (config.enemyRangeType)
        {
            case EnemyRangeType.Armata:
                _configName = "Armata";
                break;
            case 
            EnemyRangeType.Malza:
                _configName = "Malza";
                break;
            case EnemyRangeType.Totem:
                switch (config.TotemType)
                {
                    case TotemType.Totem1:
                        switch (config.PartType)
                        {
                            case PartType.Body:
                                _configName = "Totem1Body";
                                break;
                            case PartType.Head:
                                _configName = "Totem1Head";
                                break;
                        }
                        break;
                    case TotemType.Totem2:
                        switch (config.PartType)
                        {
                            case PartType.Body:
                                _configName = "Totem2Body";
                                break;
                            case PartType.Head:
                                _configName = "Totem2Head";
                                break;
                        }
                        break;
                    case TotemType.Totem3:
                        switch (config.PartType)
                        {
                            case PartType.Body:
                                _configName = "Totem3Body";
                                break;
                            case PartType.Head:
                                _configName = "Totem3Head";
                                break;
                        }
                        break;
                }
                break;
        }
    }
    private void FixedUpdate()
    {
        //wyliczanie casu do nowego ataku
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
    private IEnumerator AttackRoutine()
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
        if (bodyCollider2D == null || config == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        //rozmiar sprite
        Vector2 localSize = sr.sprite.rect.size / sr.sprite.pixelsPerUnit;

        // dopasowanie do skali obiektu
        var adj = config.colliderAdjust;

        // specjalne traktowanie totem1 i totem3 przy lookLeft = false
        bool shouldMirrorTrim = !lookLeft &&
                                (config.enemyRangeType == EnemyRangeType.Totem &&
                                 config.TotemType.HasValue &&
                                 (config.TotemType.Value == TotemType.Totem1 || config.TotemType.Value == TotemType.Totem3));

        if (shouldMirrorTrim)//czy patrzy w lewo
        {
            var mirrored = adj;
            mirrored.trimLeftPercent = adj.trimRightPercent;
            mirrored.trimRightPercent = adj.trimLeftPercent;
            adj = mirrored;
        }

        // nowy rozmiar collidera
        float newWidth = localSize.x * (1f - adj.trimLeftPercent - adj.trimRightPercent);
        float newHeight = localSize.y * (1f - adj.trimTopPercent - adj.trimBottomPercent);
        float offsetX = (adj.trimRightPercent - adj.trimLeftPercent) * localSize.x / 2f;
        float offsetY = (adj.trimBottomPercent - adj.trimTopPercent) * localSize.y / 2f;

        // jego ustawienie
        bodyCollider2D.size = new Vector2(newWidth, newHeight);
        bodyCollider2D.offset = new Vector2(offsetX, offsetY);

        if (!lookLeft)
        {
            Vector2 o = bodyCollider2D.offset;
            o.x *= -1;
            bodyCollider2D.offset = o;
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
    
    //pociski
    private void SpawnProjectile()
    {
        if (projectilePrefab == null || config.projectileConfig == null) return;

        var obj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        var bulletScript = obj.GetComponent<EnemyRangeStayOnePlaceBullet>();
        if (bulletScript != null)
        {
            bulletScript.Init(!lookLeft, config.projectileConfig); // <-- TU WYWOŁUJEMY Init()
        }
        else
        {
            Debug.LogWarning("Brak skryptu TotemBullet na prefabie projectilePrefab!");
        }
    }
    
    public override void hit(int dmg, float xPos)
    {
        if (_totemAnimator != null && _totemAnimator.GetBool("IsDying"))
            return;

        _HP -= dmg;
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
    private IEnumerator DieRoutine()
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
            Instantiate(_reward, transform.position, Quaternion.identity);
            var script = Instantiate(_deathPiecesPrefab, transform.position, Quaternion.identity)
                .GetComponent<ObjectDestructionPieces>();
            if (script != null)
            {
                // jeśli Totem – użyj TotemType/PartType
                string objectType = config.TotemType.HasValue ? config.TotemType.Value.ToString() : config.enemyRangeType.ToString();
                string partType = config.PartType.HasValue ? config.PartType.Value.ToString() : "Body";

                script.Init(objectType, partType);
                script.LaunchPieces();
            }
        }


        Destroy(gameObject);
    }

    public void setConfig(EnemyRangeStayOnePlaceConfig config)
    {
        this.config = config;
    }
}