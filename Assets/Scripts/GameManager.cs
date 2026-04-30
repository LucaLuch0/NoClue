using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the initial setup of the game, including creating players,
/// assigning characters and colours, placing players on the board,
/// dealing cards, and starting the first turn.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>Reference to the MapGenerator used to access starting spaces.</summary>
    public MapGenerator mapGenerator;

    /// <summary>Prefab used to instantiate each player's GameObject.</summary>
    public GameObject playerPrefab;

    /// <summary>Reference to the TurnManager to hand off control once setup is complete.</summary>
    public TurnManager turnManager;

    /// <summary>Reference to the MurderCardSelect responsible for drawing murder cards and dealing hands.</summary>
    public MurderCardSelect murderCardSelect;

    /// <summary>Reference to the PlayerInformation UI, set up after players are created.</summary>
    public PlayerInformation playerInformation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    /// <summary>
    /// Sets up the full game state:
    /// creates player instances, randomly assigns Cluedo characters,
    /// applies player colours, places players on starting spaces,
    /// deals cards, initialises notes, and starts the turn order.
    /// </summary>
    public void setup()
    {
        List<Player> players = new List<Player>();

        // All available Cluedo characters to randomly assign to players
        List<String> characterNames = new List<String>();
        characterNames.Add("Miss Scarlett");
        characterNames.Add("Colonel Mustard");
        characterNames.Add("Mrs. White");
        characterNames.Add("Mr. Green");
        characterNames.Add("Mrs. Peacock");   //// PLACEHOLDER
        characterNames.Add("Professor Plum");

        // Create a Player instance for each registered player name,
        // assigning a random available character to each
        foreach (String name in PlayerManager.playerNames)
        {
            Debug.Log("Player name count: " + characterNames.Count);
            int random = Random.Range(0, characterNames.Count);
            string character = characterNames[random];
            characterNames.RemoveAt(random);

            GameObject playerObject = Instantiate(playerPrefab);
            Player newPlayer = playerObject.GetComponent<Player>();
            newPlayer.Initialize(name, character);
            players.Add(newPlayer);
            newPlayer.gameObject = playerObject;
        }

        Debug.Log(PlayerManager.playerColours.Count + " coloursss");

        // Apply each player's chosen colour to their sprite
        int count = 0;
        foreach (Color color in PlayerManager.playerColours)
        {
            players[count].GetComponent<SpriteRenderer>().color = color;
            count++;
        }

        PlayerManager.addPlayers(players);

        Debug.Log(PlayerManager.getPlayers());
        Debug.Log(mapGenerator.startingSpaces.Count);
        Debug.Log("Players set to start");

        // Push all starting spaces onto a stack so each player can pop one
        Stack<StartingSpace> startingSpaces = new Stack<StartingSpace>();
        foreach (StartingSpace startingSpace in mapGenerator.startingSpaces)
        {
            startingSpaces.Push(startingSpace);
        }

        // Draw murder cards and deal remaining cards to players
        murderCardSelect.setUp();

        // Place each player on a starting space and initialise their notes
        foreach (Player player in PlayerManager.getPlayers())
        {
            player.setPosition(startingSpaces.Pop());
            player.setupNotes();
        }

        Debug.Log("TURN MANAGER CREATED");

        // Hand the player list to the TurnManager and begin the game
        turnManager.activePlayers = players;
        turnManager.InitializeTurnOrder();

        playerInformation.setUp();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
