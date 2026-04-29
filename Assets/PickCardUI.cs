using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickCardUI : MonoBehaviour
{

    private Suggestion suggestion;
    private List<Card> cards = new List<Card>();
    public TMP_Dropdown dropdown;

    public void setUp(Suggestion suggestion, List<Card> cards)
    {
        this.suggestion = suggestion;
        this.cards = cards;

        List<string> cardNames = new List<string>();
        foreach (Card c in cards)
        {
            cardNames.Add(c.cardName);
        }
        dropdown.AddOptions(cardNames);
    }

    public void select()
    {
        suggestion.switchBack(cards[dropdown.value]);
        DestroyImmediate(this.gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
