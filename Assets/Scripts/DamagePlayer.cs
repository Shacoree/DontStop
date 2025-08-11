using Unity.VisualScripting;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] public AudioSource audioSourceSFX;

    private void EnemyDealDamageToPlayer()
    {
        var enemy = this.GameObject().GetComponent<EnemyAi>();

        if (enemy.groundSmashAudio != null)
        {
            audioSourceSFX = GameObject.Find("AudioSourceSFX").GetComponent<AudioSource>();
            audioSourceSFX.clip = enemy.groundSmashAudio;
            audioSourceSFX.transform.position = enemy.transform.position;
            audioSourceSFX.Play();
        }

        if (!enemy.ISPlayerInAttackRange(enemy.damageRange)) return;
        
        GameManager.gameManager.playerHealth.TakeDamage(enemy.enemyDamage);
    }
}
