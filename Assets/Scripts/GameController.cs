using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NumbstersGameState;
using System.Runtime.CompilerServices;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private DraggableRow draggableRow;
    [SerializeField] private Button drawButton;

    [SerializeField] private Button eatButton;

    [SerializeField] private Button eatFromTopButton;

    [SerializeField] private TextMeshProUGUI loseScreen;

    private GameState gameState = GameState.CardDraw;

    public void disableLoseScreen()
    {
        if (loseScreen != null)
        {
            loseScreen.gameObject.SetActive(false); // Disable the TextMeshPro UI element
        }
    }

    public void enableLoseScreen()
    {
        if (loseScreen != null)
        {
            loseScreen.gameObject.SetActive(true); // Disable the TextMeshPro UI element
        }
    }

    void EnterCardDrawPhase()
    {
        gameState = GameState.CardDraw;
        drawButton.interactable = true;
        eatButton.interactable = false;
        eatFromTopButton.interactable = false;
        draggableRow.gameState = GameState.CardDraw;
        hasEaten = false;
        deck.CardDrawn = false;
        draggableRow.HasMovedOrSwappedCards = false;
    }

    void EnterMovePhase()
    {
        Debug.Log("EnterMovePhase");
        gameState = GameState.Move;
        drawButton.interactable = false;
        eatButton.interactable = true;
        eatFromTopButton.interactable = true;
        draggableRow.gameState = GameState.Move;
    }

    void EnterEatPhase()
    {
        gameState = GameState.Eat;
        draggableRow.gameState = GameState.Eat;
    }

    void EnterEndPhase()
    {
        CheckLose();
        EnterCardDrawPhase();
    }

    void CheckLose()
    {
        var lastItem = draggableRow.rowObjects[draggableRow.rowObjects.Count - 1];
        int cardValue = lastItem.GetComponent<Card>().CardValue;
        if (cardValue == 8 || hasEaten == false)
        {
            enableLoseScreen(); // Enable the lose screen
            gameState = GameState.End;
        }
    }


    void Start()
    {
        disableLoseScreen();
        deck.DealCards();
        EnterCardDrawPhase();
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
                    EnterMovePhase();
                }
                break;
            case GameState.Move:
                if (draggableRow.HasMovedOrSwappedCards)
                {
                    EnterEatPhase();
                }
                if (hasEaten)
                {
                    EnterEndPhase();
                }
                break;
            case GameState.Eat:
                if (hasEaten)
                {
                    EnterEndPhase();
                }
                break;
        }
    }



    public void Eat()
    {
        // Ensure the mouth card index is valid
        if (draggableRow.MouthCardIndex > 0 && draggableRow.MouthCardIndex < draggableRow.rowObjects.Count - 1)
        {
            int firstCardIndex = draggableRow.MouthCardIndex - 1;
            int secondCardIndex = draggableRow.MouthCardIndex + 1;

            int firstCardValue = draggableRow.rowObjects[firstCardIndex].GetComponent<Card>().CardValue;
            int secondCardValue = draggableRow.rowObjects[secondCardIndex].GetComponent<Card>().CardValue;

            // Check if the cards are sequential
            if (Mathf.Abs(firstCardValue - secondCardValue) == 1)
            {
                if (firstCardValue < secondCardValue)
                {
                    RemoveAndAnimateCard(secondCardIndex);
                }
                else
                {
                    RemoveAndAnimateCard(firstCardIndex);
                }
                hasEaten = true;
            }
            else
            {
                hasEaten = false;
                EnterEndPhase();
            }
        }
    }

    public void EatFromTop()
    {
        if (draggableRow.MouthCardIndex > 0 && draggableRow.MouthCardIndex < draggableRow.rowObjects.Count - 1)
        {
            int firstCardIndex = draggableRow.MouthCardIndex - 1;
            int secondCardIndex = draggableRow.MouthCardIndex + 1;

            int firstCardValue = draggableRow.rowObjects[firstCardIndex].GetComponent<Card>().CardValue;
            int secondCardValue = draggableRow.rowObjects[secondCardIndex].GetComponent<Card>().CardValue;


            var lastItem = draggableRow.rowObjects[draggableRow.rowObjects.Count - 1];
            int cardValue = lastItem.GetComponent<Card>().CardValue;

            switch (cardValue)
            {
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
                            hasEaten = false;
                            EnterEndPhase();
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
                            hasEaten = false;
                            EnterEndPhase();
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
                            hasEaten = false;
                            EnterEndPhase();
                        }
                    }
                    break;

                case 8:
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
                            hasEaten = false;
                            EnterEndPhase();
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
                    else if (firstCardValue >= 10 && secondCardValue % 2 == 0)
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

                default:
                    Debug.Log("No Card Eaten");
                    hasEaten = false;
                    EnterEndPhase();
                    break;
            }
        }
    }

    private void RemoveAndAnimateCard(int cardIndex)
    {
        GameObject card = draggableRow.rowObjects[cardIndex];
        draggableRow.rowObjects.RemoveAt(cardIndex);
        StartCoroutine(AnimateCardOffScreen(card)); // Animate the card off screen
        hasEaten = true;
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



