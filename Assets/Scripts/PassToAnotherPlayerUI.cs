using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles the UI shown when the device needs to be passed to another player
/// so they can privately view and respond to a suggestion.
/// </summary>
public class PassToAnotherPlayerUI : MonoBehaviour
{
    /// <summary>The text element used to display a message to the player.</summary>
    public TextMeshProUGUI text;

    /// <summary>Reference to the Suggestion that triggered this UI.</summary>
    private Suggestion suggestion;

    /// <summary>The cards relevant to this suggestion that the next player must respond to.</summary>
    private List<Card> cards;

    /// <summary>
    /// Initialises the UI with the relevant suggestion, display message, and cards.
    /// </summary>
    /// <param name="suggestion">The suggestion this UI belongs to.</param>
    /// <param name="text">The message to display, typically instructing who to pass the device to.</param>
    /// <param name="cards">The cards the next player will choose from.</param>
    public void setUp(Suggestion suggestion, string text, List<Card> cards)
    {
        this.suggestion = suggestion;
        this.text.text = text;
        this.cards = cards;
    }

    /// <summary>
    /// Called when the player confirms the pass.
    /// Forwards the cards to the suggestion for the next player to pick from,
    /// then destroys this UI.
    /// </summary>
    public void select()
    {
        suggestion.pickCards(cards);
        DestroyImmediate(this.gameObject);
    }
}
