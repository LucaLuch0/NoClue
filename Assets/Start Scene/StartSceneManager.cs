using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StartSceneManager : MonoBehaviour
{

    // PLAYER NAME INPUT OBJECTS
    public GameObject inputGrid;
    public GameObject inputPrefab;
    
    public GameObject colourGrid;
    public GameObject colourPrefab;
    private int playerCount;

    // stack allows us to remove the last player added
    private Stack<GameObject> playerInputs;
    private Stack<GameObject> playerColours;

    public void addPlayer()
    {
        if (playerCount == 6) { return; } // max players (6)

        // creats a new player input object
        GameObject newPlayerInput = Instantiate(inputPrefab, inputGrid.transform);
        newPlayerInput.name = "Player " + (playerCount + 1) + "'s Name...";
        playerInputs.Push(newPlayerInput);
        addColourOption(playerCount);
        playerCount++;
    }
    
    public void addColourOption(int value)
    {
        GameObject newColourInput = Instantiate(colourPrefab, colourGrid.transform);
        newColourInput.GetComponent<TMP_Dropdown>().value = value;
        playerColours.Push(newColourInput);
    }

    public void removePlayer()
    {
        if (playerCount == 0) { return; } // no players (0)

        Destroy(playerInputs.Pop());
        Destroy(playerColours.Pop());
        playerCount--;
    }

    private List<String> getPlayerNames()
    {
        List<String> playerNames = new List<String>();

        Debug.Log("Player Count: " + playerInputs.Count);
        int count = 0;
        foreach (GameObject playerInput in playerInputs)
        {
            String name = playerInput.GetComponent<TMP_InputField>().text;
            if(name == "")
            {
                name = "Player " + count;
            }
            playerNames.Add(name);
            count++;
        }

        return playerNames;
    }
    
    private List<Color> getColours()
    {
        List<Color> colours = new List<Color>();

        Debug.Log("Player Count: " + playerInputs.Count);
        foreach (GameObject playerColour in playerColours)
        {
            Debug.Log("DEBUG COLOURS");
            TMP_Dropdown dropdown = playerColour.GetComponent<TMP_Dropdown>();
            int colour = dropdown.value;
            
            switch (colour)
            {
                case 0:
                    colours.Add(Color.red);
                    break;
                case 1:
                    colours.Add(Color.blue);
                    break;
                case 2:
                    colours.Add(Color.green);
                    break;
                case 3:
                    colours.Add(Color.magenta);
                    break;
                case 4:
                    colours.Add(Color.yellow);
                    break;
                case 5:
                    colours.Add(Color.cyan);
                    break;
                default:
                    Debug.Log("COLOUR NOT FOUND");
                    break;
            }
        }
        
        return colours;
    }

    public void startGame()
    {
        PlayerManager.playerNames = getPlayerNames();
        PlayerManager.playerColours = getColours();
        
        SceneManager.LoadScene("MainScene");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInputs = new Stack<GameObject>();
        playerColours = new Stack<GameObject>();
        playerCount = 0;
        addPlayer();
        addPlayer();
        addPlayer();
        addPlayer();
        addPlayer();
        addPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }
}