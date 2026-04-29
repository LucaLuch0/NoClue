using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public static class PlayerManager
{
    public static MapGenerator mapGenerator;
    public static List<String> playerNames = new List<String>();
    public static List<Color> playerColours = new List<Color>();

    private static List<Player> players = new List<Player>();
    public static GameObject playerPrefab;
    public static SuggestionUIManager suggestionUIManager;
    public static Suggestion suggestion;


    public static void addPlayers(List<Player> newPlayers)
    {
        players = newPlayers;
    }
    
    public static List<Player> getPlayers()
    {
        return players;
    }

    public static void generatePlayers()
    {
        
    }
    
}
