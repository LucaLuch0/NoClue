using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetectiveNotes : MonoBehaviour
{

    private Dictionary<Card, bool> notes;

    public GameObject weaponsGrid;
    public GameObject suspectsGrid;
    public GameObject roomsGrid;

    public GameObject textPrefab;
    private Dictionary<Card, TextMeshProUGUI> cardTexts;
    
    
    
    public void setUp(Dictionary<Card, bool> notes)
    {
        this.notes = notes;
        cardTexts = new Dictionary<Card, TextMeshProUGUI>();

        foreach (Card c in notes.Keys)
        {
            GameObject grid = null;
            if (c.cardType == Card.CardType.Weapon)
            {
                grid = weaponsGrid;
            }
            if (c.cardType == Card.CardType.Room)
            {
                grid = roomsGrid;
            }
            if (c.cardType == Card.CardType.Suspect)
            {
                grid = suspectsGrid;
            }

            TextMeshProUGUI text = Instantiate(textPrefab, grid.transform).GetComponent<TextMeshProUGUI>();
            text.text = c.cardName;
            if (notes[c])
            {
                text.fontStyle = FontStyles.Strikethrough | FontStyles.UpperCase;
            }
            else
            {
                text.fontStyle = FontStyles.UpperCase;

            }

            cardTexts.Add(c, text);
        }
    }
}
