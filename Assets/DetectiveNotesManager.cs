using UnityEngine;
using TMPro;

public class DetectiveNotesManager : MonoBehaviour
{
    public static DetectiveNotesManager Instance;

    // VARIABLES
    public TMP_Dropdown suspectDropdown;
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown roomDropdown;
    public TMP_Text notesOutputText;

    private Player currentPlayer; // Tracks who has the phone

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // UI LOGIC FOR TURNMANAGER
    public void LoadPlayerNotes(Player playerHoldingPhone)
    {
        currentPlayer = playerHoldingPhone;

        // Show this specific player's saved notes on the screen
        notesOutputText.text = currentPlayer.personalNotesLog;
    }

    // BUTTON 
    public void MarkRuledOut()
    {
        if (currentPlayer == null) return;

        string suspect = suspectDropdown.options[suspectDropdown.value].text;
        string weapon = weaponDropdown.options[weaponDropdown.value].text;
        string room = roomDropdown.options[roomDropdown.value].text;
        string newEntry = "\n- " + suspect + " / " + weapon + " / " + room;
        // Save to memory and update
        currentPlayer.personalNotesLog += newEntry;
        notesOutputText.text = currentPlayer.personalNotesLog;
    }
}