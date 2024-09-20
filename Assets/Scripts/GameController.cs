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
        switch(gameState)
        {
            case GameState.CardDraw:
                if (deck.CardDrawn)
                {
                    drawButton.interactable = false;
                    gameState = GameState.Move;
                    draggableRow.gameState = GameState.Move;

                    eatButton.interactable = false;
                }
                break;
            case GameState.Move:
                if(draggableRow.HasMovedOrSwappedCards)
                {
                    //drawButton.interactable = true;
                    draggableRow.HasMovedOrSwappedCards = false;
                    draggableRow.gameState = GameState.Eat;
                    gameState = GameState.Eat;
                    eatButton.interactable = true;
                }
                break;
            case GameState.Eat:
                if(hasEaten)
                {
                    eatButton.interactable = false;
                    hasEaten = false;
                    gameState = GameState.CardDraw;
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
        // if (draggableRow.MouthCardIndex > 0 && draggableRow.MouthCardIndex < draggableRow.rowObjects.Count - 1)
        // {
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
                    Destroy(secondCard); // Optionally destroy the GameObject
                }
                else
                {
                    // Remove the first card
                    GameObject firstCard = draggableRow.rowObjects[firstCardIndex];
                    draggableRow.rowObjects.RemoveAt(firstCardIndex);
                    Destroy(firstCard); // Optionally destroy the GameObject
                }
                hasEaten = true;
            // }
        }
    }
}



