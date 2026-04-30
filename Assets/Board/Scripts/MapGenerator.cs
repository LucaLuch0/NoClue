using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// Generates the Tilemap for the board. Places tiles and allocates them to the relevant class depending on whether they are in a Room, Hall, a Shortcut, or and Door/Exit.
/// </summary>

public class MapGenerator : MonoBehaviour
{
    /// <summary>The total width of the board in tiles.</summary>
    [Header("Board Dimensions")] public int width = 24;
    /// <summary>The total height of the board in tiles.</summary>
    public int height = 25;

    /// <summary>The Tilemap used for rendering all board tiles visually.</summary>
    [Header("Base Tiles")]
    public Tilemap visualTilemap;
    /// <summary>The default tile used for hallway spaces.</summary>
    public TileBase hallwayBase;
    /// <summary>The world-space width of a single tile, derived from the Tilemap cell size.</summary>
    public float tileWidth;
    
    /// <summary>Room Interior & Outline Tiles.</summary>
    [Header("Room Interior & Outlines")]
    public TileBase roomCenter; 
    public TileBase roomWallTop;
    public TileBase roomWallBottom;
    public TileBase roomWallLeft;
    public TileBase roomWallRight;
    public TileBase roomCornerTopLeft;
    public TileBase roomCornerTopRight;
    public TileBase roomCornerBottomLeft;
    public TileBase roomCornerBottomRight;

    /// <summary>Hallway door Tiles.</summary>
    [Header("Hallway Door Tiles")]
    public TileBase hallwayDoorTop;
    public TileBase hallwayDoorBottom;
    public TileBase hallwayDoorLeft;
    public TileBase hallwayDoorRight;

    /// <summary>Shortcut Tile.</summary>
    public TileBase shortcut;

    /// <summary>Room Door Tiles.</summary>
    [Header("Room Door Tiles")]
    public TileBase roomDoorTop;
    public TileBase roomDoorBottom;
    public TileBase roomDoorLeft;
    public TileBase roomDoorRight;

    /// <summary>Room Interior & Outline Tiles.</summary>
    [Header("UI Labels")]
    public GameObject roomLabelPrefab;

    /// <summary>The four cardinal directions a door can face.</summary>
    public enum DoorDirection { Top, Bottom, Left, Right }
    /// <summary>Maps each door tile position to its facing direction.</summary>
    private Dictionary<Vector2Int, DoorDirection> doorRegistry = new Dictionary<Vector2Int, DoorDirection>();
    /// <summary>All paired door connections on the board.</summary>
    public List<Door> doors = new List<Door>();
    /// <summary>Raw room rectangles used for tile placement and room name lookup.</summary>
    private List<RectInt> allRooms = new List<RectInt>();
    /// <summary>Room objects containing metadata and tile lists for each named room.</summary>
    private List<Room> rooms = new List<Room>();
    /// <summary>All player starting spaces on the board perimeter.</summary>
    public List<StartingSpace> startingSpaces = new List<StartingSpace>();
    /// <summary>All spaces that fall inside a room boundary.</summary>
    public List<RoomSpace> roomSpaces = new List<RoomSpace>();
    /// <summary>All spaces that fall in the hallway (outside rooms).</summary>
    public List<HallwaySpace> hallwaySpaces = new List<HallwaySpace>();
    /// <summary>Hallway-side tiles of each doorway.</summary>
    public List<DoorSpace> doorSpaces = new List<DoorSpace>();
    /// <summary>Room-side tiles of each doorway (used to enter a room).</summary>
    public List<ExitSpace> exitSpaces = new List<ExitSpace>();
    /// <summary>Corner shortcut spaces that teleport players to the opposite corner room.</summary>
    public List<ShortcutSpace> shortcutSpaces = new List<ShortcutSpace>();
    /// <summary>Reference to the central game manager, called after board setup is complete.</summary>
    public GameManager gameManager;

