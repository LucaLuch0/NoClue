using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public MapGenerator mapGenerator;
    
    public GameObject playerPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void setup()
    {
        List<Player> players = new List<Player>();
        List<String> characterNames = new List<String>();
        characterNames.Add("1");
        characterNames.Add("2");
        characterNames.Add("3");
        characterNames.Add("4");
        characterNames.Add("5");   //// PLACEHOLDER
        characterNames.Add("6");

        foreach (String name in PlayerManager.playerNames)
        {
            Debug.Log("Player name count: " +  characterNames.Count);
            int random = Random.Range(0, characterNames.Count);
            string character = characterNames[random];
            characterNames.RemoveAt(random);

            
            GameObject playerObject = Instantiate(playerPrefab);
            Player newPlayer = playerObject.GetComponent<Player>();
            newPlayer.Initialize(name, character);
            players.Add(newPlayer);
            newPlayer.gameObject = playerObject;
        }

        PlayerManager.addPlayers(players);
        
        Debug.Log(PlayerManager.getPlayers());
        Debug.Log(mapGenerator.startingSpaces.Count );
        Debug.Log("Players set to start");
        Stack<StartingSpace> startingSpaces = new Stack<StartingSpace>();

        foreach (StartingSpace startingSpace in mapGenerator.startingSpaces)
        {  
            startingSpaces.Push(startingSpace);
        }
        
        foreach(Player player in PlayerManager.getPlayers())
        {
            player.setPosition(startingSpaces.Pop());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
