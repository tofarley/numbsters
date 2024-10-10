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

    [SerializeField] private Button eatFromTopButton;

    private GameState gameState = GameState.CardDraw;

    void Start()
    {
        deck.DealCards();
        drawButton.interactable = true;
        eatButton.interactable = false;
        eatFromTopButton.interactable = false;
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
                    eatFromTopButton.interactable = true;
                }
                break;
            case GameState.Move:
                if(draggableRow.HasMovedOrSwappedCards)
                {
                    //drawButton.interactable = true;
                    draggableRow.HasMovedOrSwappedCards = false;
                    draggableRow.gameState = GameState.Eat;
                    gameState = GameState.Eat;
                    // If I was in a position where I didn't have to move, I didn't properly reset the game state
                    //hasEaten = false;
                    // If I start a right-click and cancel it, it skips my chance to swap.
                    eatButton.interactable = true;
                    eatFromTopButton.interactable = true;
                }
                break;
            case GameState.Eat:
                if(hasEaten)
                {
                    deck.CardDrawn = false;
                    eatButton.interactable = false;
                    eatFromTopButton.interactable = false;
                    
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
        //Debug.Log("Eat button clicked");
        // Ensure the mouth card index is valid
        if (draggableRow.MouthCardIndex > 0 && draggableRow.MouthCardIndex < draggableRow.rowObjects.Count - 1)
        {
            int firstCardIndex = draggableRow.MouthCardIndex - 1;
            int secondCardIndex = draggableRow.MouthCardIndex + 1;

            int firstCardValue = draggableRow.rowObjects[firstCardIndex].GetComponent<Card>().CardValue;
            int secondCardValue = draggableRow.rowObjects[secondCardIndex].GetComponent<Card>().CardValue;

            //Debug.Log("Mouth card index: " + draggableRow.MouthCardIndex);
            //Debug.Log("First card value: " + firstCardValue);
            //Debug.Log("Second card value: " + secondCardValue);

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
                gameState = GameState.CardDraw;
            }
        }
    }

    public void EatFromTop() {
        if (draggableRow.MouthCardIndex > 0 && draggableRow.MouthCardIndex < draggableRow.rowObjects.Count - 1)
        {
            int firstCardIndex = draggableRow.MouthCardIndex - 1;
            int secondCardIndex = draggableRow.MouthCardIndex + 1;

            int firstCardValue = draggableRow.rowObjects[firstCardIndex].GetComponent<Card>().CardValue;
            int secondCardValue = draggableRow.rowObjects[secondCardIndex].GetComponent<Card>().CardValue;

            //Debug.Log("Mouth card index: " + draggableRow.MouthCardIndex);
            //Debug.Log("First card value: " + firstCardValue);
            //Debug.Log("Second card value: " + secondCardValue);


        var lastItem = draggableRow.rowObjects[draggableRow.rowObjects.Count - 1];
        //Debug.Log(lastItem.GetComponent<Card>().CardValue);
        int cardValue = lastItem.GetComponent<Card>().CardValue;

        switch (cardValue) {
            case 1:
                Debug.Log("Eating 1");
                if (firstCardValue % 2 == 0 && secondCardValue % 2 == 0)
                {
                    if (firstCardValue < secondCardValue)
                    {
                            RemoveAndAnimateCard(firstCardIndex);

                    }
                    else if (firstCardValue > secondCardValue)
                    {
                            RemoveAndAnimateCard(secondCardIndex);
                    }
                    else
                    {
                        Debug.Log("You lose");
                    }
                }
                break;
            case 2:
                // need to handle the case where both cards are even and less than 10
                Debug.Log("Eating 2");
                if (secondCardValue % 2 == 0 && firstCardValue < 10)
                {
                        RemoveAndAnimateCard(firstCardIndex);
                }
                else if (firstCardValue % 2 == 0 && secondCardValue < 10)
                {
                        RemoveAndAnimateCard(secondCardIndex);
                }
                break;

            case 3:
                Debug.Log("Eating 3");


                break;

            case 4:
                Debug.Log("Eating 4");
                if (firstCardValue % 2 == 1 && secondCardValue % 2 == 1)
                {
                    if (firstCardValue > secondCardValue)
                    {
                            RemoveAndAnimateCard(firstCardIndex);
                    }
                    else if (firstCardValue < secondCardValue)
                    {
                            RemoveAndAnimateCard(secondCardIndex);
                    }
                    else
                    {
                        Debug.Log("You lose");
                    }
                }

                break;

                case 5:
                    // need to handle the case where both cards are even and less than 10
                    Debug.Log("Eating 5");
                    if (secondCardValue % 2 == 1 || firstCardValue % 2 == 1)
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                        else if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                    }

                    break;

                case 6:
                    Debug.Log("Eating 6");
                    if ((firstCardValue < 10 && secondCardValue >= 10) || (firstCardValue >= 10 && secondCardValue < 10))
                    {
                        if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                        else if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                    }
                    break;

                case 7:
                    Debug.Log("Eating 7");

                    if (firstCardValue % 2 == 1 && secondCardValue % 2 == 1)
                    {
                        if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                        else if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                        else
                        {
                            Debug.Log("You lose");
                        }
                    }
                    break;

                case 9:
                    Debug.Log("Eating 9");
                    if ((firstCardValue < 10 && secondCardValue >= 10) || (firstCardValue >= 10 && secondCardValue < 10))
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                        else if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                    }

                    break;

                case 10:
                    Debug.Log("Eating 10");

                    if (Mathf.Abs(firstCardValue - secondCardValue) == 1)
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            // Remove the second card
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                        else
                        {
                            // Remove the first card
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                    }

                    break;

                case 11:
                    Debug.Log("Eating 11");

                    if (firstCardValue % 2 == 0)
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                    }
                    else if (secondCardValue % 2 == 0)
                    {
                        if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                    }


                    break;

                case 12:
                    Debug.Log("Eating 12");

                    if (firstCardValue % 2 == 0 && secondCardValue % 2 == 0)
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                        else if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                        else
                        {
                            Debug.Log("You lose");
                        }
                    }
                    break;

                case 13:
                    Debug.Log("Eating 13");

                    if ((firstCardValue % 2 == 1 && secondCardValue >= 10) || (firstCardValue >= 10 && secondCardValue % 2 == 1))
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                        else if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                    }
                    break;

                case 14:
                    Debug.Log("Eating 14");
                    if (firstCardValue < 10 && secondCardValue % 2 == 1) 
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (firstCardValue % 2 == 1 && secondCardValue < 10)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    break;

                case 15:
                    Debug.Log("Eating 15");
                    if (firstCardValue % 2 == 0 && secondCardValue >= 10)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (firstCardValue >= 10 && secondCardValue %2 == 0)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }

                    break;


                case 16:
                    Debug.Log("Eating 16");

                    break;


                case 17:
                    Debug.Log("Eating 17");
                    if (firstCardValue % 2 == 0 || secondCardValue % 2 == 0)
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                        else if (firstCardValue < secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                    }

                    break;

                case 18:
                    Debug.Log("Eating 18");
                    if (firstCardValue % 2 == 1 && firstCardValue > secondCardValue)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (secondCardValue % 2 == 1 && secondCardValue > firstCardValue)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    break;
            }
        //Debug.Log("eating from top");
        }
    }

    private void RemoveAndAnimateCard(int cardIndex)
    {
        GameObject card = draggableRow.rowObjects[cardIndex];
        draggableRow.rowObjects.RemoveAt(cardIndex);
        StartCoroutine(AnimateCardOffScreen(card)); // Animate the card off screen
        hasEaten = true;
        gameState = GameState.CardDraw;
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



