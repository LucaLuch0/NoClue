using UnityEngine;
using TMPro;

public class DetectiveNotesManager : MonoBehaviour
{
    public TMP_Dropdown suspectDropdown;
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown roomDropdown;
    public TMP_Text notesOutputText;

    public void MarkRuledOut()
    {
        string suspect = suspectDropdown.options[suspectDropdown.value].text;
        string weapon = weaponDropdown.options[weaponDropdown.value].text;
        string room = roomDropdown.options[roomDropdown.value].text;

        notesOutputText.text =
            "Ruled Out:\n\n" +
            "Suspect: " + suspect + "\n" +
            "Weapon: " + weapon + "\n" +
            "Room: " + room;
    }

    public void LogRevealedCard(string cardName)
    {
        notesOutputText.text += "\nRevealed: " + cardName;
    }
}