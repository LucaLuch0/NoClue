using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SuggestionUIManager : MonoBehaviour
{
    public GameObject suggestionPanel, accusationPanel, cardRevealPanel, resultPanel;
    public TMP_Dropdown suspectDropdown, weaponDropdown, accuseSuspectDropdown, accuseWeaponDropdown, accuseRoomDropdown, pickCardDropdown;
    public TextMeshProUGUI roomLabel, revealInstructionText, resultText;
    public Button suggestButton, cancelButton, confirmAccuseButton, cancelAccuseButton, selectButton, noCardsButton;
    public DetectiveNotesManager notesManager;

    private string currentPlayer = "";
    private Player player;
    
    public TurnManager turnManager;
    
    List<String> suspectNames = new List<String>();
    List<String> weaponNames = new List<String>();
    List<String> roomNames = new List<String>();
    
    List<Card> correctCards = new List<Card>();
    
    public List<Card> suspectCards = new List<Card>();
    public List<Card> weaponCards = new List<Card>();
    public List<Card> roomCards = new List<Card>();
    
    
    void Start()
    {
        foreach (Card c in suspectCards)
        {
            suspectNames.Add(c.name);
        }
        foreach (Card c in weaponCards)
        {
            weaponNames.Add(c.name);
        }
        foreach (Card c in roomCards)
        {
            roomNames.Add(c.name);
        }
        
        Fill(suspectDropdown, suspectNames); Fill(weaponDropdown, weaponNames);
        Fill(accuseSuspectDropdown, suspectNames); Fill(accuseWeaponDropdown, weaponNames); Fill(accuseRoomDropdown, roomNames);

        suggestButton.onClick.AddListener(OnSuggest);
        cancelButton.onClick.AddListener(() => suggestionPanel.SetActive(false));
        confirmAccuseButton.onClick.AddListener(OnAccuse);
        cancelAccuseButton.onClick.AddListener(() => accusationPanel.SetActive(false));
        selectButton.onClick.AddListener(() => cardRevealPanel.SetActive(false));
        
        noCardsButton.onClick.AddListener(() => cardRevealPanel.SetActive(false));
        noCardsButton.onClick.AddListener(() => noCardsButton.gameObject.SetActive(false));


        suggestionPanel.SetActive(false);
        accusationPanel.SetActive(false);
        cardRevealPanel.SetActive(false);
        resultPanel.SetActive(false);

        PlayerManager.suggestionUIManager = this;
    }

    public void OpenSuggestion(Player player, string room)
    {
        this.player  = player;
        roomLabel.text = room;
        suggestionPanel.SetActive(true);
    }

    void OnSuggest()
    {
        suggestionPanel.SetActive(false);
        List<Card> selectedCards = new List<Card>();
        
        foreach (Card c in roomCards)
        {
            if (c.cardName.ToLower() == roomLabel.text.ToLower())
            {
                selectedCards.Add(c);
            }
        }
        selectedCards.Add(suspectCards[suspectDropdown.value]);
        selectedCards.Add(weaponCards[weaponDropdown.value]);
        
        Debug.Log("Selected cards count: " + selectedCards.Count);
        
        ShowReveal("Next Player", selectedCards);
    }

    public void ShowReveal(string responder, List<Card> cards)
    {
        pickCardDropdown.gameObject.SetActive(false);
        revealInstructionText.gameObject.SetActive(false);
        selectButton.gameObject.SetActive(false);
        cardRevealPanel.SetActive(true);
        
        foreach (Player p in turnManager.activePlayers)
        {
            if (p == player)
            {
                continue;
            }
            
            foreach (Card c in p.hand)
            {
                foreach (Card cr in cards)
                {
                    if (c == cr)
                    {
                        correctCards.Add(c);
                        Debug.Log("Correct Cards : " + cr.cardName);
                    }
                }
            }

            if (correctCards.Count != 0)
            {
                List<String> correctCardNames = new List<String>();
                foreach (Card c in correctCards)
                {
                    correctCardNames.Add(c.name);
                }

                pickCardDropdown.AddOptions(correctCardNames);
                selectButton.onClick.AddListener(OnCardPicked);
                pickCardDropdown.gameObject.SetActive(true);
                revealInstructionText.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(true);

                return;

            }
        }
        
        noCardsButton.gameObject.SetActive(true);
    }
    
    void OnCardPicked()
    {
        Card picked =  correctCards[pickCardDropdown.value];
        cardRevealPanel.SetActive(false);
        ShowResult(currentPlayer + " was shown: " + picked.cardName);
        if (notesManager != null) notesManager.LogRevealedCard(picked.cardName);
        
        correctCards.Clear();
        pickCardDropdown.ClearOptions();
        selectButton.onClick.RemoveListener(OnCardPicked);
        pickCardDropdown.gameObject.SetActive(false);
        revealInstructionText.gameObject.SetActive(false);
        selectButton.gameObject.SetActive(false);
        
    }

    void OnAccuse()
    {
        accusationPanel.SetActive(false);
        ShowResult("Accusation: " + suspectNames[accuseSuspectDropdown.value] + ", " + weaponNames[accuseWeaponDropdown.value] + ", " + roomNames[accuseRoomDropdown.value]);
    }

    void ShowResult(string msg) { resultText.text = msg; resultPanel.SetActive(true); Invoke("HideResult", 3f); }
    void HideResult() { resultPanel.SetActive(false); }

    void Fill(TMP_Dropdown d, List<string> options) { d.ClearOptions(); d.AddOptions(options); }
}