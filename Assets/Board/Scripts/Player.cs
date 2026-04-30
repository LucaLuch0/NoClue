using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
/// Represents a player in the game, tracking their identity, position,
/// hand of cards, notes, moves, and handling movement logic on the board.
/// </summary>
public class Player : MonoBehaviour
{
    // VARIABLES
    /// <summary>The human player's name (e.g. "Alice").</summary>
    [SerializeField] private string playerName;

    /// <summary>The Cluedo character this player is assigned (e.g. "Miss Scarlett").</summary>
    [SerializeField] private string character;

    /// <summary>The cards currently held in this player's hand.</summary>
    public List<Card> hand = new List<Card>();

    /// <summary>A personal log of notes the player has ruled out. Hidden in the Inspector.</summary>
    [HideInInspector] public string personalNotesLog = "My Ruled Out Notes:\n";

    /// <summary>
    /// A dictionary mapping each card to whether it has been ruled out (true) or not (false).
    /// </summary>
    public Dictionary<Card, bool> notes = new Dictionary<Card, bool>();

    /// <summary>The GameObject representing this player on the board.</summary>
    public GameObject gameObject;

    /// <summary>Whether this player has already made their final accusation.</summary>
    public bool guessed;

    /// <summary>Positional offset applied when placing the player token on a tile.</summary>
    private float adjustment = 0.5f;

    /// <summary>The number of moves the player has remaining this turn.</summary>
    public int moves;

    /// <summary>
    /// Assigns a random number of moves (1–12) to the player at the start of their turn.
    /// </summary>
    public void setMoves()
    {
        moves = Random.Range(1, 13);
    }

    // CONSTRUCTOR
    /// <summary>
    /// Initialises the player with a name and character, and resets their guessed state.
    /// </summary>
    /// <param name="name">The human player's display name.</param>
    /// <param name="character">The Cluedo character assigned to this player.</param>
    public void Initialize(string name, string character)
    {
        this.playerName = name;
        this.character = character;
        guessed = false;
    }

    /// <summary>
    /// Populates the player's notes dictionary with all cards in the game.
    /// Cards in the player's hand are pre-marked as ruled out (true),
    /// all others default to not ruled out (false).
    /// </summary>
    public void setupNotes()
    {
        foreach (Card c in CardManager.getCharacters())
        {
            notes.Add(c, false);
        }
        foreach (Card c in CardManager.getRooms())
        {
            notes.Add(c, false);
        }
        foreach (Card c in CardManager.getWeapons())
        {
            notes.Add(c, false);
        }

        // Mark cards in the player's hand as already known/ruled out
        foreach (Card c in hand)
        {
            notes[c] = true;
        }

        Debug.Log("NOTES LENGTH" + notes.Count);
    }

    /// <summary>The board space the player is currently occupying.</summary>
    private BoardSpace position;

    // BASIC SETTERS
    /// <summary>
    /// Sets the player's board position and updates their GameObject's world position,
    /// applying the visual adjustment offset.
    /// </summary>
    /// <param name="tile">The board space to place the player on.</param>
    public void setPosition(BoardSpace tile)
    {
        this.position = tile;
        Vector2 newVector = new Vector2(tile.worldPos.x + this.adjustment, tile.worldPos.y + adjustment);
        gameObject.transform.position = newVector;
    }

    /// <summary>
    /// Attempts to move the player to the given tile, handling all movement rules:
    /// room exits, shortcuts, door entry, and standard hallway movement.
    /// Decrements moves on a successful move.
    /// </summary>
    /// <param name="tile">The board space the player is attempting to move to.</param>
    public void movePlayer(BoardSpace tile)
    {
        // Cannot move if no moves remain
        if (moves <= 0)
        {
            return;
        }

        // Handle movement logic when the player is currently inside a room
        if (position is RoomSpace)
        {
            // Check if the target tile is a shortcut space linked to the current room
            foreach (ShortcutSpace shortcutSpace in PlayerManager.mapGenerator.shortcutSpaces)
            {
                if (tile.pos == shortcutSpace.pos)
                {
                    Debug.Log("Shortcut room " + shortcutSpace.room.name + "Position room" + ((RoomSpace)position).room.name);
                    if (shortcutSpace.room.Equals(((RoomSpace)position).room))
                    {
                        Debug.Log("SHORTCUT STARTED");
                        // Teleport the player to the opposite room via the shortcut
                        Room newRoom =
                            ((ShortcutSpace)PlayerManager.mapGenerator.getBoardSpace(shortcutSpace.oppositeSpace)).room;
                        ((RoomSpace)position).room.removePlayer(this);
                        newRoom.addPlayer(this, adjustment);
                        position = newRoom.getPlayerPosition(this);
                        moves = 0;
                        Debug.Log("Is Door");
                        PlayerManager.suggestion.uiSetup(this, newRoom);
                        return;
                    }
                }
            }

            // Check if the target tile is a valid exit for the current room
            foreach (ExitSpace exitSpace in PlayerManager.mapGenerator.exitSpaces)
            {
                if (exitSpace.room.Equals(((RoomSpace)position).room))
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

        // Ensure the target tile is adjacent to the player
        if (!isTileNextToPlayer(tile))
        {
            Debug.Log("Tile not next to the player");
            return;
        }

        // Ensure the target tile is a legal move (hallway, door, or exit)
        if (!isValidMove(tile))
        {
            Debug.Log("Invalid move");
            return;
        }

        // If moving onto a door, add the player to the corresponding room
        if (isDoor(tile))
        {
            foreach (RoomSpace roomSpace in PlayerManager.mapGenerator.roomSpaces)
            {
                if (roomSpace.pos == tile.pos)
                {
                    roomSpace.room.addPlayer(this, adjustment);
                    position = roomSpace.room.getPlayerPosition(this);
                    moves = 0;
                    Debug.Log("Is Door");
                    PlayerManager.suggestion.uiSetup(this, roomSpace.room);
                    return;
                }
            }
        }
        else
        {
            // Standard move to a hallway tile
            setPosition(tile);
        }

        Debug.Log("Player Moved");
        moves--;
    }

    /// <summary>
    /// Returns true if the given tile is a door space on the board.
    /// </summary>
    /// <param name="tile">The tile to check.</param>
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

    /// <summary>
    /// Returns true if the given tile is a legal destination:
    /// a hallway space, a door, or a room exit.
    /// </summary>
    /// <param name="tile">The tile to validate.</param>
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

    /// <summary>Returns the human player's display name.</summary>
    public string getName()
    {
        return playerName;
    }

    /// <summary>Returns the Cluedo character assigned to this player.</summary>
    public string getCharacter()
    {
        return character;
    }

    /// <summary>
    /// Returns true if the given tile is directly adjacent (up, down, left, or right)
    /// to the player's current position.
    /// </summary>
    /// <param name="tile">The tile to check adjacency for.</param>
    public bool isTileNextToPlayer(BoardSpace tile)
    {
        if (tile.pos.x == position.pos.x + 1 && tile.pos.y == position.pos.y) return true;
        if (tile.pos.x == position.pos.x - 1 && tile.pos.y == position.pos.y) return true;
        if (tile.pos.x == position.pos.x && tile.pos.y == position.pos.y + 1) return true;
        if (tile.pos.x == position.pos.x && tile.pos.y == position.pos.y - 1) return true;

        return false;
    }

    /// <summary>Returns the board space the player is currently on.</summary>
    public BoardSpace getPosition()
    {
        return position;
    }

    /// <summary>Returns the GameObject representing this player on the board.</summary>
    public GameObject getGameObject()
    {
        return this.gameObject;
    }
}