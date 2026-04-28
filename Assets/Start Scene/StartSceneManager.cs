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
    private int playerCount;

    // stack allows us to remove the last player added
    private Stack<GameObject> playerInputs;

    public void addPlayer()
    {
        if (playerCount == 6) { return; } // max players (6)

        // creats a new player input object
        GameObject newPlayerInput = Instantiate(inputPrefab, inputGrid.transform);
        newPlayerInput.name = "Player " + (playerCount + 1) + "'s Name...";
        playerInputs.Push(newPlayerInput);
        playerCount++;
    }

    public void removePlayer()
    {
        if (playerCount == 0) { return; } // no players (0)

        Destroy(playerInputs.Pop());
        playerCount--;
    }

    private List<String> getPlayerNames()
    {
        List<String> playerNames = new List<String>();

        Debug.Log("Player Count: " + playerInputs.Count);
        foreach (GameObject playerInput in playerInputs)
        {
            playerNames.Add(playerInput.GetComponent<TMP_InputField>().text);
        }

        return playerNames;
    }

    public void startGame()
    {
        PlayerManager.playerNames = getPlayerNames();
        
        SceneManager.LoadScene("MainScene");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInputs = new Stack<GameObject>();
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