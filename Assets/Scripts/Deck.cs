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
        cards.Insert(UnityEngine.Random.Range(0, 6), mouthCard);

        // Calculate the total width of all cards
        float totalWidth = cardSpacing * 6; // 5 spaces between 6 cards

        // Calculate the starting position (left-most card)
        Vector3 startPos = new Vector3(-totalWidth / 2, 0, 0);

        Vector3 offscreen = new Vector3(0, -10, 0);

        GameObject cardRow = GameObject.Find("CardRow"); // Find the parent object in the scene

        // Deal 6 cards
        for (int i = 0; i < 7; i++)
        {

            GameObject card = cards[i];
            cardsDealt++;

            SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
            // float spriteWidth = spriteRenderer.bounds.size.x;

            spriteRenderer.sortingOrder = 0;

            Vector3 targetPosition = startPos + new Vector3(cardSpacing * i, 0, 0);

            GameObject currentCard = Instantiate(card, offscreen, Quaternion.identity);

            currentCard.transform.SetParent(cardRow.transform);
            // Set the initial position below the screen
            //card.transform.position = targetPosition - new Vector3(0, 10, 0);

            cardRow.GetComponent<DraggableRow>().rowObjects.Add(currentCard);
            // Animate the card to its final position
            StartCoroutine(MoveOverTime(currentCard, targetPosition, 0.5f));
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
        // GameObject cardRow = GameObject.Find("CardRow");
        // cardRow.GetComponent<DraggableRow>().PopulateRow();

    }

    public void DrawCard()
    {
        GameObject cardRow = GameObject.Find("CardRow");


        float totalWidth = cardSpacing * cardRow.GetComponent<DraggableRow>().rowObjects.Count; // 5 spaces between 6 cards

        // Calculate the starting position (left-most card)
        Vector3 startPos = new Vector3(-totalWidth / 2, 0, 0);

        Vector3 offscreen = new Vector3(0, -10, 0);
        // Draw a card from the deck

        GameObject card = cards[cardsDealt];
        cardsDealt++;
        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        // float spriteWidth = spriteRenderer.bounds.size.x;

        spriteRenderer.sortingOrder = 0;

        Vector3 targetPosition = startPos + new Vector3(cardSpacing * cardRow.GetComponent<DraggableRow>().rowObjects.Count, 0, 0);

        GameObject currentCard = Instantiate(card, offscreen, Quaternion.identity);

        currentCard.transform.SetParent(cardRow.transform);
        // Set the initial position below the screen
        //card.transform.position = targetPosition - new Vector3(0, 10, 0);
        //cardsInPlay.Add(currentCard);

        cardRow.GetComponent<DraggableRow>().rowObjects.Add(currentCard);
        // Animate the card to its final position
        StartCoroutine(MoveOverTime(currentCard, targetPosition, 0.5f));
        cardRow.GetComponent<DraggableRow>().ArrangeObjects();
    }

    public List<T> Shuffle<T>(List<T> list)
    {
        List<T> shuffledList = list.OrderBy(x => UnityEngine.Random.value).ToList();
        // Insert the mouth card at a random position in the top six cards.
        return shuffledList;
    }
}
