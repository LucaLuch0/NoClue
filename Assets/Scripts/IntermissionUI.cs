using UnityEngine;
using TMPro;

/// <summary>
/// Manages the intermission screen shown between turns,
/// prompting the next player to take the device.
/// Implemented as a Singleton for global access.
/// </summary>
public class IntermissionUI : MonoBehaviour
{
    /// <summary>Singleton instance of the IntermissionUI.</summary>
    public static IntermissionUI Instance;

    // VARIABLES
    /// <summary>The panel GameObject shown during the intermission.</summary>
    [SerializeField] private GameObject intermissionPanel;

    /// <summary>The text element displaying the pass-device prompt and next player's info.</summary>
    [SerializeField] private TextMeshProUGUI promptText;

    // INITIALIZATION
    /// <summary>
    /// Ensures only one instance of IntermissionUI exists
    /// and hides the intermission panel on startup.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Hide panel on startup until a turn ends
        intermissionPanel.SetActive(false);
    }

    // UI LOGIC
    /// <summary>
    /// Displays the intermission panel and prompts the next player
    /// to take the device, showing their name and character.
    /// </summary>
    /// <param name="nextPlayer">The player whose turn is coming up next.</param>
    public void ShowIntermission(Player nextPlayer)
    {
        intermissionPanel.SetActive(true);
        promptText.text = "Turn Over.\nPass device to:\n" + nextPlayer.getName() + " (" + nextPlayer.getCharacter() + ")";
    }

    // BUTTON
    /// <summary>
    /// Called when the ready button is clicked by the next player.
    /// Hides the intermission panel and starts the next player's turn.
    /// </summary>
    public void OnReadyButtonClicked()
    {
        intermissionPanel.SetActive(false);
        TurnManager.Instance.StartTurn();
    }
}