using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles camera panning and zooming based on player input actions.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    /// <summary>Input action for moving the camera (e.g. WASD or arrow keys).</summary>
    public InputActionReference moveAction;

    /// <summary>Input action for zooming the camera (e.g. scroll wheel).</summary>
    public InputActionReference zoomAction;

    /// <summary>Speed at which the camera pans across the map.</summary>
    public float moveSpeed = 10f;

    /// <summary>Speed at which the camera zooms in and out.</summary>
    public float zoomSpeed = 10f;

    /// <summary>The minimum orthographic size the camera can zoom in to.</summary>
    public float minZoom = 1f;

    /// <summary>The maximum orthographic size the camera can zoom out to.</summary>
    public float maxZoom = 20f;

    /// <summary>The camera to apply movement and zoom to.</summary>
    public Camera camera;

    /// <summary>
    /// Reads input and applies movement and zoom each frame.
    /// </summary>
    void Update()
    {
        movement();
        zoom();
    }

    /// <summary>
    /// Reads the movement input and translates the camera accordingly,
    /// scaled by move speed and delta time.
    /// </summary>
    void movement()
    {
        Vector2 movement = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(movement.x, movement.y, 0f);
        camera.transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Reads the zoom input and adjusts the camera's orthographic size,
    /// clamped between the minimum and maximum zoom values.
    /// </summary>
    void zoom()
    {
        float zoom = zoomAction.action.ReadValue<float>();
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - zoom * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
    }
}
