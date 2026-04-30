using TMPro;
using UnityEngine;

/// <summary>
/// Handles the UI shown to a player when they need to confirm
/// passing a revealed card back during a suggestion.
/// </summary>
public class PassBackPlayerUI : MonoBehaviour
{
    /// <summary>The text element used to display a message to the player.</summary>
    public TextMeshProUGUI text;

    /// <summary>Reference to the Suggestion that triggered this UI.</summary>
    private Suggestion suggestion;

    /// <summary>The card associated with this pass-back action.</summary>
    private Card card;

    /// <summary>
    /// Initialises the UI with the relevant suggestion, display message, and card.
    /// </summary>
    /// <param name="suggestion">The suggestion this UI belongs to.</param>
    /// <param name="text">The message to display to the player.</param>
    /// <param name="card">The card to be passed back upon confirmation.</param>
    public void setUp(Suggestion suggestion, string text, Card card)
    {
        this.suggestion = suggestion;
        this.text.text = text;
        this.card = card;
    }

    /// <summary>
    /// Called when the player confirms the action.
    /// Notifies the suggestion of the selected card and destroys this UI.
    /// </summary>
    public void select()
    {
        suggestion.cardSelected(card);
        DestroyImmediate(this.gameObject);
    }
}
