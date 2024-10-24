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

    [SerializeField] private TextMeshProUGUI cardsRemainingText;

    [SerializeField] private Canvas menuCanvas;
    private GameState gameState = GameState.CardDraw;

    private bool hasEaten = false;
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
            gameState = GameState.End;
            loseScreen.gameObject.SetActive(true); // Disable the TextMeshPro UI element
        }
    }

    public void disableMenuCanvas()
    {
        if (menuCanvas != null)
        {
            menuCanvas.gameObject.SetActive(false); // Disable the Canvas
        }
    }

    public void enableMenuCanvas()
    {
        if (menuCanvas != null)
        {
            menuCanvas.gameObject.SetActive(true); // Enable the Canvas
        }
    }

    public bool IsMenuCanvasActive()
    {
        if (menuCanvas != null)
        {
            return menuCanvas.gameObject.activeSelf; // Check if the Canvas is active
        }
        return false;
    }

    public void enableWinScreen()
    {
        if (loseScreen != null)
        {
            gameState = GameState.End;
            loseScreen.text = "You Win!";
            loseScreen.gameObject.SetActive(true); // Disable the TextMeshPro UI element
        }
    }

    void EnterCardDrawPhase()
    {
        gameState = GameState.CardDraw;
        if (deck.GetRemainingCardCount() > 0)
        {
            drawButton.interactable = true;
        }
        else
        {
            drawButton.interactable = false;
        }
        eatButton.interactable = false;
        eatFromTopButton.interactable = false;
        draggableRow.gameState = GameState.CardDraw;
        hasEaten = false;
        deck.CardDrawn = false;
        draggableRow.HasMovedOrSwappedCards = false;
        cardsRemainingText.text = "Draw [" + deck.GetRemainingCardCount() + "]";
    }

    void EnterMovePhase()
    {
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
        var rowCount = draggableRow.rowObjects.Count;
        int cardValue = lastItem.GetComponent<Card>().CardValue;
        if (cardValue == 8 || hasEaten == false)
        {
            gameState = GameState.End;
            enableLoseScreen(); // Enable the lose screen

        }
        if (rowCount == 2)
        {
            gameState = GameState.End;
            enableWinScreen(); // Enable the lose screen
        }
    }

    public void EmptyDraggableRow()
    {
        foreach (var card in draggableRow.rowObjects)
        {
            Destroy(card); // Optionally destroy the GameObject
        }
        draggableRow.rowObjects.Clear(); // Clear the list
    }
    void Start()
    {
        disableMenuCanvas();
        disableLoseScreen();
        deck.DealCards();
        EnterCardDrawPhase();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        disableMenuCanvas();
    }

    public void NewGame()
    {
        deck.CardsDealt = 0;
        EmptyDraggableRow();
        disableMenuCanvas();
        disableLoseScreen();
        deck.DealCards();
        EnterCardDrawPhase();
    }
    void Update()
    {
        if (gameState == GameState.End)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsMenuCanvasActive())
            {
                disableMenuCanvas();
            }
            else
            {
                enableMenuCanvas();
            }
            //Application.Quit();
        }

        switch (gameState)
        {
            case GameState.CardDraw:
                if (deck.GetRemainingCardCount() == 0)
                {
                    EnterMovePhase();
                }
                if (deck.CardDrawn)
                {
                    cardsRemainingText.text = "Draw [" + deck.GetRemainingCardCount() + "]";
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

    private bool IsLargestValueInRow(int cardValue)
    {
        foreach (GameObject cardObject in draggableRow.rowObjects)
        {
            int value = cardObject.GetComponent<Card>().CardValue;
            if (value > cardValue)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsSmallestValueInRow(int cardValue)
    {
        foreach (GameObject cardObject in draggableRow.rowObjects)
        {
            int value = cardObject.GetComponent<Card>().CardValue;
            if (value < cardValue)
            {
                return false;
            }
        }
        return true;
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
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;
                case 2:
                    // need to handle the case where both cards are even and less than 10
                    if (secondCardValue % 2 == 0 && firstCardValue < 10)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    else if (firstCardValue % 2 == 0 && secondCardValue < 10)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 3:
                    if (IsLargestValueInRow(firstCardValue) && secondCardValue % 2 == 1)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (IsLargestValueInRow(secondCardValue) && firstCardValue % 2 == 1)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 4:
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
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 5:
                    // need to handle the case where both cards are even and less than 10
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
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 6:
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
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 7:
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
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 8:
                    break;

                case 9:
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
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 10:
                    if (Mathf.Abs(firstCardValue - secondCardValue) == 1)
                    {
                        if (firstCardValue > secondCardValue)
                        {
                            RemoveAndAnimateCard(secondCardIndex);
                        }
                        else
                        {
                            RemoveAndAnimateCard(firstCardIndex);
                        }
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 11:
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
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 12:
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
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 13:
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
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 14:
                    if (firstCardValue < 10 && secondCardValue % 2 == 1)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (firstCardValue % 2 == 1 && secondCardValue < 10)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 15:
                    if (firstCardValue % 2 == 0 && secondCardValue >= 10)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (firstCardValue >= 10 && secondCardValue % 2 == 0)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 16:
                    if (IsSmallestValueInRow(firstCardValue) && secondCardValue % 2 == 0)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (IsSmallestValueInRow(secondCardValue) && firstCardValue % 2 == 0)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 17:
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
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                case 18:
                    if (firstCardValue % 2 == 1 && firstCardValue > secondCardValue)
                    {
                        RemoveAndAnimateCard(secondCardIndex);
                    }
                    else if (secondCardValue % 2 == 1 && secondCardValue > firstCardValue)
                    {
                        RemoveAndAnimateCard(firstCardIndex);
                    }
                    else
                    {
                        hasEaten = false;
                        EnterEndPhase();
                    }
                    break;

                default:
                    Debug.Log("No Card Eaten");
                    hasEaten = false;
                    EnterEndPhase();
                    break;
            }

            // did we eat a card? if so we need to move the card to the beginning.
            if (hasEaten)
            {
                MoveLastCardToFirst();
            }

        }
    }

    private void MoveLastCardToFirst()
    {
        if (draggableRow.rowObjects.Count > 0)
        {
            GameObject lastCard = draggableRow.rowObjects[draggableRow.rowObjects.Count - 1];
            SpriteRenderer spriteRenderer = lastCard.GetComponent<SpriteRenderer>();
            var originalSortingOrder = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = 100;
            draggableRow.rowObjects.RemoveAt(draggableRow.rowObjects.Count - 1);
            draggableRow.rowObjects.Insert(0, lastCard);
            StartCoroutine(MoveAndArrange(lastCard, originalSortingOrder));
        }
    }

    private IEnumerator MoveAndArrange(GameObject card, int originalSortingOrder)
    {
        yield return StartCoroutine(AnimateCardToFirstPosition(card));
        card.GetComponent<SpriteRenderer>().sortingOrder = originalSortingOrder; // Reset sorting order
        draggableRow.ArrangeObjects();
    }

    private IEnumerator AnimateCardToFirstPosition(GameObject card)
    {
        float duration = 1.0f; // Duration of the animation
        float elapsedTime = 0.0f;
        Vector3 startPosition = card.transform.position;

        // Calculate the target position for the first position in the row
        float cardSpacing = 2f; // Adjust this value to change the spacing between objects
        float totalWidth = cardSpacing * (draggableRow.rowObjects.Count - 1); // 5 spaces between 6 cards

        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        int originalSortingOrder = spriteRenderer.sortingOrder; // Store the original sorting order
        spriteRenderer.sortingOrder = 10; // Set a higher sorting order to ensure the card is above all others

        Vector3 targetPosition = new Vector3(-totalWidth / 2, 0, 0);

        while (elapsedTime < duration)
        {
            card.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.position = targetPosition; // Ensure the card reaches the target position
        spriteRenderer.sortingOrder = originalSortingOrder; // Reset the sorting order to its original value
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



