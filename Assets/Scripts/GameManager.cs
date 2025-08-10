using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] public float rageLossMultiplier = 1.0f;
    [SerializeField] public float foodLossMultiplier = 0.5f;
    public static GameManager gameManager { get; private set; }

    public UnitHealth playerHealth = new UnitHealth(100, 100);
    public PlayerRage playerRage = new PlayerRage(100, 100);
    public PlayerFood playerFood = new PlayerFood(50, 100);
    public int playerKills = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        playerKills = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 144;

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
    // Update is called once per frame
    void FixedUpdate()
    {
        playerRage.LoseRage(Time.fixedDeltaTime * rageLossMultiplier);
        playerFood.LoseFood(Time.fixedDeltaTime * foodLossMultiplier);
    }


}
