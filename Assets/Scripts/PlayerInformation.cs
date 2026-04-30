using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the player information UI panel, displaying each player's
/// name, character, and colour. The panel can be toggled with a button.
/// </summary>
public class PlayerInformation : MonoBehaviour
{
    /// <summary>Input action reference for toggling the player information panel.</summary>
    public InputActionReference pButton;

    /// <summary>The UI panel GameObject to show or hide.</summary>
    public GameObject ui;

    /// <summary>Reference to the TurnManager to access the list of active players.</summary>
    public TurnManager turnManager;

    /// <summary>The grid transform used as the parent for instantiated player info entries.</summary>
    public Transform grid;

    /// <summary>Prefab used to instantiate a text entry for each player.</summary>
    public GameObject textPrefab;

    /// <summary>
    /// Enables the toggle input action and subscribes to its performed event.
    /// </summary>
    void OnEnable()
    {
        pButton.action.Enable();
        pButton.action.performed += OnPressed;
    }

    /// <summary>
    /// Unsubscribes from the toggle input action and disables it.
    /// </summary>
    void OnDisable()
    {
        pButton.action.performed -= OnPressed;
        pButton.action.Disable();
    }

    /// <summary>
    /// Populates the player information panel with a coloured text entry
    /// for each active player, showing their name and assigned character.
    /// </summary>
    public void setUp()
    {
        List<Player> players = turnManager.activePlayers;

        foreach (Player p in players)
        {
            // Match the text colour to the player's token colour
            Color color = p.gameObject.GetComponent<SpriteRenderer>().color;

            GameObject textObject = Instantiate(textPrefab, grid);
            textObject.GetComponent<TextMeshProUGUI>().text = p.getName() + " | " + p.getCharacter();
            textObject.GetComponent<TextMeshProUGUI>().color = color;
        }
    }

    /// <summary>
    /// Toggles the visibility of the player information panel when the button is pressed.
    /// </summary>
    void OnPressed(InputAction.CallbackContext context)
    {
        ui.SetActive(!ui.activeSelf);
    }

    void Update()
    {
    }
}
