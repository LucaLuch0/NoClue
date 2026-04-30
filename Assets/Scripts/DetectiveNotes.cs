using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Populates and displays the detective notes UI for a player,
/// showing all cards categorised by type and visually indicating
/// which have been ruled out with a strikethrough.
/// </summary>
public class DetectiveNotes : MonoBehaviour
{
    /// <summary>The player's notes dictionary, mapping each card to whether it has been ruled out.</summary>
    private Dictionary<Card, bool> notes;

    /// <summary>The grid UI container for weapon card entries.</summary>
    public GameObject weaponsGrid;

    /// <summary>The grid UI container for suspect card entries.</summary>
    public GameObject suspectsGrid;

    /// <summary>The grid UI container for room card entries.</summary>
    public GameObject roomsGrid;

    /// <summary>Prefab used to instantiate a text label for each card entry.</summary>
    public GameObject textPrefab;

    /// <summary>Maps each card to its corresponding text label in the UI.</summary>
    private Dictionary<Card, TextMeshProUGUI> cardTexts;

    /// <summary>
    /// Initialises the detective notes UI from the player's notes dictionary.
    /// Each card is placed in the appropriate grid and styled to indicate
    /// whether it has been ruled out (strikethrough) or not.
    /// </summary>
    /// <param name="notes">The player's notes, mapping cards to their ruled-out state.</param>
    public void setUp(Dictionary<Card, bool> notes)
    {
        this.notes = notes;
        cardTexts = new Dictionary<Card, TextMeshProUGUI>();

        foreach (Card c in notes.Keys)
        {
            // Determine which grid this card belongs to based on its type
            GameObject grid = null;
            if (c.cardType == Card.CardType.Weapon)  grid = weaponsGrid;
            if (c.cardType == Card.CardType.Room)    grid = roomsGrid;
            if (c.cardType == Card.CardType.Suspect) grid = suspectsGrid;

            // Instantiate a text label in the appropriate grid
            TextMeshProUGUI text = Instantiate(textPrefab, grid.transform).GetComponent<TextMeshProUGUI>();
            text.text = c.cardName;

            // Apply strikethrough if the card has been ruled out
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
