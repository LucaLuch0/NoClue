using TMPro;
using UnityEngine;

public class DisplayTextUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void setUp(string text)
    {
        this.text.text = text;
    }
    
    public void select()
    {
        DestroyImmediate(this.gameObject);

    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
