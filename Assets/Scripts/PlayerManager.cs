using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A static manager that holds global references and data shared across the game,
/// including the list of players, their names, colours, and key system references.
/// </summary>
public static class PlayerManager
{
    /// <summary>Reference to the MapGenerator, used by players for movement and board lookups.</summary>
    public static MapGenerator mapGenerator;

    /// <summary>The names entered by players at the start of the game.</summary>
    public static List<String> playerNames = new List<String>();

    /// <summary>The colours assigned to each player's token.</summary>
    public static List<Color> playerColours = new List<Color>();

    /// <summary>The list of active Player instances in the game.</summary>
    private static List<Player> players = new List<Player>();

    /// <summary>The prefab used to instantiate player GameObjects.</summary>
    public static GameObject playerPrefab;

    /// <summary>Reference to the legacy SuggestionUIManager (currently unused in favour of Suggestion).</summary>
    public static SuggestionUIManager suggestionUIManager;

    /// <summary>Reference to the active Suggestion handler used during gameplay.</summary>
    public static Suggestion suggestion;

    /// <summary>
    /// Replaces the current player list with the provided list of players.
    /// </summary>
    /// <param name="newPlayers">The new list of players to store.</param>
    public static void addPlayers(List<Player> newPlayers)
    {
        players = newPlayers;
    }

    /// <summary>
    /// Returns the current list of active players.
    /// </summary>
    public static List<Player> getPlayers()
    {
        return players;
    }

    /// <summary>
    /// Placeholder for future player generation logic.
    /// </summary>
    public static void generatePlayers()
    {
    }
}
