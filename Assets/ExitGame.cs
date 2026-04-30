using UnityEngine;
using UnityEngine.InputSystem;

public class ExitGame : MonoBehaviour
{
    
    public InputActionReference escapeButton;
    public GameObject ui;

    void OnEnable()
    {
        escapeButton.action.Enable();
        escapeButton.action.performed += OnPressed;
    }

    void OnDisable()
    {
        escapeButton.action.performed -= OnPressed;
        escapeButton.action.Disable();
    }

    void OnPressed(InputAction.CallbackContext context)
    {
        ui.SetActive(!ui.activeSelf);
    }

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
