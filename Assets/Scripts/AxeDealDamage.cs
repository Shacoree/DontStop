using Unity.VisualScripting;
using UnityEngine;

public class AxeDealDamage : MonoBehaviour
{
    private Collider weaponCollider;
    
    void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || other.IsDestroyed()) return;

        if (other.CompareTag("Enemy"))
        {
            if (!other.TryGetComponent<EnemyAi>(out var enemy)) return;
            if (enemy == null || enemy.IsDestroyed()) return;
            
            enemy.OnTakeDamage(PlayerActions.playerDamage);
            weaponCollider.enabled = false;
        }
    }
    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }
    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}
