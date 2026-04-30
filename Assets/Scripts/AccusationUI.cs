using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UI that allows a player to make a final accusation,
/// selecting a weapon, character, and room from dropdown menus.
/// </summary>
public class AccusationUI : MonoBehaviour
{
    /// <summary>Dropdown for selecting the accused weapon.</summary>
    public TMP_Dropdown weaponDropdown;

    /// <summary>Dropdown for selecting the accused character.</summary>
    public TMP_Dropdown characterDropdown;

    /// <summary>Dropdown for selecting the accused room.</summary>
    public TMP_Dropdown roomDropdown;

    /// <summary>The list of weapon cards available to accuse.</summary>
    private List<Card> weapons = new List<Card>();

    /// <summary>The list of character cards available to accuse.</summary>
    private List<Card> characters = new List<Card>();

    /// <summary>The list of room cards available to accuse.</summary>
    private List<Card> rooms = new List<Card>();

    /// <summary>Reference to the Accusation that opened this UI.</summary>
    private Accusation accusation;

    /// <summary>
    /// Initialises the UI with all available cards and populates the dropdowns.
    /// </summary>
    /// <param name="accusation">The Accusation instance to submit the result to.</param>
    /// <param name="weapons">The list of weapon cards to populate the weapon dropdown.</param>
    /// <param name="characters">The list of character cards to populate the character dropdown.</param>
    /// <param name="rooms">The list of room cards to populate the room dropdown.</param>
    public void setUp(Accusation accusation, List<Card> weapons, List<Card> characters, List<Card> rooms)
    {
        this.accusation = accusation;
        this.weapons = weapons;
        this.characters = characters;
        this.rooms = rooms;

        // Build name lists for each dropdown
        List<string> weaponNames = new List<string>();
        List<string> characterNames = new List<string>();
        List<string> roomNames = new List<string>();

        foreach (Card c in weapons)    weaponNames.Add(c.cardName);
        foreach (Card c in characters) characterNames.Add(c.cardName);
        foreach (Card c in rooms)      roomNames.Add(c.cardName);

        weaponDropdown.AddOptions(weaponNames);
        characterDropdown.AddOptions(characterNames);
        roomDropdown.AddOptions(roomNames);
    }

    /// <summary>
    /// Called when the player confirms their accusation.
    /// Submits the selected weapon, character, and room to the Accusation,
    /// then destroys this UI.
    /// </summary>
    public void select()
    {
        accusation.accuse(weapons[weaponDropdown.value], characters[characterDropdown.value], rooms[roomDropdown.value]);
        DestroyImmediate(this.gameObject);
    }
}