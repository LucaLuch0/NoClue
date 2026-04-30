using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the turn order and turn logic for the Cluedo game.
/// Implemented as a Singleton to allow global access.
/// </summary>
public class TurnManager : MonoBehaviour
{
    /// <summary>Singleton instance of the TurnManager.</summary>
    public static TurnManager Instance;

    // VARIABLES
    /// <summary>List of all players currently active in the game.</summary>
    public List<Player> activePlayers = new List<Player>();

    /// <summary>Index of the player whose turn it currently is.</summary>
    private int currentPlayerIndex = 0;

    /// <summary>UI text element displaying the current player's name and character.</summary>
    public TextMeshProUGUI playerNameText;

    /// <summary>Reference to the curved hand layout used to display the current player's cards.</summary>
    public CurvedHandLayout curvedHandLayout;

    /// <summary>UI text element displaying the current player's remaining moves.</summary>
    public TextMeshProUGUI playerMovesText;

    /// <summary>Transform used as the parent for the instantiated notes UI.</summary>
    public Transform notesTransform;

    /// <summary>Prefab used to instantiate the detective notes UI.</summary>
    public GameObject notesPrefab;

    /// <summary>The currently active detective notes UI instance.</summary>
    private GameObject notesUI;

    /// <summary>Input action reference for toggling the notes UI.</summary>
    public InputActionReference notesButton;

    /// <summary>
    /// Enables the notes input action and subscribes to its performed event.
    /// </summary>
    void OnEnable()
    {
        notesButton.action.Enable();
        notesButton.action.performed += OnPressed;
    }

    /// <summary>
    /// Unsubscribes from the notes input action and disables it.
    /// </summary>
    void OnDisable()
    {
        notesButton.action.performed -= OnPressed;
        notesButton.action.Disable();
    }

    /// <summary>
    /// Toggles the visibility of the notes UI when the notes button is pressed.
    /// </summary>
    void OnPressed(InputAction.CallbackContext context)
    {
        if (notesUI != null)
        {
            notesUI.SetActive(!notesUI.activeSelf);
        }
    }

    /// <summary>
    /// Destroys the current notes UI and re-instantiates it,
    /// reloading the current player's notes data.
    /// </summary>
    public void resetNotes()
    {
        Destroy(notesUI);
        notesUI = Instantiate(notesPrefab, notesTransform);
        notesUI.GetComponent<DetectiveNotes>().setUp(currentPlayer().notes);
    }

    /// <summary>
    /// Updates the moves remaining text every frame for the current player.
    /// </summary>
    private void Update()
    {
        playerMovesText.text = "Moves Left: " + currentPlayer().moves;
    }

    /// <summary>
    /// Returns the player whose turn it currently is.
    /// </summary>
    public Player currentPlayer()
    {
        return activePlayers[currentPlayerIndex];
    }

    // OFFICIAL CLUEDO TURN ORDER
    /// <summary>
    /// The official Cluedo character turn order. Players are sorted
    /// according to this list during turn initialisation.
    /// </summary>
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
    /// <summary>
    /// Ensures only one instance of TurnManager exists (Singleton pattern).
    /// Destroys duplicate instances.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Sorts the active players according to the official Cluedo turn order,
    /// resets the player index to zero, and starts the first turn.
    /// </summary>
    public void InitializeTurnOrder()
    {
        if (activePlayers.Count == 0) return;
        activePlayers = activePlayers.OrderBy(p => officialTurnOrder.IndexOf(p.getCharacter())).ToList();
        currentPlayerIndex = 0;
        StartTurn();
    }

    // TURN LOGIC
    /// <summary>
    /// Starts the turn for the current player. Sets their available moves,
    /// updates the UI with their name and character, displays their hand,
    /// and instantiates a fresh detective notes UI populated with their notes.
    /// </summary>
    public void StartTurn()
    {
        Player currentPlayer = activePlayers[currentPlayerIndex];
        currentPlayer.setMoves();
        playerNameText.text = currentPlayer.getName() + " : " + currentPlayer.getCharacter();
        curvedHandLayout.setup(currentPlayer.hand);
        notesUI = Instantiate(notesPrefab, notesTransform);
        notesUI.GetComponentInChildren<DetectiveNotes>().setUp(currentPlayer.notes);
        Debug.Log("--- TURN STARTED: " + currentPlayer.getName() + " playing as " + currentPlayer.getCharacter() + " ---");
        //DetectiveNotesManager.Instance.LoadPlayerNotes(currentPlayer);
    }

    /// <summary>
    /// Ends the current player's turn, advances to the next player
    /// (wrapping around to the first if the end of the list is reached),
    /// destroys the current notes UI, and triggers the intermission screen.
    /// </summary>
    public void EndCurrentTurn()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= activePlayers.Count)
        {
            // Wrap back to the first player
            currentPlayerIndex = 0;
        }
        Player nextPlayer = activePlayers[currentPlayerIndex];
        Destroy(notesUI);
        TriggerIntermissionScreen(nextPlayer);
    }

    // UI LOGIC
    /// <summary>
    /// Delegates to the IntermissionUI to display the intermission screen
    /// before the next player's turn begins.
    /// </summary>
    private void TriggerIntermissionScreen(Player nextPlayer)
    {
        // Call the UI manager to show the screen
        IntermissionUI.Instance.ShowIntermission(nextPlayer);
    }
}