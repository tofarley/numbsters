using UnityEngine;
using System.Collections.Generic;

public class DraggableRow : MonoBehaviour
{
    public List<GameObject> rowObjects = new List<GameObject>();
    private GameObject draggedObject;
    private Vector3 offset;

    void Start()
    {
        //ArrangeObjects();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            draggedObject = GetClickedObject();
            if (draggedObject != null)
            {
                offset = draggedObject.transform.position - GetMouseWorldPosition();
            }
        }

        if (Input.GetMouseButton(0) && draggedObject != null)
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            draggedObject.transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0) && draggedObject != null)
        {
            RearrangeObjects();
            draggedObject = null;
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
            Vector3 targetPosition = startPos + new Vector3(cardSpacing * i, 0, 0);
            rowObjects[i].transform.position = targetPosition;
            //rowObjects[i].transform.position = startPos + Vector3.right * i * cardSpacing;
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
            Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            SpriteRenderer spriteRenderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = rowObjects.Count + 1;
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

    public void PopulateRow()
    {
        foreach (Transform child in transform)
        {
            rowObjects.Add(child.gameObject);
            //child is your child transform
        }
        ArrangeObjects();
    }
}