using UnityEngine;

public class CurvedHandLayout : MonoBehaviour
{
    public GameObject cardPrefab;   // THIS CREATES THE SLOT
    public int numberOfCards = 7;

    public float cardSpacing = 150f;
    public float curveHeight = 40f;
    public float maxRotation = 12f;

    void Start()
    {
        CreateCards();
        ArrangeCards();
    }

    void CreateCards()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, transform);
            newCard.name = "Card_" + i;
        }
    }

    void ArrangeCards()
    {
        int count = transform.childCount;
        float centerIndex = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            RectTransform card = transform.GetChild(i).GetComponent<RectTransform>();

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