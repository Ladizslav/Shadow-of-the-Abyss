using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health), typeof(NavMeshAgent))]
public class EnemyEndless : MonoBehaviour
{
    [SerializeField] private float attackCD = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float aggroRange = 8f;
    [SerializeField] private float baseMoveSpeed = 3.5f;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damageScaling = 0.1f;

    private NavMeshAgent agent;
    private Animator animator;
    private Health health;
    private Transform player;
    private float timePassed;
    private float currentDamage;
    private int currentWave;
    private bool isDead = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Start()
    {
        agent.speed = baseMoveSpeed;
        agent.stoppingDistance = attackRange * 0.9f;
        health.OnDeath += OnEnemyDeath;
    }

    void OnDestroy()
    {
        health.OnDeath -= OnEnemyDeath;
    }

    public void InitializeForWave(int wave)
    {
        currentWave = wave;
        currentDamage = baseDamage * (1 + (wave * damageScaling));
    }

    void Update()
    {
        if (isDead || health.currentHealth <= 0 || player == null) return;

        UpdateMovement();
        UpdateAttack();
    }

    void UpdateMovement()
    {
        if (Vector3.Distance(transform.position, player.position) <= aggroRange)
        {
            agent.SetDestination(player.position);
            animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }

        if (agent.velocity.magnitude > 0.1f)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void UpdateAttack()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= attackCD &&
            Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
            timePassed = 0;
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");

        if (player.TryGetComponent<Health>(out var playerHealth))
        {
            playerHealth.TakeDamage(currentDamage);
        }
    }

    void OnEnemyDeath()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("death");

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        EndlessModeManager.Instance?.EnemyDefeated();
        Destroy(gameObject, 2f);
    }

    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>()?.StartDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>()?.EndDealDamage();
    }
}