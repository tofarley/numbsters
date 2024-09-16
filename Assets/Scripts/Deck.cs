using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Deck : MonoBehaviour
{
    [SerializeField] private List<GameObject> cards = new List<GameObject>();

    [SerializeField] private GameObject mouthCard;

    public float cardSpacing = 2.0f; // Spacing between cards
    public float dealingDuration = 0.5f; // Time to deal all cards

    private int cardsDealt = 0;

    void Start()
    {
        DealCards();
    }

    void DealCards()
    {
        // Shuffle the cards
        cards = Shuffle(cards);

        // Insert the mouth card at a random position in the top 7 cards.
        cards.Insert(UnityEngine.Random.Range(0, 6), mouthCard);

        // Calculate the total width of all cards
        float totalWidth = cardSpacing * 6; // 5 spaces between 6 cards

        // Calculate the starting position (left-most card)
        Vector3 startPos = new Vector3(-totalWidth / 2, 0, 0);

        GameObject cardRow = GameObject.Find("CardRow"); // Find the parent object in the scene

        // Deal 7 cards
        for (int i = 0; i < 7; i++)
        {
            GameObject card = cards[i];
            cardsDealt++;

            Vector3 targetPosition = startPos + new Vector3(cardSpacing * i, 0, 0);
            InstantiateAndPositionCard(card, targetPosition, cardRow);
        }
    }

    IEnumerator MoveOverTime(GameObject card, Vector3 targetPosition, float duration)
    {

        float time = 0;
        Vector3 startPosition = card.transform.position;
        while (time < duration)
        {
            card.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        card.transform.position = targetPosition;
        yield return new WaitForSeconds(0.1f);

    }

    public void DrawCard()
    {
        GameObject cardRow = GameObject.Find("CardRow");

        float totalWidth = cardSpacing * cardRow.GetComponent<DraggableRow>().rowObjects.Count;

        // Calculate the starting position (left-most card)
        Vector3 startPos = new Vector3(-totalWidth / 2, 0, 0);

        GameObject card = cards[cardsDealt];
        cardsDealt++;

        Vector3 targetPosition = startPos + new Vector3(cardSpacing * cardRow.GetComponent<DraggableRow>().rowObjects.Count, 0, 0);
        InstantiateAndPositionCard(card, targetPosition, cardRow);

        cardRow.GetComponent<DraggableRow>().ArrangeObjects();
    }

    private void InstantiateAndPositionCard(GameObject card, Vector3 targetPosition, GameObject parent)
    {
        Vector3 offscreen = new Vector3(0, -10, 0);

        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 0;

        GameObject currentCard = Instantiate(card, offscreen, Quaternion.identity);
        currentCard.transform.SetParent(parent.transform);

        parent.GetComponent<DraggableRow>().rowObjects.Add(currentCard);

        StartCoroutine(MoveOverTime(currentCard, targetPosition, 0.5f));
    }
    public List<T> Shuffle<T>(List<T> list)
    {
        List<T> shuffledList = list.OrderBy(x => UnityEngine.Random.value).ToList();
        return shuffledList;
    }
}
