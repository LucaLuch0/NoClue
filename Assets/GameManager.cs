using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public MapGenerator mapGenerator;
    
    public GameObject playerPrefab;
    
    public TurnManager turnManager;

    public MurderCardSelect murderCardSelect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void setup()
    {
        List<Player> players = new List<Player>();
        List<String> characterNames = new List<String>();
        
        characterNames.Add("Miss Scarlett");
        characterNames.Add("Colonel Mustard");
        characterNames.Add("Mrs. White");
        characterNames.Add("Mr. Green");
        characterNames.Add("Mrs. Peacock");   //// PLACEHOLDER
        characterNames.Add("Professor Plum");

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
        
        Debug.Log(PlayerManager.playerColours.Count + " coloursss");

        int count = 0;
        foreach (Color color in PlayerManager.playerColours)
        {
            players[count].GetComponent<SpriteRenderer>().color = color;
            count++;
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
        
        murderCardSelect.setUp();
        
        Debug.Log("TURN MANAGER CREATED");
        turnManager.activePlayers = (players);
        turnManager.InitializeTurnOrder();

    }

    // Update is called once per frame
    void Update()
    {
    }
}
