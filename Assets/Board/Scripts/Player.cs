using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private string playerName;
    [SerializeField] private string character;

    public List<Card> hand = new List<Card>();
    [HideInInspector] public string personalNotesLog = "My Ruled Out Notes:\n";

    // CONSTRUCTOR
    public void Initialize(string name, string character)
    {
        this.playerName = name;
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
        return playerName;
    }

    public string getCharacter()
    {
        return character;
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