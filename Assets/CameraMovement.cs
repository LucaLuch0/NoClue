using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    
    public InputActionReference moveAction;
    public InputActionReference zoomAction;
    
    public float moveSpeed = 10f;
    public float zoomSpeed = 10f;
    public float minZoom = 1f;
    public float maxZoom = 20f;

    public Camera camera;
    void Update()
    {
        movement();
        zoom();
    }

    void movement()
    {
        Vector2 movement = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(movement.x, movement.y, 0f);
        camera.transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
    
    void zoom()
    {
        float zoom = zoomAction.action.ReadValue<float>();
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - zoom * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
    }
}
