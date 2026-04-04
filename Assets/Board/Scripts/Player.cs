using UnityEngine;

public class Player : MonoBehaviour
{
    
    // VARIABLES
    private string name;
    private string character;
    
    // CONSTRUCTOR
    public Player(string name, string character)
    {
        this.name = name;
        this.character = character;
        
        Debug.Log("Player Created: Name = " + name + " Character = " + character + "");
    }

    // the tile the player is assigned to (the parent gameobject)
    private BoardSpace position;

    // BASIC SETTERS
    public void setPosition(BoardSpace tile)
    {
        this.position = tile;
    }
    
    // GETTERS
    public string getName()
    {
        return name;
    }
    public BoardSpace getPosition()
    {
        return position;
    }
    public GameObject getGameObject()
    {
        return this.gameObject;
    }

    
}

