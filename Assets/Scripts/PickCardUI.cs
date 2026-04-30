using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UI that allows a player to pick a card from a dropdown
/// when responding to a suggestion during the game.
/// </summary>
public class PickCardUI : MonoBehaviour
{
    /// <summary>Reference to the Suggestion that opened this UI, used to pass the selected card back.</summary>
    private Suggestion suggestion;

    /// <summary>The list of cards available for the player to choose from.</summary>
    private List<Card> cards = new List<Card>();

    /// <summary>The dropdown UI element populated with the available card names.</summary>
    public TMP_Dropdown dropdown;

    /// <summary>
    /// Initialises the UI with the relevant suggestion and the cards to display.
    /// Populates the dropdown with the card names.
    /// </summary>
    /// <param name="suggestion">The suggestion this UI is responding to.</param>
    /// <param name="cards">The cards the player can choose to reveal.</param>
    public void setUp(Suggestion suggestion, List<Card> cards)
    {
        this.suggestion = suggestion;
        this.cards = cards;

        // Populate the dropdown with the name of each available card
        List<string> cardNames = new List<string>();
        foreach (Card c in cards)
        {
            cardNames.Add(c.cardName);
        }
        dropdown.AddOptions(cardNames);
    }

    /// <summary>
    /// Called when the player confirms their card selection.
    /// Passes the chosen card back to the suggestion and destroys this UI.
    /// </summary>
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
