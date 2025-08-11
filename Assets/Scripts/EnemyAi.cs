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
    protected Transform player;
    protected NavMeshAgent enemy;
    protected Animator enemyAnimator;

    [SerializeField] protected AudioClip axeHit;
    [SerializeField] public AudioClip groundSmashAudio;
    public AudioSource audioSourceSFX;

    public UnitHealth enemyHealth = new UnitHealth(30, 30);

    protected IObjectPool<EnemyAi> enemyPool;

    protected bool isDead;
    protected bool hasStartedJump;

    [Header("Attacking")]
    public float enemyDamage = 20;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public float damageDelay;
    public float animationDelay;
    public float attackRange;
    public float damageRange;

    [Header("PlayerVariables")]
    public float giveRageOnKill = 10.0f;
    public float giveHealthOnKill = 5.0f;

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

    protected void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();

        isDead = false;
        hasStartedJump = false;
    }

    protected void FixedUpdate()
    {
        if (ISPlayerInAttackRange(attackRange) && !isDead) Invoke(nameof(AttackPlayer), damageDelay);
        else if (alreadyAttacked == false && !isDead) FollowPlayer();

        HandleJumping();
    }
    public bool ISPlayerInAttackRange(float range)
    {
        if (transform.position.x - player.position.x < range && transform.position.x - player.position.x > -range && transform.position.z - player.position.z < range && transform.position.z - player.position.z > -range && transform.position.y - player.position.y < range && transform.position.y - player.position.y > -range && Physics.Linecast((transform.position + new Vector3(0, 1.8f, 0)), (player.position + new Vector3(0, 1.8f, 0))))
            return true;
        else return false;
    }
    protected void FollowPlayer()
    {
        if (enemy == null || !enemy.isActiveAndEnabled) return;

        enemy.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        if (enemy == null || !enemy.isActiveAndEnabled || isDead) return;
        if (!ISPlayerInAttackRange(attackRange)) return;

        enemy.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            
            enemyAnimator.SetTrigger("Attack");
            transform.LookAt(new Vector3(player.position.x, 0.0f, player.position.z));

            
            Invoke(nameof(ResetAttack), timeBetweenAttacks + animationDelay);
        }
    }
    protected void ResetAttack()
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

            //no body block while dead
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

    //Make pathfinding not run every fixedUpdate to increase performance when many enemies
    /*IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            if (isDead || alreadyAttacked) break;
            enemy.SetDestination(player.position);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.25f, 0.5f));
        }
    }*/
}
