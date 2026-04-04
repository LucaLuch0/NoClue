using UnityEngine;

public class Player : MonoBehaviour
{
    
    // VARIABLES
    
    // the tile the player is assigned to (the parent gameobject)
    private BoardSpace position;

    // BASIC SETTERS
    public void setPosition(BoardSpace tile)
    {
        this.position = tile;
    }
    
    // BASIC GETTERS
    public BoardSpace getPosition()
    {
        return position;
    }
    public GameObject getGameObject()
    {
        return this.gameObject;
    }

    
}

