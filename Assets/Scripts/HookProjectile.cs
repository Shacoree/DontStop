using UnityEngine;

public class HookProjectile : MonoBehaviour
{
    private Transform targetPlayer;
    private float speed;
    private float maxDistance;
    private Vector3 startPosition;
    private HookerEnemyAi owner;

    public void Init(Transform player, float spd, float range, HookerEnemyAi ownerEnemy)
    {
        targetPlayer = player;
        speed = spd;
        maxDistance = range;
        startPosition = transform.position;
        owner = ownerEnemy;
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player") || other.CompareTag("Axe"))
        {
            owner.PullPlayer(targetPlayer);
        }
        Destroy(gameObject);
    }
}