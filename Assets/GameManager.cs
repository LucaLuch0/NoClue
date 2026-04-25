using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    ///  TEST FOR PLAYERS INTERACTING WITH ROOMS
    /// </summary>
    
    public GameObject playerPrefab;
    
    public Stack<GameObject> players;
    
    public MapGenerator mapGenerator;

    public bool test = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = new Stack<GameObject>();
        players.Push(Instantiate(playerPrefab));
        players.Push(Instantiate(playerPrefab));
        players.Push(Instantiate(playerPrefab));
        players.Push(Instantiate(playerPrefab));
        players.Push(Instantiate(playerPrefab));
        players.Push(Instantiate(playerPrefab));
        players.Push(Instantiate(playerPrefab));
        
    }

    // Update is called once per frame
    void Update()
    {
        if (test) { return; }
        
        if(mapGenerator.GetRoom("DINING ROOM") == null) { return; }

        mapGenerator.GetRoom("DINING ROOM").addPlayer(players.Pop().GetComponent<Player>(), mapGenerator.tileWidth / 2f);
        mapGenerator.GetRoom("DINING ROOM").addPlayer(players.Pop().GetComponent<Player>(), mapGenerator.tileWidth / 2f);
        mapGenerator.GetRoom("DINING ROOM").addPlayer(players.Pop().GetComponent<Player>(), mapGenerator.tileWidth / 2f);
        mapGenerator.GetRoom("DINING ROOM").addPlayer(players.Pop().GetComponent<Player>(), mapGenerator.tileWidth / 2f);
        mapGenerator.GetRoom("DINING ROOM").addPlayer(players.Pop().GetComponent<Player>(), mapGenerator.tileWidth / 2f);
        mapGenerator.GetRoom("DINING ROOM").addPlayer(players.Pop().GetComponent<Player>(), mapGenerator.tileWidth / 2f);

        
        test = true;

    }
}
