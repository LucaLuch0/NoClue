using TMPro;
using UnityEngine;

/// <summary>
/// A generic UI component for displaying a text message to the player,
/// with a confirmation button to dismiss it.
/// </summary>
public class DisplayTextUI : MonoBehaviour
{
    /// <summary>The text element used to display the message.</summary>
    public TextMeshProUGUI text;

    /// <summary>
    /// Sets the displayed text to the given string.
    /// </summary>
    /// <param name="text">The message to display.</param>
    public void setUp(string text)
    {
        this.text.text = text;
    }

    /// <summary>
    /// Called when the player dismisses the message.
    /// Immediately destroys this UI GameObject.
    /// </summary>
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
