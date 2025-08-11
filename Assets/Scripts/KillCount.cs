using TMPro;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = GameManager.gameManager.playerKills.ToString();
    }
}
