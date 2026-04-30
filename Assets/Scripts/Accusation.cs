using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the accusation mechanic, allowing the current player to
/// make a final accusation against a suspect, weapon, and room.
/// </summary>
public class Accusation : MonoBehaviour
{
    /// <summary>The currently active accusation UI instance.</summary>
    private GameObject accusationUI;

    /// <summary>Prefab used to instantiate the accusation selection UI.</summary>
    public GameObject accusationUIPrefab;

    /// <summary>Prefab used to instantiate a message/feedback UI.</summary>
    public GameObject messageUIPrefab;

    /// <summary>Reference to the TurnManager to identify the current player.</summary>
    public TurnManager turnManager;

    /// <summary>Reference to the MurderCardSelect holding the correct murder cards.</summary>
    public MurderCardSelect murderCardSelect;

    /// <summary>
    /// Opens the accusation UI for the current player, provided they
    /// have not already made an accusation this game.
    /// </summary>
    public void setup()
    {
        // Prevent a player from accusing more than once
        if (turnManager.currentPlayer().guessed == true)
        {
            return;
        }

        accusationUI = Instantiate(accusationUIPrefab, transform);
        accusationUI.GetComponent<AccusationUI>().setUp(this, CardManager.weapons, CardManager.characters, CardManager.rooms);
    }

    /// <summary>
    /// Processes the player's accusation. If the accused weapon, character,
    /// and room all match the murder cards, the game ends with a win.
    /// Otherwise, the player is shown a failure message and marked as
    /// having used their accusation.
    /// </summary>
    /// <param name="weapon">The weapon card the player is accusing.</param>
    /// <param name="character">The suspect card the player is accusing.</param>
    /// <param name="room">The room card the player is accusing.</param>
    public void accuse(Card weapon, Card character, Card room)
    {
        if (weapon == murderCardSelect.murderWeapon && character == murderCardSelect.murderSuspect &&
            room == murderCardSelect.murderRoom)
        {
            // Correct accusation — load the end game scene
            SceneManager.LoadScene("EndGame");
        }
        else
        {
            // Wrong accusation — notify the player and prevent further accusations
            GameObject messageUI = Instantiate(messageUIPrefab, transform);
            messageUI.GetComponent<DisplayTextUI>().setUp("Wrong Accusation. You can no longer accuse anyone!");
            turnManager.currentPlayer().guessed = true;
        }
    }
}
