using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Pool;
using static UnityEngine.Rendering.DebugUI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] public GameObject bloodEffect;
    public UnitHealth enemyHealth = new UnitHealth(30, 30);

    [SerializeField] protected AudioClip axeHit;
    [SerializeField] public AudioClip groundSmashAudio;
    public AudioSource audioSourceSFX;

    protected Transform player;
    protected NavMeshAgent enemy;
    protected Animator enemyAnimator;

    protected bool isDead;
    protected bool hasStartedJump;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    public bool HasStartedJump
    {
        get { return hasStartedJump; }
        set { hasStartedJump = value; }
    }

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Attacking")]
    public float enemyDamage = 20;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public float damageDelay;
    public float animationDelay;

    //States
    public float attackRange;

    [Header("PlayerVariables")]
    public float giveRageOnKill = 15.0f;
    public float giveHealthOnKill = 5.0f;

    protected IObjectPool<EnemyAi> enemyPool;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();

        isDead = false;
        hasStartedJump = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ISPlayerInAttackRange() && !isDead) Invoke(nameof(AttackPlayer), damageDelay);
        else if (alreadyAttacked == false && !isDead) FollowPlayer();

        HandleJumping();
    }
    public bool ISPlayerInAttackRange()
    {
        if (transform.position.x - player.position.x < attackRange && transform.position.x - player.position.x > -attackRange && transform.position.z - player.position.z < attackRange && transform.position.z - player.position.z > -attackRange && transform.position.y - player.position.y < attackRange && transform.position.y - player.position.y > -attackRange && Physics.Linecast((transform.position + new Vector3(0, 1.8f, 0)), (player.position + new Vector3(0, 1.8f, 0))))
            return true;
        else return false;
    }
    private void FollowPlayer()
    {
        if (enemy == null || !enemy.isActiveAndEnabled) return;
        enemy.SetDestination(player.position);
        //transform.LookAt(new Vector3(player.position.x, 0.0f, player.position.z));
        //transform.LookAt(player, Vector3.up);
    }
    private void AttackPlayer()
    {
        if (enemy == null || !enemy.isActiveAndEnabled) return;
        if (!ISPlayerInAttackRange()) return;

        enemy.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            enemyAnimator.SetTrigger("Attack");
            transform.LookAt(new Vector3(player.position.x, 0.0f, player.position.z));


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void ToggleParticle(bool value)
    {
        bloodEffect.SetActive(value);
    }
    private void DisableParticle()
    {
        bloodEffect.SetActive(false);
    }
    public void OnTakeDamage(float damage)
    {
        if (isDead) return;
        enemyHealth.TakeDamage(damage);
        ToggleParticle(true);
        Invoke(nameof(DisableParticle), 0.6f);
        audioSourceSFX = GameObject.Find("AudioSourceSFX").GetComponent<AudioSource>();
        audioSourceSFX.clip = axeHit;
        audioSourceSFX.transform.position = player.position;
        audioSourceSFX.Play();

        if (enemyHealth.Health <= 0.0f)
        {
            isDead = true;
            GameManager.gameManager.playerHealth.Heal(giveHealthOnKill);
            GameManager.gameManager.playerRage.GiveRage(giveRageOnKill);
            enemy.SetDestination(transform.position);

            
            enemyAnimator.SetTrigger("Dead");
            GameManager.gameManager.playerKills++;

            GetComponent<CapsuleCollider>().enabled = false;
            Invoke(nameof(ReleaseEnemy), 3.0f);
        }
    }
    protected void ReleaseEnemy()
    {
        enemyPool.Release(this);

    }
    public void SetPool(IObjectPool<EnemyAi> pool)
    {
        enemyPool = pool;
    }
    public void HandleJumping()
    {
        if (enemy.isOnOffMeshLink && !hasStartedJump)
        {
            enemy.autoTraverseOffMeshLink = false;
            enemyAnimator.SetTrigger("Jump");
            Invoke(nameof(TurnOnAutoTraverse), 0.5f);
            StartCoroutine(DurationOffMeshLink());
        }
    }
    IEnumerator DurationOffMeshLink()
    {
        hasStartedJump = true;
        while (enemy.isOnOffMeshLink)
        {
            yield return null;
        }
        hasStartedJump = false;
    }
    public void TurnOnAutoTraverse()
    {
        enemy.autoTraverseOffMeshLink = true;
    }

}
