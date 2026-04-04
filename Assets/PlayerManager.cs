using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public static class PlayerManager
{
    private static List<Player> players = new List<Player>();

    public static void addPlayers(List<Player> newPlayers)
    {
        players = newPlayers;
    }
    
    public static List<Player> getPlayers()
    {
        return players;
    }
    
}