    /// <summary>
    /// Finds and returns the BoardSpace at the given cell position.
    /// Checks shortcut, room, and hallway spaces in priority order.
    /// </summary>
    /// <param name="cellPos">The grid position to look up.</param>
    /// <returns>
    /// The matching BoardSpace, or null if no space exists at that position.
    /// </returns>
    public BoardSpace getBoardSpace(Vector2Int cellPos)
    {
        foreach (ShortcutSpace shortcutSpace in shortcutSpaces)
        {
            if (shortcutSpace.pos == cellPos)
            {
                return shortcutSpace;
            }
        }
        
        foreach (RoomSpace roomSpace in roomSpaces)
        {
            if (roomSpace.pos == cellPos)
            {
                return roomSpace;
            }
        }

        foreach (HallwaySpace hallwaySpace in hallwaySpaces)
        {
            if (hallwaySpace.pos == cellPos)
            {
                return hallwaySpace;
            }
        }

        return null;
    }
    
    
    /// <summary>
    /// Returns a list of all registered door positions on the board.
    /// </summary>
    /// <returns>A list of Vector2Int door positions.</returns>
    public List<Vector2Int> getDoors()
    {
        List<Vector2Int> doors = new List<Vector2Int>();
        foreach (Vector2Int pos in doorRegistry.Keys)
        {
            doors.Add(pos);
        }

        return doors;
    }
    
    /// <summary>
    /// Defines all rooms, doors, and starting spaces,
    /// then triggers board generation, room labelling, shortcut placement,
    /// and finally calls setup.
    /// </summary>

    void Start()
    {
        PlayerManager.mapGenerator = this;
        
        tileWidth = visualTilemap.cellSize.x;
        
        // Define room rectangles (position + size) for each named Cluedo room
        allRooms.Add(new RectInt(0, 0, 6, 6));   // Kitchen
        allRooms.Add(new RectInt(8, 0, 8, 7));   // Ballroom
        allRooms.Add(new RectInt(18, 0, 6, 5));  // Conservatory
        allRooms.Add(new RectInt(0, 9, 8, 7));   // Dining Room
        allRooms.Add(new RectInt(10, 9, 5, 7));  // Cellar
        allRooms.Add(new RectInt(18, 8, 6, 5));  // Billiard
        allRooms.Add(new RectInt(17, 14, 7, 5)); // Library
        allRooms.Add(new RectInt(0, 19, 7, 6));  // Lounge
        allRooms.Add(new RectInt(9, 18, 6, 7));  // Hall
        allRooms.Add(new RectInt(17, 21, 7, 4)); // Study
        
        rooms.Add(new Room("KITCHEN"));
        rooms.Add(new Room("BALLROOM"));
        rooms.Add(new Room("CONSERVATORY"));
        rooms.Add(new Room("DINING ROOM"));
        rooms.Add(new Room("CELLAR"));
        rooms.Add(new Room("BILLIARD"));
        rooms.Add(new Room("LIBRARY"));
        rooms.Add(new Room("LOUNGE"));
        rooms.Add(new Room("HALL"));
        rooms.Add(new Room("STUDY"));
        
        // Define the six player starting positions along the board edges
        Vector2Int startingSpace1 = new  Vector2Int(6, 0);
        Vector2Int startingSpace2 = new  Vector2Int(16, 0);
        Vector2Int startingSpace3 = new  Vector2Int(7, 24);        
        Vector2Int startingSpace4 = new  Vector2Int(15, 24);
        Vector2Int startingSpace5 = new  Vector2Int(0, 7); 
        Vector2Int startingSpace6 = new  Vector2Int(23, 6);

        startingSpaces.Add(new StartingSpace(startingSpace1, visualTilemap.CellToWorld((Vector3Int) startingSpace1)));
        startingSpaces.Add(new StartingSpace(startingSpace2, visualTilemap.CellToWorld((Vector3Int) startingSpace2)));
        startingSpaces.Add(new StartingSpace(startingSpace3, visualTilemap.CellToWorld((Vector3Int) startingSpace3)));
        startingSpaces.Add(new StartingSpace(startingSpace4, visualTilemap.CellToWorld((Vector3Int) startingSpace4)));
        startingSpaces.Add(new StartingSpace(startingSpace5, visualTilemap.CellToWorld((Vector3Int) startingSpace5)));
        startingSpaces.Add(new StartingSpace(startingSpace6, visualTilemap.CellToWorld((Vector3Int) startingSpace6)));
        
        // Define Doors
        // Lounge
        RegisterDoor(4, 5, DoorDirection.Top, false, "LOUNGE"); 
        RegisterDoor(4, 6, DoorDirection.Bottom, true, "LOUNGE"); 

        // Dining Room
        RegisterDoor(6, 8, DoorDirection.Top, true, "DINING ROOM");
        RegisterDoor(6, 9, DoorDirection.Bottom, false, "DINING ROOM");

        RegisterDoor(7, 12, DoorDirection.Right, false, "DINING ROOM");
        RegisterDoor(8, 12, DoorDirection.Left, true,  "DINING ROOM");
        
        // Library
        RegisterDoor(17, 10, DoorDirection.Right, true, "LIBRARY");
        RegisterDoor(18, 10, DoorDirection.Left, false, "LIBRARY");

        RegisterDoor(20, 12, DoorDirection.Top, false, "LIBRARY");
        RegisterDoor(20, 13, DoorDirection.Bottom, true, "LIBRARY");

        // Ball Room
        RegisterDoor(13, 17, DoorDirection.Top,  true, "BALLROOM");
        RegisterDoor(13, 18, DoorDirection.Bottom, false,  "BALLROOM");

        RegisterDoor(14, 20, DoorDirection.Right, false, "BALLROOM");
        RegisterDoor(15, 20, DoorDirection.Left, true,  "BALLROOM");

        RegisterDoor(10, 17, DoorDirection.Top, true, "BALLROOM");
        RegisterDoor(10, 18, DoorDirection.Bottom, false, "BALLROOM");

        RegisterDoor(8, 20, DoorDirection.Right, true,  "BALLROOM");
        RegisterDoor(9, 20, DoorDirection.Left, false, "BALLROOM");

        // Kitchen
        RegisterDoor(5, 19, DoorDirection.Bottom, false, "KITCHEN");
        RegisterDoor(5, 18, DoorDirection.Top, true, "KITCHEN");

        // Conservatory
        RegisterDoor(18, 20, DoorDirection.Top, true, "CONSERVATORY");
        RegisterDoor(18, 21, DoorDirection.Bottom, false, "CONSERVATORY");

        // Billiard Room
        RegisterDoor(16, 17, DoorDirection.Right, true, "BILLIARD");
        RegisterDoor(17, 17, DoorDirection.Left, false,  "BILLIARD");

        RegisterDoor(22, 13, DoorDirection.Top, true, "BILLIARD");
        RegisterDoor(22, 14, DoorDirection.Bottom, false, "BILLIARD");

        // Hall
        RegisterDoor(11, 6, DoorDirection.Top, false, "HALL");
        RegisterDoor(11, 7, DoorDirection.Bottom, true, "HALL");
        RegisterDoor(12, 6, DoorDirection.Top, false, "HALL");
        RegisterDoor(12, 7, DoorDirection.Bottom, true, "HALL");

        RegisterDoor(15, 3, DoorDirection.Right,  false, "HALL");
        RegisterDoor(16, 3, DoorDirection.Left, true, "HALL");

        // Study
        RegisterDoor(19, 4, DoorDirection.Top, false, "STUDY");
        RegisterDoor(19, 5, DoorDirection.Bottom, true, "STUDY");

        
        GenerateCluedoBoard();
        LabelRooms();
        GenerateShortcuts();
        
        gameManager.setup();
    }
    
