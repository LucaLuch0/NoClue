using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles mouse/pointer click input on the tilemap and translates
/// clicked tile positions into player movement commands.
/// </summary>
public class TileClickHandler : MonoBehaviour
{
    /// <summary>The tilemap used to convert world positions to cell coordinates.</summary>
    public Tilemap tilemap;

    /// <summary>Reference to the map generator, used to retrieve board spaces by position.</summary>
    public MapGenerator mapGenerator;

    /// <summary>Reference to the TurnManager to retrieve the current player.</summary>
    public TurnManager turnManager;

    /// <summary>Input action for detecting a click/tap.</summary>
    public InputActionReference clickAction;

    /// <summary>Input action for reading the pointer's screen position.</summary>
    public InputActionReference pointAction;

    /// <summary>
    /// Enables input actions and subscribes to the click event.
    /// </summary>
    private void OnEnable()
    {
        clickAction.action.performed += OnClick;
        clickAction.action.Enable();
        pointAction.action.Enable();
    }

    /// <summary>
    /// Disables input actions and unsubscribes from the click event.
    /// </summary>
    private void OnDisable()
    {
        clickAction.action.performed -= OnClick;
        clickAction.action.Disable();
        pointAction.action.Disable();
    }

    /// <summary>
    /// Called when a click input is performed. Converts the pointer's screen position
    /// to a tilemap cell position, then attempts to move the current player to that tile.
    /// </summary>
    private void OnClick(InputAction.CallbackContext context)
    {
        // Read the current pointer position in screen space
        Vector2 screenPos = pointAction.action.ReadValue<Vector2>();

        // Convert screen position to world position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        // Convert world position to tilemap cell coordinates
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        TileBase tile = tilemap.GetTile(cellPos);

        if (tile != null)
        {
            Debug.Log("Tile Clicked: " + cellPos);
        }

        Vector2Int pos = new Vector2Int(cellPos.x, cellPos.y);

        // Attempt to move the current player to the clicked board space
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
