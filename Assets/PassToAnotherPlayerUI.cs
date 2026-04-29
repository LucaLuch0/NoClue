using System;
using System.Collections.Generic;
using Mono.Cecil;
using TMPro;
using UnityEngine;

public class PassToAnotherPlayerUI : MonoBehaviour
{

    public TextMeshProUGUI text;
    private Suggestion suggestion;
    
    private List<Card> cards;
    
    public void setUp(Suggestion suggestion, string text, List<Card> cards)
    {
        this.suggestion = suggestion; 
        this.text.text = text;
        this.cards = cards;
    }

    public void select()
    {
        suggestion.pickCards(cards);
        DestroyImmediate(this.gameObject);
    }


}
