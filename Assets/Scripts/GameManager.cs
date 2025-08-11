using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    [SerializeField] public float rageLossMultiplier = 1.0f;
    [SerializeField] public float foodLossMultiplier = 0.5f;

    public UnitHealth playerHealth = new UnitHealth(100, 100);
    public PlayerRage playerRage = new PlayerRage(100, 100);
    public PlayerFood playerFood = new PlayerFood(50, 100);

    public int playerKills = 0;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 144;

        playerKills = 0;

        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else 
        {
            gameManager = this;
        }
    }
    private void Start()
    {
        ManageMixer.audioManager.SetMasterVolume(PlayerPrefs.GetFloat(SettingsMenu.prefsMasterVolume));
        ManageMixer.audioManager.SetMusicVolume(PlayerPrefs.GetFloat(SettingsMenu.prefsMusicVolume));
        ManageMixer.audioManager.SetSFXVolume(PlayerPrefs.GetFloat(SettingsMenu.prefsSFXVolume));
    }
    void FixedUpdate()
    {
        playerRage.LoseRage(Time.fixedDeltaTime * rageLossMultiplier);
        playerFood.LoseFood(Time.fixedDeltaTime * foodLossMultiplier);
    }
}
