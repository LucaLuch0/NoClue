using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInformation : MonoBehaviour
{
    public InputActionReference pButton;
    public GameObject ui;
    public TurnManager turnManager;
    public Transform grid;
    public GameObject textPrefab;

    void OnEnable()
    {
        pButton.action.Enable();
        pButton.action.performed += OnPressed;
    }

    void OnDisable()
    {
        pButton.action.performed -= OnPressed;
        pButton.action.Disable();
    }

    public void setUp()
    {
        List<Player> players = turnManager.activePlayers;
        string message = "";
        foreach (Player p in players)
        {
            Color color = p.gameObject.GetComponent<SpriteRenderer>().color;
            GameObject textObject = Instantiate(textPrefab, grid);
            textObject.GetComponent<TextMeshProUGUI>().text = p.getName() + " | " + p.getCharacter();
            textObject.GetComponent<TextMeshProUGUI>().color = color;
        }
    }
    void OnPressed(InputAction.CallbackContext context)
    {
        ui.SetActive(!ui.activeSelf);
    }

    void Update()
    {
        
    }
}
