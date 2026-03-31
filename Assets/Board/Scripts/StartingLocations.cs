using System.Collections.Generic;
using UnityEngine;

public class StartingLocations : MonoBehaviour
{
    private Dictionary<Player, Character> playerCharacters;
    
    private Dictionary<Character, BoardSpace> characterStartingLocations;
    public StartingLocations(Dictionary<Player, Character> playerCharacters, Dictionary<Character, BoardSpace> characterStartingLocations)
    {
        this.playerCharacters = playerCharacters;
        this.characterStartingLocations = characterStartingLocations;
    }

    public void assign()
    {
        if (playerCharacters == null) { return; }

        if (characterStartingLocations == null) { return; }

        foreach (var item in playerCharacters)
        {
            //item.Key.gameObject.transform.SetParent(characterStartingLocations[item.Value].getGameObject().transform);
            //item.Key.gameObject.transform.position = characterStartingLocations[item.Value].getGameObject().transform.position;

        }
    }
}

// REMOVE - PLACEHOLDER
public class Character
{
    
}