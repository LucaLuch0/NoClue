using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SuggestionUIManager : MonoBehaviour
{
    public GameObject suggestionPanel, accusationPanel, cardRevealPanel, resultPanel;
    public TMP_Dropdown suspectDropdown, weaponDropdown, accuseSuspectDropdown, accuseWeaponDropdown, accuseRoomDropdown;
    public TextMeshProUGUI roomLabel, revealInstructionText, resultText;
    public Button suggestButton, cancelButton, confirmAccuseButton, cancelAccuseButton, cardButton1, cardButton2, cardButton3, noCardButton;
    public DetectiveNotesManager notesManager;

    private string currentPlayer = "";

    private List<string> suspects = new List<string> { "Col. Mustard", "Prof. Plum", "Rev. Green", "Mrs. Peacock", "Miss Scarlett", "Mrs. White" };
    private List<string> weapons = new List<string> { "Dagger", "Candlestick", "Revolver", "Rope", "Lead Piping", "Spanner" };
    private List<string> rooms = new List<string> { "Hall", "Lounge", "Dining Room", "Kitchen", "Ball Room", "Conservatory", "Billiard Room", "Library", "Study" };

    void Start()
    {
        Fill(suspectDropdown, suspects); Fill(weaponDropdown, weapons);
        Fill(accuseSuspectDropdown, suspects); Fill(accuseWeaponDropdown, weapons); Fill(accuseRoomDropdown, rooms);

        suggestButton.onClick.AddListener(OnSuggest);
        cancelButton.onClick.AddListener(() => suggestionPanel.SetActive(false));
        confirmAccuseButton.onClick.AddListener(OnAccuse);
        cancelAccuseButton.onClick.AddListener(() => accusationPanel.SetActive(false));
        noCardButton.onClick.AddListener(() => cardRevealPanel.SetActive(false));

        suggestionPanel.SetActive(false);
        accusationPanel.SetActive(false);
        cardRevealPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void OpenSuggestion(string player, string room)
    {
        currentPlayer = player;
        roomLabel.text = "Room: " + room;
        suggestionPanel.SetActive(true);
    }

    void OnSuggest()
    {
        suggestionPanel.SetActive(false);
        ShowReveal("Next Player", new List<string> { suspects[suspectDropdown.value] });
    }

    public void ShowReveal(string responder, List<string> cards)
    {
        cardRevealPanel.SetActive(true);
        Button[] btns = { cardButton1, cardButton2, cardButton3 };
        foreach (Button b in btns) b.gameObject.SetActive(false);
        noCardButton.gameObject.SetActive(cards.Count == 0);
        revealInstructionText.text = cards.Count == 0 ? responder + " has no matching cards." : responder + ": pick a card";

        for (int i = 0; i < cards.Count && i < 3; i++)
        {
            string card = cards[i];
            btns[i].gameObject.SetActive(true);
            btns[i].GetComponentInChildren<TextMeshProUGUI>().text = card;
            btns[i].onClick.RemoveAllListeners();
            btns[i].onClick.AddListener(() => OnCardPicked(card));
        }
    }

    void OnCardPicked(string card)
    {
        cardRevealPanel.SetActive(false);
        ShowResult(currentPlayer + " was shown: " + card);
        if (notesManager != null) notesManager.LogRevealedCard(card);
    }

    void OnAccuse()
    {
        accusationPanel.SetActive(false);
        ShowResult("Accusation: " + suspects[accuseSuspectDropdown.value] + ", " + weapons[accuseWeaponDropdown.value] + ", " + rooms[accuseRoomDropdown.value]);
    }

    void ShowResult(string msg) { resultText.text = msg; resultPanel.SetActive(true); Invoke("HideResult", 3f); }
    void HideResult() { resultPanel.SetActive(false); }

    void Fill(TMP_Dropdown d, List<string> options) { d.ClearOptions(); d.AddOptions(options); }
}