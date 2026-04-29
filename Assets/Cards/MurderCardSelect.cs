using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class MurderCardSelect : MonoBehaviour
{
    [Header("The Master Deck")]
    public List<Card> allCardsDeck = new List<Card>();

    [Header("Players")]
    public List<Player> players = new List<Player>();

    [Header("The Murder Envelope")]
    public Card murderRoom;
    public Card murderSuspect;
    public Card murderWeapon;
    public void setUp()
    {
        Debug.Log("PLAYER CARDS: SET UP STARTED");
        DrawMurderCards();
        DealRemainingCards();
    }

    public void DrawMurderCards()
    {
        List<Card> roomCards = allCardsDeck.Where(c => c.cardType == Card.CardType.Room).ToList();
        List<Card> suspectCards = allCardsDeck.Where(c => c.cardType == Card.CardType.Suspect).ToList();
        List<Card> weaponCards = allCardsDeck.Where(c => c.cardType == Card.CardType.Weapon).ToList();

        CardManager.rooms = roomCards;
        CardManager.characters = suspectCards;
        CardManager.weapons = weaponCards;

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

    public void DealRemainingCards()
    {
        this.players = PlayerManager.getPlayers();

        allCardsDeck = allCardsDeck.OrderBy(card => Random.value).ToList();
        int currentPlayerIndex = 0;
        
        foreach (Card card in allCardsDeck)
        {
            players[currentPlayerIndex].hand.Add(card);
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }

        allCardsDeck.Clear();
        Debug.Log("Cards Dealt");
    }

}
