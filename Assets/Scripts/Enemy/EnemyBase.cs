using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public abstract class EnemyBase:MonoBehaviour
    {
        [SerializeField] protected int _startHP = 3;
        [SerializeField] protected float _attackInterval = 2f;
        public Image healthBar;
        protected bool _isHittedInAir = false;
        protected int _HP;
        protected BasicPlayerMovment _player;
        protected Rigidbody2D _rb;
        protected Animator _animator;
        protected float _hitTimer = 0f;
        
        public bool IsPlayerInAttackRange { get; set; } = false;
        protected virtual void Awake()
        {
            _HP = _startHP;
            _player = FindObjectOfType<BasicPlayerMovment>();
            _rb = GetComponent<Rigidbody2D>();
            if (_rb) _rb.freezeRotation = true;
            _animator = GetComponent<Animator>();
            UpdateHealthBar();
        }
        
        public virtual void hit(int damage)
        {
            _isHittedInAir = true;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(new Vector2((GameObject.FindGameObjectWithTag("Player").transform.position.x < transform.position.x ? 1 : -1) * 150, 150));
            _HP-=damage;
            UpdateHealthBar();
            _animator.SetBool("isHit", true);
        }
        
        
        protected void UpdateHealthBar()
        {
            if (healthBar != null)
                healthBar.fillAmount = (float)_HP / _startHP;
        }

        public virtual void TakeDamage()
        {
            UpdateHealthBar();
        }
        
        public virtual void endHiting()
        {
            _isHittedInAir = false;
            _animator.SetBool("isHit", false);
        }
        
    }
}