using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the curved hand layout UI, displaying the current player's cards
/// in an arc arrangement. The hand can be toggled on and off with a button.
/// </summary>
public class CurvedHandLayout : MonoBehaviour
{
    /// <summary>Prefab used to instantiate each card slot in the hand.</summary>
    public GameObject cardPrefab;

    /// <summary>The parent GameObject that holds all instantiated card slots.</summary>
    public GameObject layoutObject;

    /// <summary>The list of currently instantiated card GameObjects.</summary>
    private List<GameObject> cardObjects = new List<GameObject>();

    /// <summary>The horizontal spacing between cards in the hand.</summary>
    public float cardSpacing = 100f;

    /// <summary>The height of the arc curve applied to the card layout.</summary>
    public float curveHeight = 20f;

    /// <summary>The maximum rotation (in degrees) applied to cards at the edges of the hand.</summary>
    public float maxRotation = 12f;

    /// <summary>Input action reference for toggling the hand layout visibility.</summary>
    public InputActionReference spaceButtonAction;

    /// <summary>
    /// Clears the existing hand and instantiates new card slots for the given list of cards,
    /// then arranges them in the curved layout.
    /// </summary>
    /// <param name="cards">The list of cards to display in the hand.</param>
    public void setup(List<Card> cards)
    {
        Debug.Log("Player cards: " + cards.Count);

        // Destroy all existing card objects before rebuilding the hand
        foreach (GameObject card in cardObjects)
        {
            DestroyImmediate(card);
        }
        cardObjects.Clear();

        // Instantiate a card slot for each card and set its label
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, layoutObject.transform);
            newCard.GetComponentInChildren<TextMeshProUGUI>().text = cards[i].name;
            cardObjects.Add(newCard);
        }

        ArrangeCards();
    }

    /// <summary>
    /// Enables the toggle input action and subscribes to its performed event.
    /// </summary>
    void OnEnable()
    {
        spaceButtonAction.action.Enable();
        spaceButtonAction.action.performed += OnPressed;
    }

    /// <summary>
    /// Unsubscribes from the toggle input action and disables it.
    /// </summary>
    void OnDisable()
    {
        spaceButtonAction.action.performed -= OnPressed;
        spaceButtonAction.action.Disable();
    }

    /// <summary>
    /// Toggles the visibility of the hand layout when the assigned button is pressed.
    /// </summary>
    void OnPressed(InputAction.CallbackContext context)
    {
        layoutObject.SetActive(!layoutObject.activeSelf);
    }

    /// <summary>
    /// Positions and rotates each card in the layout to form a curved arc.
    /// Cards are spread horizontally, raised at the centre, and rotated
    /// outward from the middle to simulate a hand of cards.
    /// </summary>
    void ArrangeCards()
    {
        int count = layoutObject.transform.childCount;
        float centerIndex = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            RectTransform card = layoutObject.transform.GetChild(i).GetComponent<RectTransform>();

            float offset = i - centerIndex;

            // Normalise offset to range [-1, 1] relative to the centre
            float normalized = centerIndex == 0 ? 0 : offset / centerIndex;

            // Calculate position: spread horizontally, arc upward at centre
            float x = offset * cardSpacing;
            float y = -(normalized * normalized) * curveHeight + curveHeight;

            // Rotate cards outward from the centre
            float rotation = -normalized * maxRotation;

            card.anchoredPosition = new Vector2(x, y);
            card.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}