using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class HookerEnemyAi : EnemyAi
{
    private float hookTimer = 0f;
    private float nextHookRandomness = 0f;

    [Header("Hook Settings")]
    public float hookCooldown = 15f;
    public float hookSpeed = 20f;
    public float hookRange = 25f;
    public Vector3 hookSize = new Vector3(0.2f, 0.2f, 2f);
    public Transform hookFirePoint;

    new private IObjectPool<HookerEnemyAi> enemyPool;

    new void Awake()
    {
        base.Awake();

        enemyHealth = new UnitHealth(100, 100);

        // Create a fire point if none assigned
        if (hookFirePoint == null)
        {
            GameObject fp = new GameObject("HookFirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(0, 1.5f, 0.5f);
            hookFirePoint = fp.transform;
        }
    }

    new void FixedUpdate()
    {
        hookTimer += Time.fixedDeltaTime;

        nextHookRandomness = Random.Range(-1.5f, 1.5f);
        
        if (hookTimer >= hookCooldown + nextHookRandomness && !isDead)
        {
            FireHookRoutine();
            hookTimer = 0;
        }
        else if(!alreadyAttacked && !IsDead)
        {
            FollowPlayer();
        }

        HandleJumping();
    }

    new public void ReleaseEnemy()
    {
        enemyPool.Release(this);
    }
    public void SetPool(IObjectPool<HookerEnemyAi> pool)
    {
        enemyPool = pool;
    }

    private void FireHookRoutine()
    {
        //yield return new WaitForSeconds(Random.Range(0.5f, 1.5f)); // slight delay for variation

        //check if player is in LOS
        RaycastHit whatIsHit;
        GetComponent<CapsuleCollider>().enabled = false;
        if(!Physics.Linecast((transform.position + new Vector3(0, 1.8f, 0)), (player.position + new Vector3(0, 1.8f, 0)), out whatIsHit))
            FireHook();
        else if(whatIsHit.transform.gameObject.tag == "Player")
            FireHook();
        GetComponent<CapsuleCollider>().enabled = true;
        
        //yield return new WaitForSeconds(hookCooldown);
    }

    private void FireHook()
    {
        enemy.SetDestination(transform.position);
        enemyAnimator.SetTrigger("Attack");
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks + animationDelay);
        GameObject hook = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hook.name = "EnemyHook";
        hook.transform.localScale = hookSize;
        hook.transform.position = hookFirePoint.position;
        hook.transform.rotation = Quaternion.LookRotation((player.position + new Vector3(0, 1, 0)) - hookFirePoint.position);

        Collider hookCol = hook.GetComponent<Collider>();

        // Ignore collision with enemy itself
        Collider enemyCol = GetComponent<Collider>();
        if (enemyCol && hookCol)
        {
            Physics.IgnoreCollision(hookCol, enemyCol, true);
        }

        Rigidbody hookRigidbody = hook.AddComponent<Rigidbody>();
        hookRigidbody.isKinematic = false;
        hookRigidbody.useGravity = false;

        // Hook projectile behaviour
        HookProjectile projectile = hook.AddComponent<HookProjectile>();
        projectile.Init(player, hookSpeed, hookRange, this);

        // make it visible in different color
        Renderer rend = hook.GetComponent<Renderer>();
        if (rend) rend.material.color = Color.red;
    }

    public void PullPlayer(Transform playerTransform)
    {
        StartCoroutine(PullPlayerRoutine(playerTransform));
    }

    private IEnumerator PullPlayerRoutine(Transform playerTransform)
    {
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null)
        {
            PlayerActions.isBeingHooked = true;
            float pullDuration = 0.5f;
            Vector3 startPos = playerTransform.position;
            Vector3 endPos = transform.position + Vector3.up * 1f;
            float elapsed = 0;

            while (elapsed < pullDuration)
            {
                elapsed += Time.deltaTime;
                Vector3 newPos = Vector3.Lerp(startPos, endPos, elapsed / pullDuration);

                playerTransform.position = newPos;
                yield return null;
            }
            PlayerActions.isBeingHooked = false;
        }
    }
}