using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Room
{

    #region VARIABLES
    
    // name of room
    public String name;
    
    // tiles within the room, excluding entrances and walls. used for placing players in the room.
    // min 6
    private List<RoomSpace> roomTiles;

    // players in the room
    private List<Player> players;
    
    private List<RoomSpace> doorTiles;
    
    // where players can be placed in the room
    private Dictionary<RoomSpace, Player> positions;
    
    #endregion
    
    //CONSTRUCTOR
    public Room(string name)
    {
        this.name = name;
        
        roomTiles = new List<RoomSpace>();
        doorTiles = new List<RoomSpace>();
        players = new List<Player>();
        positions = new Dictionary<RoomSpace, Player>();
    }

    public RoomSpace getPlayerPosition(Player p)
    {
        return positions.FirstOrDefault(x => x.Value == p).Key;

    }
    public void addRoomTile(RoomSpace roomSpace)
    {
        roomTiles.Add(roomSpace);
    }

    public void addDoorTile(RoomSpace roomSpace)
    {
        doorTiles.Add(roomSpace);
    }
    
    // BASIC GETTERS

    public String getName()
    {
        return name;
    }

    public void debug()
    {
        Debug.Log(name + "RoomTiles: " + roomTiles.Count + "DoorTiles: " + doorTiles.Count);
    }
    
    // PLAYER MANAGEMENT 
    
    // Adds player to the Room and assigns a Tile (Room operates as a single tile)
    public void addPlayer(Player player, float adjustment)
    {
        if (players.Contains(player)) { return; }
        
        players.Add(player);

        // assings the player a random free tile
        for (int i = 0; i < roomTiles.Count; i++)
        {
            int random = Random.Range(0, roomTiles.Count);
            RoomSpace roomSpace = roomTiles[random];

            if (positions.ContainsKey(roomSpace)) { continue; }
            
            positions.Add(roomSpace, player);
            
            Debug.Log(roomSpace.worldPos);
            
            Vector2 newWorldPos = new Vector2(roomSpace.worldPos.x + adjustment, roomSpace.worldPos.y + adjustment);
            
            // sets players gameobject position to be that of a valid tile
            player.getGameObject().transform.position = newWorldPos;

            break;
        }        
    }
    
    // Removes player from Tile and Room, and reassigns to an exit
    public void removePlayer(Player player)
    {
        players.Remove(player);
        
        // removes players tile assignment
        foreach (var item in positions)
        {
            if (item.Value == player)
            {
                positions.Remove(item.Key);
                break;
            }
        }
    }
    

}
