using UnityEngine;

public class Player : MonoBehaviour
{
    
    // VARIABLES
    
    // players unity gameobject. used for moving player visual around
    private GameObject gameObject;
    
    // the tile the player is assigned to (the parent gameobject)
    private Tile position;
    
    // CONSTRUCTOR
    public Player(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    // BASIC SETTERS
    public void setPosition(Tile tile)
    {
        this.position = tile;
    }
    
    // BASIC GETTERS
    public Tile getPosition()
    {
        return position;
    }
    public GameObject getGameObject()
    {
        return gameObject;
    }

    
}

// PLACEHOLDERS TO REMOVE
// public class Tile
// {
//     //REMOVE
//     public GameObject getGameObject()
//     {
//         return null;
//     }
//
//     public void setPlayer(Player player)
//     {
//         
//     }
// }

