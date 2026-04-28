using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileClickHandler : MonoBehaviour
{

    public Tilemap tilemap;
    public MapGenerator mapGenerator;
    public TurnManager turnManager;

    public InputActionReference clickAction;
    public InputActionReference pointAction;

    private void OnEnable()
    {
        clickAction.action.performed += OnClick;
        clickAction.action.Enable();
        pointAction.action.Enable();
    }

    private void OnDisable()
    {
        clickAction.action.performed -= OnClick;
        clickAction.action.Disable();
        pointAction.action.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 screenPos = pointAction.action.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        TileBase tile = tilemap.GetTile(cellPos);

        if (tile != null)
        {
            Debug.Log("Tile Clicked: " + cellPos);
        }
        
        Vector2Int pos = new Vector2Int(cellPos.x, cellPos.y);
        
        // GET CURRENT PLAYERS TURN
        // CHECK MOVE IS VALID
        
        turnManager.currentPlayer().movePlayer(mapGenerator.getBoardSpace(pos));
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