    /// <summary>
    /// Determines whether a given BoardSpace is a shortcut space.
    /// </summary>
    /// <param name="boardSpace">The space to check.</param>
    /// <returns>true if the space is a shortcut corner, otherwise false.</returns>
    public bool isShortcutSpace(BoardSpace boardSpace)
    {
        foreach (ShortcutSpace shortcutSpace in shortcutSpaces)
        {
            if (boardSpace.pos == shortcutSpace.pos)
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Creates the four corner shortcut spaces, links each to its opposite corner,
    /// and places the shortcut tile on the Tilemap.
    /// </summary>
    /// <remarks>
    /// Shortcuts connect: Lounge ↔ Conservatory and Study ↔ Kitchen.
    /// </remarks>
    void GenerateShortcuts()
    {
        Vector2Int shortcut1point1 = new Vector2Int(0, 0);
        Vector2Int shortcut1point2 = new Vector2Int(23, 24);
        Vector2Int shortcut2point1 = new Vector2Int(23, 0);
        Vector2Int shortcut2point2 = new Vector2Int(0, 24);
        
        shortcutSpaces.Add(new ShortcutSpace(shortcut1point1, visualTilemap.CellToWorld((Vector3Int) shortcut1point1), GetRoom("LOUNGE"), shortcut1point2));
        shortcutSpaces.Add(new ShortcutSpace(shortcut1point2, visualTilemap.CellToWorld((Vector3Int) shortcut1point2), GetRoom("CONSERVATORY"), shortcut1point1));
        shortcutSpaces.Add(new ShortcutSpace(shortcut2point1, visualTilemap.CellToWorld((Vector3Int) shortcut2point1), GetRoom("STUDY"), shortcut2point2));
        shortcutSpaces.Add(new ShortcutSpace(shortcut2point2, visualTilemap.CellToWorld((Vector3Int) shortcut2point2), GetRoom("KITCHEN"), shortcut2point1));

        visualTilemap.SetTile((Vector3Int)shortcut1point1, shortcut);
        visualTilemap.SetTile((Vector3Int)shortcut1point2, shortcut);
        visualTilemap.SetTile((Vector3Int)shortcut2point1, shortcut);
        visualTilemap.SetTile((Vector3Int)shortcut2point2, shortcut);
    }
    
    /// <summary>
    /// Iterates over every tile position in the board grid and creates either
    /// a room tile or a hallway tile depending on whether the position falls
    /// inside a defined room rectangle.
    /// </summary>
    void GenerateCluedoBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                RectInt foundRect = new RectInt(0,0,0,0);
                bool isInside = false;

                foreach (var room in allRooms)
                {
                    if (room.Contains(pos))
                    {
                        isInside = true;
                        foundRect = room;
                        break;
                    }
                }

                if (isInside) CreateRoom(pos, foundRect);
                else CreateHallway(pos);
            }
        }
        Debug.Log("Board Generation Finished!");
    }

    /// <summary>
    /// Registers a door tile at the given position, assigns its facing direction,
    /// and adds it to either exitSpaces (room-side) or
    /// doorSpaces (hallway-side).
    /// </summary>
    /// <param name="x">The tile's X grid coordinate.</param>
    /// <param name="y">The tile's Y grid coordinate.</param>
    /// <param name="dir">The direction the door faces.</param>
    /// <param name="isExit">
    /// true if this tile is the room-side (exit) tile; false for the hallway-side tile.
    /// </param>
    /// <param name="room">The name of the room this door belongs to.</param>
    void RegisterDoor(int x, int y, DoorDirection dir, bool isExit, String room)
    {
        doorRegistry[new Vector2Int(x, y)] = dir;
        if (isExit)
        {
            ExitSpace exitSpace = new ExitSpace(new Vector2Int(x, y), visualTilemap.CellToWorld(new Vector3Int(x, y)), GetRoom(room));
            exitSpaces.Add(exitSpace);
        }
        else
        {
            DoorSpace doorSpace = new DoorSpace(new Vector2Int(x, y),visualTilemap.CellToWorld(new Vector3Int(x, y)), GetRoom(room));
            doorSpaces.Add(doorSpace);
        }
    }

    /// <summary>
    /// Creates a hallway tile at the given position. If the position is registered
    /// as a door, the appropriate directional door tile is used instead of the base hallway tile.
    /// </summary>
    /// <param name="pos">The grid position to place the hallway tile.</param>
    void CreateHallway(Vector2Int pos)
    {
        bool isDoor = false;
        TileBase selectedTile = hallwayBase;
        if (doorRegistry.ContainsKey(pos))
        {
            isDoor = true;
            switch (doorRegistry[pos])
            {
                case DoorDirection.Top: selectedTile = hallwayDoorTop; break;
                case DoorDirection.Bottom: selectedTile = hallwayDoorBottom; break;
                case DoorDirection.Left: selectedTile = hallwayDoorLeft; break;
                case DoorDirection.Right: selectedTile = hallwayDoorRight; break;
            }
        }
        visualTilemap.SetTile((Vector3Int)pos, selectedTile);
        hallwaySpaces.Add(new HallwaySpace(pos, visualTilemap.CellToWorld((Vector3Int) pos), isDoor));
    }

    /// <summary>
    /// Creates a room tile at the given position within the specified room rectangle.
    /// Door tiles take priority, followed by corners, then walls, then the room interior.
    /// The tile is registered as a RoomSpace and added to the owning Room.
    /// Shortcut positions are excluded from the room's walkable tile list.
    /// </summary>
    /// <param name="pos">The grid position to place the room tile.</param>
    /// <param name="rect">The bounding rectangle of the room this position belongs to.</param>
    void CreateRoom(Vector2Int pos, RectInt rect)
    {
        TileBase selectedTile = roomCenter;

        // 1. Doors take priority
        if (doorRegistry.ContainsKey(pos))
        {
            switch (doorRegistry[pos])
            {
                case DoorDirection.Top: selectedTile = roomDoorTop; break;
                case DoorDirection.Bottom: selectedTile = roomDoorBottom; break;
                case DoorDirection.Left: selectedTile = roomDoorLeft; break;
                case DoorDirection.Right: selectedTile = roomDoorRight; break;
            }
            
            // Adds the Tilemap & World Position of the Tile to the Room Class
            Vector2 worldPos = visualTilemap.CellToWorld(new Vector3Int(pos.x, pos.y, 0));
            string roomName = GetRoomName(rect);
            Room room = GetRoom(roomName);
            if (room == null) { return; }
            RoomSpace roomSpace = (new RoomSpace(pos, worldPos, room, true));
            roomSpaces.Add(roomSpace);
            
        }
        else
        {
            // 2. Corner Detection
            if (pos.x == rect.xMin && pos.y == rect.yMax - 1) selectedTile = roomCornerTopLeft;
            else if (pos.x == rect.xMax - 1 && pos.y == rect.yMax - 1) selectedTile = roomCornerTopRight;
            else if (pos.x == rect.xMin && pos.y == rect.yMin) selectedTile = roomCornerBottomLeft;
            else if (pos.x == rect.xMax - 1 && pos.y == rect.yMin) selectedTile = roomCornerBottomRight;
            
            // 3. Wall Detection (if not a corner)
            else if (pos.y == rect.yMax - 1) selectedTile = roomWallTop;
            else if (pos.y == rect.yMin) selectedTile = roomWallBottom;
            else if (pos.x == rect.xMin) selectedTile = roomWallLeft;
            else if (pos.x == rect.xMax - 1) selectedTile = roomWallRight;
            
            // Adds the World Position of the Tile to the Room Class
            Vector2 worldPos = visualTilemap.CellToWorld(new Vector3Int(pos.x, pos.y, 0));
            
            string roomName = GetRoomName(rect);
            Room room = GetRoom(roomName);
            if (room == null) { return; }
            RoomSpace roomSpace = (new RoomSpace(pos, worldPos, room, false));
            roomSpaces.Add(roomSpace);
            
            // Checks if its a shortcut space. If not it adds to the room
            // This is to avoid players being placed in the same tile as a shortcut
            if (!isShortcutSpace(roomSpace))
            {
                room.addRoomTile(roomSpace);
            }
        }
        
        visualTilemap.SetTile((Vector3Int)pos, selectedTile);
        
    }

    /// <summary>
    /// Instantiates a room label prefab at the centre of each room rectangle
    /// and sets its text to the room's name.
    /// </summary>
    void LabelRooms()
    {
        foreach (var room in allRooms)
        {
            float centreX = room.x + (room.width / 2f);
            float centreY = room.y + (room.height / 2f);
            Vector3 centrePos = new Vector3(centreX, centreY, -1f);

            GameObject label = Instantiate(roomLabelPrefab, centrePos, Quaternion.identity);

            TMP_Text textComponent = label.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = GetRoomName(room);
            }
        }
    }

    /// <summary>
    /// Maps a room rectangle to its Cluedo room name using its origin coordinates.
    /// </summary>
    /// <param name="rect">The room's bounding rectangle.</param>
    /// <returns>The room name as a string, or an empty string if unrecognised.</returns>
    string GetRoomName(RectInt rect)
    {
        if (rect.x == 0 && rect.y == 0) return "LOUNGE";
        if (rect.x == 8 && rect.y == 0) return "HALL";
        if (rect.x == 18 && rect.y == 0) return "STUDY";
        if (rect.x == 0 && rect.y == 9) return "DINING ROOM";
        //if (rect.x == 10 && rect.y == 9) return "NoClue";
        if (rect.x == 18 && rect.y == 8) return "LIBRARY";
        if (rect.x == 17 && rect.y == 14) return "BILLIARD";
        if (rect.x == 0 && rect.y == 19) return "KITCHEN";
        if (rect.x == 9 && rect.y == 18) return "BALLROOM";
        if (rect.x == 17 && rect.y == 21) return "CONSERVATORY";
        return "";
    }
    
    /// <summary>
    /// Finds and returns the Room with the given name.
    /// </summary>
    /// <param name="name">The room name to search for (case-sensitive).</param>
    /// <returns>The matching Room, or null if not found.</returns>
    public Room GetRoom(string name)
    {
        foreach (Room room in rooms)
        {
            if (room.name == name)
            {
                return room;
            }
        }
        Debug.LogError("Room Not Found!");
        return null;
    }
}

