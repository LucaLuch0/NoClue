using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

/// <summary>
/// Handles the setup of the murder envelope and card dealing.
/// Randomly selects one room, suspect, and weapon card as the murder solution,
/// then deals the remaining cards evenly among all players.
/// </summary>
public class MurderCardSelect : MonoBehaviour
{
    [Header("The Master Deck")]
    /// <summary>The full deck of all cards in the game, used as the source for dealing.</summary>
    public List<Card> allCardsDeck = new List<Card>();

    [Header("Players")]
    /// <summary>The list of players to deal cards to.</summary>
    public List<Player> players = new List<Player>();

    [Header("The Murder Envelope")]
    /// <summary>The randomly selected murder room card.</summary>
    public Card murderRoom;

    /// <summary>The randomly selected murder suspect card.</summary>
    public Card murderSuspect;

    /// <summary>The randomly selected murder weapon card.</summary>
    public Card murderWeapon;

    /// <summary>
    /// Begins the card setup process by drawing the murder cards
    /// and then dealing the remainder to players.
    /// </summary>
    public void setUp()
    {
        Debug.Log("PLAYER CARDS: SET UP STARTED");
        DrawMurderCards();
        DealRemainingCards();
    }

    /// <summary>
    /// Separates the deck by card type, registers each category with the CardManager,
    /// then randomly selects one card of each type as the murder solution,
    /// removing those cards from the deck.
    /// </summary>
    public void DrawMurderCards()
    {
        List<Card> roomCards = allCardsDeck.Where(c => c.cardType == Card.CardType.Room).ToList();
        List<Card> suspectCards = allCardsDeck.Where(c => c.cardType == Card.CardType.Suspect).ToList();
        List<Card> weaponCards = allCardsDeck.Where(c => c.cardType == Card.CardType.Weapon).ToList();

        // Register all cards with the CardManager for global access
        CardManager.rooms = roomCards;
        CardManager.characters = suspectCards;
        CardManager.weapons = weaponCards;

        // Randomly pick and remove one card of each type as the murder solution
        if (roomCards.Count > 0)
        {
            murderRoom = roomCards[Random.Range(0, roomCards.Count)];
            allCardsDeck.Remove(murderRoom);
        }

        if (suspectCards.Count > 0)
        {
            murderSuspect = suspectCards[Random.Range(0, suspectCards.Count)];
            allCardsDeck.Remove(murderSuspect);
        }

        if (weaponCards.Count > 0)
        {
            murderWeapon = weaponCards[Random.Range(0, weaponCards.Count)];
            allCardsDeck.Remove(murderWeapon);
        }

        if (murderSuspect != null && murderRoom != null && murderWeapon != null)
        {
            Debug.Log($"It was {murderSuspect.cardName} in the {murderRoom.cardName} with the {murderWeapon.cardName}");
        }
        else
        {
            Debug.LogError("Deck Empty");
        }
    }

    /// <summary>
    /// Shuffles the remaining deck and deals cards evenly to all players
    /// in a round-robin fashion, then clears the deck.
    /// </summary>
    public void DealRemainingCards()
    {
        this.players = PlayerManager.getPlayers();

        // Shuffle the remaining deck randomly
        allCardsDeck = allCardsDeck.OrderBy(card => Random.value).ToList();

        int currentPlayerIndex = 0;

        // Deal each card to the next player in sequence, wrapping around
        foreach (Card card in allCardsDeck)
        {
            players[currentPlayerIndex].hand.Add(card);
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }

        allCardsDeck.Clear();
        Debug.Log("Cards Dealt");
    }
}
