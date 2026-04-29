using UnityEngine;
using UnityEngine.SceneManagement;

public class Accusation : MonoBehaviour
{

    public GameObject accusationUIPrefab;
    private GameObject accusationUI;
    public GameObject messageUIPrefab;

    public TurnManager turnManager;

    public MurderCardSelect murderCardSelect;
    
    public void setup()
    {
        if (turnManager.currentPlayer().guessed == true)
        {
            return;
        }
        
        accusationUI =  Instantiate(accusationUIPrefab, transform);
        accusationUI.GetComponent<AccusationUI>().setUp(this, CardManager.weapons, CardManager.characters, CardManager.rooms);
    }

    public void accuse(Card weapon, Card character, Card room)
    {
        if (weapon == murderCardSelect.murderWeapon && character == murderCardSelect.murderSuspect &&
            room == murderCardSelect.murderRoom)
        {
            SceneManager.LoadScene("EndGame");

        }
        else
        {
            GameObject messageUI = Instantiate(messageUIPrefab, transform);
            messageUI.GetComponent<DisplayTextUI>().setUp("Wrong Accusation.");
            turnManager.currentPlayer().guessed = true;
        }
    }
}
