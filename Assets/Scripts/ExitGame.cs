using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the in-game exit/pause functionality, toggling a UI panel
/// when the escape button is pressed and quitting the application on demand.
/// </summary>
public class ExitGame : MonoBehaviour
{
    /// <summary>Input action reference for the escape/pause button.</summary>
    public InputActionReference escapeButton;

    /// <summary>The UI panel to toggle when the escape button is pressed (e.g. a pause menu).</summary>
    public GameObject ui;

    /// <summary>
    /// Enables the escape input action and subscribes to its performed event.
    /// </summary>
    void OnEnable()
    {
        escapeButton.action.Enable();
        escapeButton.action.performed += OnPressed;
    }

    /// <summary>
    /// Unsubscribes from the escape input action and disables it.
    /// </summary>
    void OnDisable()
    {
        escapeButton.action.performed -= OnPressed;
        escapeButton.action.Disable();
    }

    /// <summary>
    /// Toggles the visibility of the exit UI panel when the escape button is pressed.
    /// </summary>
    void OnPressed(InputAction.CallbackContext context)
    {
        ui.SetActive(!ui.activeSelf);
    }

    /// <summary>
    /// Quits the application. Intended to be called from a button in the exit UI.
    /// </summary>
    public void exit()
    {
        Application.Quit();
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
