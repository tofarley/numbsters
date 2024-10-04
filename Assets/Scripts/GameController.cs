using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NumbstersGameState;
using System.Runtime.CompilerServices;

public class GameController : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private DraggableRow draggableRow;
    [SerializeField] private Button drawButton;

    [SerializeField] private Button eatButton;
    private GameState gameState = GameState.CardDraw;

    void Start()
    {
        deck.DealCards();
        drawButton.interactable = true;
        eatButton.interactable = false;
    }

    private bool hasEaten = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch (gameState)
        {
            case GameState.CardDraw:
                if (deck.CardDrawn)
                {
                    drawButton.interactable = false;
                    gameState = GameState.Move;
                    draggableRow.gameState = GameState.Move;
                    hasEaten = false;
                    eatButton.interactable = true;
                }
                break;
            case GameState.Move:
                if(draggableRow.HasMovedOrSwappedCards)
                {
                    //drawButton.interactable = true;
                    draggableRow.HasMovedOrSwappedCards = false;
                    draggableRow.gameState = GameState.Eat;
                    gameState = GameState.Eat;
                    hasEaten = false;
                    eatButton.interactable = true;
                }
                break;
            case GameState.Eat:
                if(hasEaten)
                {
                    deck.CardDrawn = false;
                    eatButton.interactable = false;
                    
                    gameState = GameState.CardDraw;
                    draggableRow.gameState = GameState.CardDraw;
                    drawButton.interactable = true;
                }
                break;
        }
        // if (deck.CardDrawn && draggableRow.HasMovedOrSwappedCards)
        // {
        //     drawButton.interactable = false;
        //     draggableRow.HasMovedOrSwappedCards = false;
        //     //deck.CardDrawn = false;
        //     // Let's do other steps here.
        // 
        
    }



    public void Eat()
    {
        Debug.Log("Eat button clicked");
        // Ensure the mouth card index is valid
        if (draggableRow.MouthCardIndex > 0 && draggableRow.MouthCardIndex < draggableRow.rowObjects.Count - 1)
        {
            int firstCardIndex = draggableRow.MouthCardIndex - 1;
            int secondCardIndex = draggableRow.MouthCardIndex + 1;

            int firstCardValue = draggableRow.rowObjects[firstCardIndex].GetComponent<Card>().CardValue;
            int secondCardValue = draggableRow.rowObjects[secondCardIndex].GetComponent<Card>().CardValue;

            Debug.Log("Mouth card index: " + draggableRow.MouthCardIndex);
            Debug.Log("First card value: " + firstCardValue);
            Debug.Log("Second card value: " + secondCardValue);

            // Check if the cards are sequential
            if (Mathf.Abs(firstCardValue - secondCardValue) == 1)
            {
                if (firstCardValue < secondCardValue)
                {
                    // Remove the second card
                    GameObject secondCard = draggableRow.rowObjects[secondCardIndex];
                    draggableRow.rowObjects.RemoveAt(secondCardIndex);
                    StartCoroutine(AnimateCardOffScreen(secondCard)); // Animate the card off screen
                }
                else
                {
                    // Remove the first card
                    GameObject firstCard = draggableRow.rowObjects[firstCardIndex];
                    draggableRow.rowObjects.RemoveAt(firstCardIndex);
                    StartCoroutine(AnimateCardOffScreen(firstCard)); // Animate the card off screen
                }
                hasEaten = true;
            }
        }
    }

    private IEnumerator AnimateCardOffScreen(GameObject card)
    {
        float duration = 0.5f; // Duration of the animation
        float elapsedTime = 0.0f;
        Vector3 startPosition = card.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 10, 0); // Move upwards by 10 units

        while (elapsedTime < duration)
        {
            card.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.position = endPosition; // Ensure the card reaches the end position
        draggableRow.ArrangeObjects(); // Rearrange the remaining cards
        Destroy(card); // Optionally destroy the GameObject
    }
}



