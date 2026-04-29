using System;
using System.Collections.Generic;
using UnityEngine;

public class Suggestion : MonoBehaviour
{
    
    public GameObject suggestUIPrefab;
    public GameObject noCardsUIPrefab;
    public GameObject pickCardsUIPrefab;
    public GameObject displayCardUIPrefab;
    public GameObject switchPlayerUIPrefab;
    public GameObject switchBackUIPrefab;

    private GameObject suggestUI, noCardsUI, pickCardsUI, displayCardUI, switchPlayerUI, switchBackUI;
    
    public TurnManager turnManager;
    private Player player;
    
    void Start()
    {
        PlayerManager.suggestion = this;
    }

    public void uiSetup(Player player, Room room)
    {
        this.player = player;
        
        suggestUI =  Instantiate(suggestUIPrefab, transform);
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

    public void noCards()
    {
        noCardsUI = Instantiate(noCardsUIPrefab, transform);
    }

    public void cardSelected(Card c)
    {
        displayCardUI = Instantiate(displayCardUIPrefab, transform);
        displayCardUI.GetComponent<DisplayTextUI>().setUp(c.cardName);
    }

    public void switchBack(Card c)
    {
        switchBackUI = Instantiate(switchBackUIPrefab, transform);
        switchBackUI.GetComponent<PassBackPlayerUI>().setUp(this, "Pass back", c);
    }
    public void pickCards(List<Card> cards)
    {
        pickCardsUI = Instantiate(pickCardsUIPrefab, transform);
        pickCardsUI.GetComponent<PickCardUI>().setUp(this, cards);
    }

    public void switchPlayer(Player pickPlayer, List<Card> cards)
    {
        switchPlayerUI = Instantiate(switchPlayerUIPrefab, transform);
        switchPlayerUI.GetComponent<PassToAnotherPlayerUI>().setUp(this, "Pass to player: " + pickPlayer.getName(), cards);
    }
    public void suggest(Card weapon, Card character, Card room)
    {
        foreach (Player p in turnManager.activePlayers)
        {
            if (p != player)
            {
                List<Card> correctCards = new List<Card>();
                foreach (Card c in p.hand)
                {
                    if (c == weapon)
                    {
                        correctCards.Add(c);
                    }
                    if (c == character)
                    {
                        correctCards.Add(c);
                    }
                    if (c == room)
                    {
                        correctCards.Add(c);
                    }
                }

                if (correctCards.Count != 0)
                {
                    switchPlayer(p, correctCards);
                    return;
                }
            }
        }
        
        // no cards found
        noCards();
    }
}
