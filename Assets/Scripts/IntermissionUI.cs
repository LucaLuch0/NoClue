using UnityEngine;
using TMPro; 

public class IntermissionUI : MonoBehaviour
{
    public static IntermissionUI Instance;

    // VARIABLES
    [SerializeField] private GameObject intermissionPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    // INITIALIZATION
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Hide panel
        intermissionPanel.SetActive(false);
    }

    // UI LOGIC
    public void ShowIntermission(Player nextPlayer)
    {
        // Activate Panel
        intermissionPanel.SetActive(true);

        // Update text with player info
        promptText.text = "Turn Over.\nPass device to:\n" + nextPlayer.getName() + " (" + nextPlayer.getCharacter() + ")";
    }

    // BUTTON
    public void OnReadyButtonClicked()
    {
        // Hide the panel
        intermissionPanel.SetActive(false);

        // Tell the Turn Manager to start the turn
        TurnManager.Instance.StartTurn();
    }
}