/// <summary>
/// Abstract base class for all spaces on the board.
/// Stores the grid position and world-space position of a tile.
/// </summary>

public abstract class BoardSpace { 
    /// <summary>The grid (cell) position of this space.</summary>
    public Vector2Int pos;
    /// <summary>The world-space position of this space.</summary>
    public Vector2 worldPos;
    /// <param name="pos">Grid position of the space.</param>
    /// <param name="worldPos">World position of the space.</param>
    public BoardSpace(Vector2Int pos, Vector2 worldPos)
    {
        this.pos = pos;
        this.worldPos = worldPos;
    }
}

/// <summary>
/// Represents a player starting space on the board perimeter.
/// Players are placed here at the start of the game.
/// </summary>
public class StartingSpace : BoardSpace
{
    public Vector2Int pos;
    public Vector2 worldPos;
    public StartingSpace(Vector2Int pos, Vector2 worldPos) : base(pos, worldPos)
    {
        this.pos = pos;
        this.worldPos = worldPos;
    }
}

/// <summary>
/// Represents a paired door connection between a hallway tile and a room tile.
/// </summary>
public class Door
{
    /// <summary>The hallway-side tile of this doorway.</summary>
    public DoorSpace doorSpace;
    /// <summary>The room-side tile of this doorway.</summary>
    public ExitSpace exitSpace;
    /// <param name="doorSpace">The hallway-side door space.</param>
    /// <param name="exitSpace">The room-side exit space.</param>
    public Door(DoorSpace doorSpace, ExitSpace exitSpace)
    {
        this.doorSpace = doorSpace;
        this.exitSpace = exitSpace;
    }
}
/// <summary>
/// Represents a hallway tile. May be a plain corridor or a doorway tile
/// adjacent to a room entrance.
/// </summary>
public class HallwaySpace : BoardSpace
{
    public bool isDoorway;
    public HallwaySpace(Vector2Int pos, Vector2 worldPos, bool isDoorway) : base(pos, worldPos)
    {
        this.isDoorway = isDoorway;
    }
}
/// <summary>
/// Represents the hallway-side tile of a doorway, associated with the room it leads to.
/// Players step onto this tile to initiate entry into the room.
/// </summary>
public class DoorSpace : BoardSpace
{
    public Room room;
    public DoorSpace(Vector2Int pos, Vector2 worldPos, Room room) : base(pos, worldPos)
    {
        this.pos = pos;
        this.worldPos = worldPos;
        this.room = room;
    }
}

