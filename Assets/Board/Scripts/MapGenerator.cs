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
    private List<RectInt> allRooms = new List<RectInt>();
    private List<Room> rooms = new List<Room>();
    
    private List<RoomSpace> roomSpaces = new List<RoomSpace>();
    private List<HallwaySpace> hallwaySpaces = new List<HallwaySpace>();

    void Start()
    {
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
        
        // Define Doors

        // Lounge
        RegisterDoor(4, 5, DoorDirection.Top); 
        RegisterDoor(4, 6, DoorDirection.Bottom); 

        // Dining Room
        RegisterDoor(6, 8, DoorDirection.Top);
        RegisterDoor(6, 9, DoorDirection.Bottom);

        RegisterDoor(7, 12, DoorDirection.Right);
        RegisterDoor(8, 12, DoorDirection.Left);
        
        // Library
        RegisterDoor(17, 10, DoorDirection.Right);
        RegisterDoor(18, 10, DoorDirection.Left);

        RegisterDoor(20, 12, DoorDirection.Top);
        RegisterDoor(20, 13, DoorDirection.Bottom);

        // Ball Room
        RegisterDoor(13, 17, DoorDirection.Top);
        RegisterDoor(13, 18, DoorDirection.Bottom);

        RegisterDoor(14, 20, DoorDirection.Right);
        RegisterDoor(15, 20, DoorDirection.Left);

        RegisterDoor(10, 17, DoorDirection.Top);
        RegisterDoor(10, 18, DoorDirection.Bottom);

        RegisterDoor(8, 20, DoorDirection.Right);
        RegisterDoor(9, 20, DoorDirection.Left);

        // Kitchen
        RegisterDoor(5, 19, DoorDirection.Bottom);
        RegisterDoor(5, 18, DoorDirection.Top);

        // Conservatory
        RegisterDoor(18, 20, DoorDirection.Top);
        RegisterDoor(18, 21, DoorDirection.Bottom);

        // Billiard Room
        RegisterDoor(16, 17, DoorDirection.Right);
        RegisterDoor(17, 17, DoorDirection.Left);

        RegisterDoor(22, 13, DoorDirection.Top);
        RegisterDoor(22, 14, DoorDirection.Bottom);

        // Hall
        RegisterDoor(11, 6, DoorDirection.Top);
        RegisterDoor(11, 7, DoorDirection.Bottom);
        RegisterDoor(12, 6, DoorDirection.Top);
        RegisterDoor(12, 7, DoorDirection.Bottom);

        RegisterDoor(15, 3, DoorDirection.Right);
        RegisterDoor(16, 3, DoorDirection.Left);

        // Study
        RegisterDoor(19, 4, DoorDirection.Top);
        RegisterDoor(19, 5, DoorDirection.Bottom);

        
        GenerateCluedoBoard();
        LabelRooms();
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

    void RegisterDoor(int x, int y, DoorDirection dir)
    {
        doorRegistry[new Vector2Int(x, y)] = dir;
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

public class HallwaySpace : BoardSpace
{
    public bool isDoorway;
    public HallwaySpace(Vector2Int pos, Vector2 worldPos, bool isDoorway) : base(pos, worldPos)
    {
        this.isDoorway = isDoorway;
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