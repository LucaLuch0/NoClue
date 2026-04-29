using TMPro;
using UnityEngine;

public class PassBackPlayerUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    private Suggestion suggestion;
    
    private Card card;
     
    public void setUp(Suggestion suggestion, string text, Card card)
    {
        this.suggestion = suggestion; 
        this.text.text = text;
        this.card = card;
    }

    public void select()
    {
        suggestion.cardSelected(card);
        DestroyImmediate(this.gameObject);
    }
}
