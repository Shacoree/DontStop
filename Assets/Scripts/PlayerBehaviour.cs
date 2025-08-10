using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int Respawn = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandlePlayerDontStop();
        if (GameManager.gameManager.playerHealth.Health <= 0)
        {
            LoadMenu();
        }
        if (GameManager.gameManager.playerRage.Rage <= 0)
        {
            LoadMenu();
        }
        HandleFood();
    }
    private void HandlePlayerDontStop()
    {
        if (PlayerActions.PlayerStartedMoving)
        {
            if (!isPlayerMoving())
            {
                SceneManager.LoadScene(Respawn);
            }
        }
    }
    private bool isPlayerMoving()
    {
        if (PlayerActions.PlayerVelocity == Vector3.zero)
            return false;
        return true;
    }
    private void PlayerTakeDamage(float damage)
    {
        GameManager.gameManager.playerHealth.TakeDamage(damage);
    }    
    private void PlayerHeal(float amount)
    {
        GameManager.gameManager.playerHealth.Heal(amount);
    }
    private void LoadMenu()
    {
        SceneManager.LoadScene(Respawn);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void HandleFood()
    {
        if (GameManager.gameManager.playerFood.Food < GameManager.gameManager.playerFood.MinAllowedFood)
        {
            PlayerActions.lowFood = true;
            PlayerActions.playerDamage = 10.0f;
        }
        else if(GameManager.gameManager.playerFood.Food > GameManager.gameManager.playerFood.MaxAllowedFood)
        {
            PlayerActions.jumpImpulse = 1f;
            PlayerActions.highFood = true;
        }
        else
        {
            PlayerActions.jumpImpulse = 6f;
            PlayerActions.playerDamage = 30.0f;
            PlayerActions.lowFood = false;
            PlayerActions.highFood = false;
        }
    }
}
