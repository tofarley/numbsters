using UnityEngine;
using System.Collections.Generic;
using NumbstersGameState;
using System.ComponentModel;

public class DraggableRow : MonoBehaviour
{
    public List<GameObject> rowObjects = new List<GameObject>();
    private GameObject draggedObject;
    private Vector3 offset;
    private GameObject firstSelectedObject;
    private GameObject secondSelectedObject;

    private int mouthCardIndex = 0;
    public int MouthCardIndex
    {
        get { return mouthCardIndex; }
        set { mouthCardIndex = value; }
    }

    private bool hasMovedOrSwappedCards = false;
    public bool HasMovedOrSwappedCards
    {
        get { return hasMovedOrSwappedCards; }
        set { hasMovedOrSwappedCards = value; }
    }

    public GameState gameState;

    void Start()
    {
        //ArrangeObjects();
    }

    void Update()
    {
        if (gameState == GameState.Move)
        {
            if (Input.GetMouseButtonDown(0))
            {
                draggedObject = GetClickedObject();
                if (draggedObject != null)
                {
                    offset = draggedObject.transform.position - GetMouseWorldPosition();
                    ScaleCard(draggedObject, 1.2f); // Scale up the card
                }
            }

            if (Input.GetMouseButton(0) && draggedObject != null)
            {
                Vector3 newPosition = GetMouseWorldPosition() + offset;
                draggedObject.transform.position = new Vector3(newPosition.x, draggedObject.transform.position.y, draggedObject.transform.position.z);

            }

            if (Input.GetMouseButtonUp(0) && draggedObject != null)
            {
                RearrangeObjects();
                ScaleCard(draggedObject, 1.0f); // Reset the scale
                this.hasMovedOrSwappedCards = true;
                draggedObject = null;
            }

            if (Input.GetMouseButtonDown(1)) // Right-click to select cards for swapping
            {
                GameObject clickedObject = GetClickedObject();
                if (clickedObject != null)
                {
                    if (firstSelectedObject == null)
                    {
                        firstSelectedObject = clickedObject;
                        HighlightCard(firstSelectedObject, true);
                    }
                    else if (secondSelectedObject == null)
                    {
                        secondSelectedObject = clickedObject;
                        HighlightCard(secondSelectedObject, true);
                        SwapSelectedCards();
                        this.hasMovedOrSwappedCards = true;
                    }
                }
            }
        }

    }

    public void ArrangeObjects()
    {
        float cardSpacing = 2f; // Adjust this value to change the spacing between objects
        float totalWidth = cardSpacing * (rowObjects.Count - 1); // 5 spaces between 6 cards

        // Calculate the starting position (left-most card)
        Vector3 startPos = new Vector3(-totalWidth / 2, 0, 0);


        for (int i = 0; i < rowObjects.Count; i++)
        {
            SpriteRenderer spriteRenderer = rowObjects[i].GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 0;
            Vector3 targetPosition = startPos + new Vector3(cardSpacing * i, 0, 0);
            rowObjects[i].transform.position = targetPosition;
            if (rowObjects[i].GetComponent<Card>().CardValue == 8)
            {
                mouthCardIndex = i;
                Debug.Log("Setting MouthCardIndex to " + i);
            }
            ScaleCard(rowObjects[i], 1.0f); // Reset the scale
        }
    }


    void RearrangeObjects()
    {
        rowObjects.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        ArrangeObjects();
    }

    GameObject GetClickedObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            SpriteRenderer spriteRenderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
            return hit.collider.gameObject;
        }
        return null;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void SwapSelectedCards()
    {
        if (firstSelectedObject != null && secondSelectedObject != null)
        {
            int firstIndex = rowObjects.IndexOf(firstSelectedObject);
            int secondIndex = rowObjects.IndexOf(secondSelectedObject);

            if (firstIndex != -1 && secondIndex != -1)
            {
                SwapCards(firstIndex, secondIndex);
            }

            HighlightCard(firstSelectedObject, false);
            HighlightCard(secondSelectedObject, false);

            firstSelectedObject = null;
            secondSelectedObject = null;
        }
    }

    void SwapCards(int index1, int index2)
    {
        GameObject temp = rowObjects[index1];
        rowObjects[index1] = rowObjects[index2];
        rowObjects[index2] = temp;

        ArrangeObjects();
    }
    void HighlightCard(GameObject card, bool highlight)
    {
        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        if (highlight)
        {
            spriteRenderer.color = Color.yellow; // Change to highlight color
        }
        else
        {
            spriteRenderer.color = Color.white; // Change back to original color
        }
    }

    public void ScaleCard(GameObject card, float scale)
    {
        card.transform.localScale = new Vector3(scale, scale, scale);
    }
}