using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using Unity.VisualScripting;

public class MapGenerator : MonoBehaviour
{
    [Header("Board Dimensions")]
    public int width = 24;
    public int height = 25;

    [Header("Base Tiles")]
    public Tilemap visualTilemap;
    public TileBase hallwayBase;
    public float tileWidth;
    
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

    [Header("Hallway Door Tiles")]
    public TileBase hallwayDoorTop;
    public TileBase hallwayDoorBottom;
    public TileBase hallwayDoorLeft;
    public TileBase hallwayDoorRight;

    [Header("Room Door Tiles")]
    public TileBase roomDoorTop;
    public TileBase roomDoorBottom;
    public TileBase roomDoorLeft;
    public TileBase roomDoorRight;

    [Header("UI Labels")]
    public GameObject roomLabelPrefab;

    public enum DoorDirection { Top, Bottom, Left, Right }
    private Dictionary<Vector2Int, DoorDirection> doorRegistry = new Dictionary<Vector2Int, DoorDirection>();
    public List<Door> doors = new List<Door>();
    private List<RectInt> allRooms = new List<RectInt>();
    private List<Room> rooms = new List<Room>();
    
    public List<StartingSpace> startingSpaces = new List<StartingSpace>();
    public List<RoomSpace> roomSpaces = new List<RoomSpace>();
    public List<HallwaySpace> hallwaySpaces = new List<HallwaySpace>();
    public List<DoorSpace> doorSpaces = new List<DoorSpace>();
    public List<ExitSpace> exitSpaces = new List<ExitSpace>();

    public BoardSpace getBoardSpace(Vector2Int cellPos)
    {
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
    
    public GameManager gameManager;

    public List<Vector2Int> getDoors()
    {
        List<Vector2Int> doors = new List<Vector2Int>();
        foreach (Vector2Int pos in doorRegistry.Keys)
        {
            doors.Add(pos);
        }

        return doors;
    }

    void Start()
    {
        PlayerManager.mapGenerator = this;
        
        tileWidth = visualTilemap.cellSize.x;
        
        // Define Rooms
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
        
        
        // Starting Locations
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
        
        gameManager.setup();
    }

    // CAN BE DELETED
    void DebugRooms()
    {
        foreach (Room room in rooms)
        {
            room.debug();
        }
    }

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
            room.addRoomTile(roomSpace);
            
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
            room.addRoomTile(roomSpace);
        }
        
        visualTilemap.SetTile((Vector3Int)pos, selectedTile);
        
    }

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

public abstract class BoardSpace { 
    
    public Vector2Int pos;
    public Vector2 worldPos;
    public BoardSpace(Vector2Int pos, Vector2 worldPos)
    {
        this.pos = pos;
        this.worldPos = worldPos;
    }
}

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

public class Door
{
    public DoorSpace doorSpace;
    public ExitSpace exitSpace;

    public Door(DoorSpace doorSpace, ExitSpace exitSpace)
    {
        this.doorSpace = doorSpace;
        this.exitSpace = exitSpace;
    }
}

public class HallwaySpace : BoardSpace
{
    public bool isDoorway;
    public HallwaySpace(Vector2Int pos, Vector2 worldPos, bool isDoorway) : base(pos, worldPos)
    {
        this.isDoorway = isDoorway;
    }
}

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