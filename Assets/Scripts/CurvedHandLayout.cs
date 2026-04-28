using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CurvedHandLayout : MonoBehaviour
{
    public GameObject cardPrefab;   // THIS CREATES THE SLOT
    public GameObject layoutObject;
    
    private List<GameObject> cardObjects = new List<GameObject>();

    public float cardSpacing = 100f;
    public float curveHeight = 20f;
    public float maxRotation = 12f;
    
    public InputActionReference spaceButtonAction;

    public void setup(List<Card> cards)
    {
        Debug.Log("Player cards: " + cards.Count);
        foreach (GameObject card in cardObjects)
        {
            Destroy(card);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, layoutObject.transform);
            newCard.GetComponentInChildren<TextMeshProUGUI>().text = cards[i].name;
            cardObjects.Add(newCard);
        }
        
        ArrangeCards();
    }

    void OnEnable()
    {
        spaceButtonAction.action.Enable();
        spaceButtonAction.action.performed += OnPressed;
    }

    void OnDisable()
    {
        spaceButtonAction.action.performed -= OnPressed;
        spaceButtonAction.action.Disable();
    }

    void OnPressed(InputAction.CallbackContext context)
    {
        layoutObject.SetActive(!layoutObject.activeSelf);
    }


    void ArrangeCards()
    {
        int count = layoutObject.transform.childCount;
        float centerIndex = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            RectTransform card = layoutObject.transform.GetChild(i).GetComponent<RectTransform>();

            float offset = i - centerIndex;
            float normalized = centerIndex == 0 ? 0 : offset / centerIndex;

            float x = offset * cardSpacing;
            float y = -(normalized * normalized) * curveHeight + curveHeight;
            float rotation = -normalized * maxRotation;

            card.anchoredPosition = new Vector2(x, y);
            card.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}