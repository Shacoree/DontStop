using Unity.VisualScripting;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] public AudioSource audioSourceSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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

        if (!enemy.ISPlayerInAttackRange()) return;
        
        GameManager.gameManager.playerHealth.TakeDamage(enemy.enemyDamage);
    }
}