/// <summary>
/// Represents the room-side tile of a doorway. Landing on this tile
/// places a player inside the associated room.
/// </summary>
public class ExitSpace : BoardSpace
{
    public Room room;
    public ExitSpace(Vector2Int pos, Vector2 worldPos, Room room) : base(pos, worldPos)
    {
        this.pos = pos;
        this.worldPos = worldPos;
        this.room = room;
    }
}

/// <summary>
/// Represents a corner shortcut space. Landing here allows a player to teleport
/// to the oppositeSpace in the diagonally opposite corner of the board.
/// </summary>
public class ShortcutSpace : BoardSpace
{
    public Room room;
    public Vector2Int oppositeSpace;
    public ShortcutSpace(Vector2Int pos, Vector2 worldPos, Room room, Vector2Int oppositeSpace) : base(pos, worldPos)
    {
        this.pos = pos;
        this.worldPos = worldPos;
        this.room = room;
        this.oppositeSpace = oppositeSpace;
    }
}

/// <summary>
/// Represents a tile inside a room. Tracks whether the tile is a doorway
/// and which room it belongs to.
/// </summary>
public class RoomSpace : BoardSpace
{
    public Room room; 
    public bool isDoorway;
    public RoomSpace(Vector2Int pos, Vector2 worldPos, Room room, bool isDoorway) : base(pos, worldPos)
    {
        this.room = room;
        this.isDoorway = isDoorway;
    }
}