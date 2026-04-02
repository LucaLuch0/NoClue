using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Room : MonoBehaviour
{

    #region VARIABLES
    
    // name of room
    private String name;
    
    // entrance/exit tiles. used for when players exit the rooms.
    private List<Tile> enterOrExitTiles;

    // tiles within the room, excluding entrances and walls. used for placing playesr in room.
    // min 6
    private List<Tile> roomTiles;

    // players in the room
    private List<Player> players;
    
    // where players can be placed in the room
    private Dictionary<Tile, Player> positions;
    
    #endregion
    
    //CONSTRUCTOR
    public Room(string name, List<Tile> enterOrExitTiles, List<Tile> roomTiles)
    {
        if (roomTiles.Count < 6) { Debug.LogError("roomTiles must contain 6 Tiles!"); return; }
        
        this.name = name;
        this.enterOrExitTiles = enterOrExitTiles;
        this.roomTiles = roomTiles;
    }
    
    // BASIC GETTERS

    public String getName()
    {
        return name;
    }

    public List<Tile> getEnterOrExitTiles()
    {
        return enterOrExitTiles;
    }
    
    // PLAYER MANAGEMENT 
    
    // Adds player to the Room and assigns a Tile (Room operates as a single tile)
    public void addPlayer(Player player)
    {
        if (players.Contains(player)) { return; }
        
        players.Add(player);

        // assigns player a free tile
        foreach (Tile tile in roomTiles)
        {
            if (positions.ContainsKey(tile)) { continue; }
            
            positions.Add(tile, player);
            
            // sets players gameobject position to be that of a valid tile
            player.getGameObject().transform.SetParent(tile.getGameObject().transform);
            player.getGameObject().transform.position =  tile.getGameObject().transform.position;;

            break;
        }
    }
    
    // Removes player from Tile and Room, and reassigns to an exit
    public void removePlayer(Player player, Tile exit)
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
        
        player.getGameObject().transform.SetParent(exit.getGameObject().transform);
        player.getGameObject().transform.position =  exit.getGameObject().transform.position;;

    }
    

}
