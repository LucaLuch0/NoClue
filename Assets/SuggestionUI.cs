using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionUI : MonoBehaviour
{

    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown characterDropdown;
    public TextMeshProUGUI roomText;
    
    private List<Card> weapons = new List<Card>();
    private List<Card> characters = new List<Card>();
    private Card room;
    
    private Suggestion suggestion;
    
    public void setUp(Suggestion suggestion, List<Card> weapons,  List<Card> characters, Card room)
    {
        this.suggestion = suggestion;
        this.weapons = weapons;
        this.characters = characters;
        this.room = room;
        
        
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
        roomText.text = room.cardName;
        
    }

    public void select()
    {
        suggestion.suggest(weapons[weaponDropdown.value], characters[characterDropdown.value], room);
        DestroyImmediate(this.gameObject);

    }
}
