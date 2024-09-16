using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{

    [SerializeField] private int cardValue;

    public int CardValue
    {
        get { return cardValue; }
        set { cardValue = value; }
    }

    [SerializeField] private String cardText;

    public String CardText
    {
        get { return cardText; }
        set { cardText = value; }
    }

    [SerializeField] private bool isMouth = false;

    public bool IsMouth
    {
        get { return isMouth; }
        set { isMouth = value; }
    }

    private TMP_Text tooltipObject;

    // Start is called before the first frame update
    void Start()
    {
        tooltipObject = GameObject.FindGameObjectWithTag("ToolTip").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Eat(int bottomCard, int topCard)
    {
        if (bottomCard == topCard)
        {
            Debug.Log("Eat");
        }
    }

    private void OnMouseEnter()
    {
        ShowTooltip();
    }

    private void OnMouseExit()
    {
        HideTooltip();
    }

    private void ShowTooltip()
    {
        tooltipObject.SetText(cardText);
        //tooltipObject.transform.parent.gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        tooltipObject.SetText("");
        //tooltipObject.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        // if (tooltipObject.transform.parent.gameObject.activeSelf)
        // {
        //     tooltipObject.transform.position = Input.mousePosition;
        // }
    }
}
