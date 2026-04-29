using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    // VARIABLES
    public List<Player> activePlayers = new List<Player>();
    private int currentPlayerIndex = 0;
    public TextMeshProUGUI playerNameText;
    public CurvedHandLayout curvedHandLayout;
    public TextMeshProUGUI playerMovesText;

    private void Update()
    {
        playerMovesText.text = "Moves Left: " + currentPlayer().moves;
    }

    public Player currentPlayer()
    {
        return activePlayers[currentPlayerIndex];
    }

    // OFFICIAL CLUEDO TURN ORDER
    private readonly List<string> officialTurnOrder = new List<string>
    {
        "Miss Scarlett",
        "Colonel Mustard",
        "Mrs. White",
        "Mr. Green",
        "Mrs. Peacock",
        "Professor Plum"
    };

    // INITIALIZATION
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void InitializeTurnOrder()
    {
        if (activePlayers.Count == 0) return;
        activePlayers = activePlayers.OrderBy(p => officialTurnOrder.IndexOf(p.getCharacter())).ToList();
        currentPlayerIndex = 0;
        StartTurn();
    }

    // TURN LOGIC
    public void StartTurn()
    {
        Player currentPlayer = activePlayers[currentPlayerIndex];
        currentPlayer.setMoves();
        playerNameText.text = currentPlayer.getName() + " : " + currentPlayer.getCharacter();
        curvedHandLayout.setup(currentPlayer.hand);
        Debug.Log("--- TURN STARTED: " + currentPlayer.getName() + " playing as " + currentPlayer.getCharacter() + " ---");
        //DetectiveNotesManager.Instance.LoadPlayerNotes(currentPlayer);
    }

    public void EndCurrentTurn()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= activePlayers.Count)
        {
            currentPlayerIndex = 0;
        }
        Player nextPlayer = activePlayers[currentPlayerIndex];
        TriggerIntermissionScreen(nextPlayer);
    }

    // UI LOGIC
    private void TriggerIntermissionScreen(Player nextPlayer)
    {
        // Call the UI manager to show the screen
        IntermissionUI.Instance.ShowIntermission(nextPlayer);
    }
}