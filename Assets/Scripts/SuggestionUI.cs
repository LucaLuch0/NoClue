using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UI that allows a player to make a suggestion,
/// selecting a weapon and character while the room is fixed
/// based on the player's current location.
/// </summary>
public class SuggestionUI : MonoBehaviour
{
    /// <summary>Dropdown for selecting the suspected weapon.</summary>
    public TMP_Dropdown weaponDropdown;

    /// <summary>Dropdown for selecting the suspected character.</summary>
    public TMP_Dropdown characterDropdown;

    /// <summary>Text element displaying the room the suggestion is being made in.</summary>
    public TextMeshProUGUI roomText;

    /// <summary>The list of weapon cards available to suggest.</summary>
    private List<Card> weapons = new List<Card>();

    /// <summary>The list of character cards available to suggest.</summary>
    private List<Card> characters = new List<Card>();

    /// <summary>The room card corresponding to the player's current room.</summary>
    private Card room;

    /// <summary>Reference to the Suggestion that opened this UI.</summary>
    private Suggestion suggestion;

    /// <summary>
    /// Initialises the UI with the available cards and populates the dropdowns.
    /// The room is fixed and displayed as text rather than a selectable option.
    /// </summary>
    /// <param name="suggestion">The Suggestion instance to submit the result to.</param>
    /// <param name="weapons">The list of weapon cards to populate the weapon dropdown.</param>
    /// <param name="characters">The list of character cards to populate the character dropdown.</param>
    /// <param name="room">The room card for the current room, displayed as fixed text.</param>
    public void setUp(Suggestion suggestion, List<Card> weapons, List<Card> characters, Card room)
    {
        this.suggestion = suggestion;
        this.weapons = weapons;
        this.characters = characters;
        this.room = room;

        // Build name lists for the dropdowns
        List<string> weaponNames = new List<string>();
        List<string> characterNames = new List<string>();

        foreach (Card c in weapons)
        {
            weaponNames.Add(c.cardName);
        }
        foreach (Card c in characters)
        {
            characterNames.Add(c.cardName);
        }

        weaponDropdown.AddOptions(weaponNames);
        characterDropdown.AddOptions(characterNames);

        // Room is fixed — just display its name
        roomText.text = room.cardName;
    }

    /// <summary>
    /// Called when the player confirms their suggestion.
    /// Submits the selected weapon, character, and current room to the Suggestion,
    /// then destroys this UI.
    /// </summary>
    public void select()
    {
        suggestion.suggest(weapons[weaponDropdown.value], characters[characterDropdown.value], room);
        DestroyImmediate(this.gameObject);
    }
}
