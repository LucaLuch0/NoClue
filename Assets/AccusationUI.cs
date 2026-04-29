using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AccusationUI : MonoBehaviour
{

    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown characterDropdown;
    public TMP_Dropdown roomDropdown;
    
    private List<Card> weapons = new List<Card>();
    private List<Card> characters = new List<Card>();
    private List<Card> rooms = new List<Card>();
    
    private Accusation accusation;
    
    public void setUp(Accusation accusation, List<Card> weapons,  List<Card> characters, List<Card> rooms)
    {
        this.accusation = accusation;
        this.weapons = weapons;
        this.characters = characters;
        this.rooms = rooms;
        
        
        List<string> weaponNames = new List<string>();
        List<string> characterNames = new List<string>();
        List<string> roomNames = new List<string>();


        foreach (Card c in weapons)
        {
            weaponNames.Add(c.cardName);
        }

        foreach (Card c in characters)
        {
            characterNames.Add(c.cardName);
        }
        foreach (Card c in rooms)
        {
            roomNames.Add(c.cardName);
        }
        
        weaponDropdown.AddOptions(weaponNames);
        characterDropdown.AddOptions(characterNames);
        roomDropdown.AddOptions(roomNames);
        
    }

    public void select()
    {
        accusation.accuse(weapons[weaponDropdown.value], characters[characterDropdown.value], rooms[roomDropdown.value]);
        DestroyImmediate(this.gameObject);

    }
}