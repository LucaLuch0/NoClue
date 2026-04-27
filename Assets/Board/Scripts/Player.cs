using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private string playerName;
    [SerializeField] private string character;

    public List<Card> hand = new List<Card>();
    [HideInInspector] public string personalNotesLog = "My Ruled Out Notes:\n";
    public GameObject gameObject;

    private float adjustment = 0.5f;
    public int moves;
    
    // CONSTRUCTOR
    public void Initialize(string name, string character)
    {
        this.playerName = name;
        this.character = character;

        moves = 100;
        Debug.Log("Player Created: Name = " + name + " Character = " + character + "");
    }

    // the tile the player is assigned to (the parent gameobject)
    private BoardSpace position;

    // BASIC SETTERS
    public void setPosition(BoardSpace tile)
    {
        this.position = tile;
        Vector2 newVector = new Vector2(tile.worldPos.x + this.adjustment, tile.worldPos.y + adjustment);
        gameObject.transform.position = newVector;
    }

    public void movePlayer(BoardSpace tile)
    {
        
        
        
        if (moves <= 0)
        {
            return;
        }
        
        // checks if player is in room and allows move if it is a valid exit to a room
        if (position is RoomSpace)
        {
            foreach (ExitSpace exitSpace in PlayerManager.mapGenerator.exitSpaces)
            {
                if (exitSpace.room.Equals(((RoomSpace) position).room))
                {
                    if (tile.pos == exitSpace.pos)
                    {
                        exitSpace.room.removePlayer(this);
                        setPosition(tile);
                        moves--;
                        return;
                    }
                }
            }
        }
        
        if (!isTileNextToPlayer(tile))
        {
            Debug.Log("Tile not next to the player");
            return;
        }

        if (!isValidMove(tile))
        {
            Debug.Log("Invalid move");
            return;
        }

        if (isDoor(tile))
        {
            foreach (RoomSpace roomSpace in PlayerManager.mapGenerator.roomSpaces)
            {
                if (roomSpace.pos == tile.pos)
                {
                    roomSpace.room.addPlayer(this, adjustment);
                    position = roomSpace;
                    Debug.Log("Is Door");
                    break;
                }
            }
        }
        else
        {
            setPosition(tile);

        }
        Debug.Log("Player Moved");
        moves--;
        
    }

    public bool isDoor(BoardSpace tile)
    {
        foreach (DoorSpace door in PlayerManager.mapGenerator.doorSpaces)
        {
            if (tile.pos == door.pos)
            {
                return true;
            }
        }

        return false;
    }

    public bool isValidMove(BoardSpace tile)
    {
        foreach (HallwaySpace hallwaySpace in PlayerManager.mapGenerator.hallwaySpaces)
        {
            if (hallwaySpace.Equals(tile))
            {
                return true;
            }
        }

        foreach (Vector2Int pos in PlayerManager.mapGenerator.getDoors())
        {
            if (tile.pos == pos)
            {
                return true;
            }
        }

        foreach (ExitSpace exitSpace in PlayerManager.mapGenerator.exitSpaces)
        {
            if (exitSpace.Equals(tile))
            {
                return true;
            }
        }

        return false;
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

    public bool isTileNextToPlayer(BoardSpace tile)
    {
        if (tile.pos.x == position.pos.x + 1 && tile.pos.y == position.pos.y)
        {
            return true;
        }
        if (tile.pos.x == position.pos.x - 1 && tile.pos.y == position.pos.y)
        {
            return true;
        }
        if (tile.pos.x == position.pos.x && tile.pos.y == position.pos.y + 1)
        {
            return true;
        }
        if (tile.pos.x == position.pos.x && tile.pos.y == position.pos.y - 1)
        {
            return true;
        }
        
        return false;
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