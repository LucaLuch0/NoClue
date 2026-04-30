using UnityEngine;

/// <summary>
/// Handles the UI displayed when a player has no cards to show.
/// </summary>
public class NoCardsUI : MonoBehaviour
{
    
    
    /// <summary>
    /// Called when the player confirms/selects this UI.
    /// Immediately destroys the UI GameObject to dismiss it.
    /// </summary>
    public void select()
    {
        DestroyImmediate(this.gameObject);
    }
    
}
