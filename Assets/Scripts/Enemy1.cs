using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private Transform patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 0f; // Zasięg wykrywania gracza
    private BasicPlayerMovment player;
    private Rigidbody2D _rb;
    private bool movingToB = true;
    
    [HideInInspector] public bool isPlayerInAttackRange = false;
    private float hitTimer = 0f;
    private Animator animator;
    
    private float attackAnimDuration = 0.5f; // czas trwania animacji ataku
    private float attackAnimTimer = 0f;
    private float attackAnimLeadTime = 0.3f; // ile wcześniej ma się zacząć animacja
    [SerializeField] private float attackInterval = 1f; //predkosc ataku
    
    private void Awake()
    {
        player = FindObjectOfType<BasicPlayerMovment>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true; // Blokada rotacji przeciwnika
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (player && IsPlayerInDetectionRange())
        {
            MoveTowardsPlayer();
        }
        else
        {
            Patrol();
        }

        if (isPlayerInAttackRange)
        {
            hitTimer += Time.fixedDeltaTime;

            // Animacja startuje wcześniej
            if (hitTimer >= attackInterval - attackAnimLeadTime && attackAnimTimer <= 0f)
            {
                if (animator != null)
                    animator.SetBool("IsAttack", true);
                attackAnimTimer = attackAnimDuration + attackAnimLeadTime;
            }

            // Napis HIT pojawia się później
            if (hitTimer >= attackInterval)
            {
                Debug.Log("HIT");
                hitTimer = 0f;
            }
        }

        if (attackAnimTimer > 0f)
        {
            attackAnimTimer -= Time.fixedDeltaTime;
            if (attackAnimTimer <= 0f && animator != null)
                animator.SetBool("IsAttack", false);
        }
        else if (!isPlayerInAttackRange && animator != null)
        {
            animator.SetBool("IsAttack", false);
        }
    }

    private bool IsPlayerInDetectionRange()
    {
        float playerX = player.transform.position.x;
        return playerX >= Mathf.Min(patrolPointA.position.x, patrolPointB.position.x) &&  //-detectionRange 
               playerX <= Mathf.Max(patrolPointA.position.x, patrolPointB.position.x) ;   //+detectionRange XD Nawet jak jest 0 to szuka dalej beka
    }

    private void MoveTowardsPlayer()
    {
        float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        _rb.velocity = new Vector2(direction * speed, _rb.velocity.y);
    }

    private void Patrol()
    {
        float targetX = movingToB ? patrolPointB.position.x : patrolPointA.position.x;
        float direction = Mathf.Sign(targetX - transform.position.x);
        _rb.velocity = new Vector2(direction * speed, _rb.velocity.y);

        // Sprawdzenie czy przeciwnik dotarł do celu (z tolerancją)
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            movingToB = !movingToB;
        }
    }
}