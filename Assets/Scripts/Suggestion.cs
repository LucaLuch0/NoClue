using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the suggestion mechanic, handling the full flow of a suggestion:
/// opening the suggestion UI, passing the device between players to reveal cards,
/// and updating the suggesting player's notes with any revealed card.
/// </summary>
public class Suggestion : MonoBehaviour
{
    /// <summary>Prefab for the suggestion selection UI.</summary>
    public GameObject suggestUIPrefab;

    /// <summary>Prefab for the UI shown when no other player holds any of the suggested cards.</summary>
    public GameObject noCardsUIPrefab;

    /// <summary>Prefab for the UI that lets a player pick which matching card to reveal.</summary>
    public GameObject pickCardsUIPrefab;

    /// <summary>Prefab for the UI that displays a revealed card name to the suggesting player.</summary>
    public GameObject displayCardUIPrefab;

    /// <summary>Prefab for the UI prompting the device to be passed to another player.</summary>
    public GameObject switchPlayerUIPrefab;

    /// <summary>Prefab for the UI prompting the device to be passed back to the suggesting player.</summary>
    public GameObject switchBackUIPrefab;

    // Active UI instance references
    private GameObject suggestUI, noCardsUI, pickCardsUI, displayCardUI, switchPlayerUI, switchBackUI;

    /// <summary>Reference to the TurnManager for accessing active players.</summary>
    public TurnManager turnManager;

    /// <summary>The player who is currently making the suggestion.</summary>
    private Player player;

    /// <summary>
    /// Registers this Suggestion instance with the PlayerManager on startup.
    /// </summary>
    void Start()
    {
        PlayerManager.suggestion = this;
    }

    /// <summary>
    /// Opens the suggestion UI for the given player in the given room.
    /// Finds the matching room card from the CardManager and passes it to the UI.
    /// </summary>
    /// <param name="player">The player making the suggestion.</param>
    /// <param name="room">The room the suggestion is being made in.</param>
    public void uiSetup(Player player, Room room)
    {
        this.player = player;

        suggestUI = Instantiate(suggestUIPrefab, transform);

        // Find the card that matches the current room by name
        Card roomCard = null;
        foreach (Card c in CardManager.rooms)
        {
            if (c.cardName.ToLower() == room.getName().ToLower())
            {
                roomCard = c;
                break;
            }
        }

        suggestUI.GetComponent<SuggestionUI>().setUp(this, CardManager.weapons, CardManager.characters, roomCard);
    }

    /// <summary>
    /// Instantiates the "no cards" UI, shown when no other player
    /// holds any of the suggested cards.
    /// </summary>
    public void noCards()
    {
        noCardsUI = Instantiate(noCardsUIPrefab, transform);
    }

    /// <summary>
    /// Called when another player has selected a card to reveal.
    /// Marks the card as ruled out in the suggesting player's notes,
    /// refreshes their notes UI, and displays the revealed card name.
    /// </summary>
    /// <param name="c">The card that was revealed.</param>
    public void cardSelected(Card c)
    {
        // Mark the revealed card as known in the suggesting player's notes
        player.notes[c] = true;
        turnManager.resetNotes();

        // Show the revealed card name to the suggesting player
        displayCardUI = Instantiate(displayCardUIPrefab, transform);
        displayCardUI.GetComponent<DisplayTextUI>().setUp(c.cardName);
    }

    /// <summary>
    /// Prompts the device to be passed back to the suggesting player,
    /// carrying the selected card with it.
    /// </summary>
    /// <param name="c">The card to pass back.</param>
    public void switchBack(Card c)
    {
        switchBackUI = Instantiate(switchBackUIPrefab, transform);
        switchBackUI.GetComponent<PassBackPlayerUI>().setUp(this, "Pass back", c);
    }

    /// <summary>
    /// Opens the card pick UI, allowing a player to choose which
    /// of their matching cards to reveal.
    /// </summary>
    /// <param name="cards">The cards the player can choose to reveal.</param>
    public void pickCards(List<Card> cards)
    {
        pickCardsUI = Instantiate(pickCardsUIPrefab, transform);
        pickCardsUI.GetComponent<PickCardUI>().setUp(this, cards);
    }

    /// <summary>
    /// Prompts the device to be passed to another player so they can
    /// privately select a card to reveal.
    /// </summary>
    /// <param name="pickPlayer">The player the device should be passed to.</param>
    /// <param name="cards">The matching cards that player can choose to reveal.</param>
    public void switchPlayer(Player pickPlayer, List<Card> cards)
    {
        switchPlayerUI = Instantiate(switchPlayerUIPrefab, transform);
        switchPlayerUI.GetComponent<PassToAnotherPlayerUI>().setUp(this, "Pass to player: " + pickPlayer.getName(), cards);
    }

    /// <summary>
    /// Processes a suggestion by checking each other player's hand for
    /// any of the suggested cards. If a matching player is found, the device
    /// is passed to them to reveal a card. If no one holds any matching card,
    /// the no-cards UI is shown.
    /// </summary>
    /// <param name="weapon">The suggested weapon card.</param>
    /// <param name="character">The suggested character card.</param>
    /// <param name="room">The suggested room card.</param>
    public void suggest(Card weapon, Card character, Card room)
    {
        foreach (Player p in turnManager.activePlayers)
        {
            if (p != player)
            {
                // Collect any cards in this player's hand that match the suggestion
                List<Card> correctCards = new List<Card>();
                foreach (Card c in p.hand)
                {
                    if (c == weapon)   correctCards.Add(c);
                    if (c == character) correctCards.Add(c);
                    if (c == room)     correctCards.Add(c);
                }

                // If this player has at least one matching card, pass the device to them
                if (correctCards.Count != 0)
                {
                    switchPlayer(p, correctCards);
                    return;
                }
            }
        }

        // No player holds any of the suggested cards
        noCards();
    }
}
