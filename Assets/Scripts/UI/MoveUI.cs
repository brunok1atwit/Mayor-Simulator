using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveUI : MonoBehaviour
{
    public RectTransform uiElement;
    public float moveSpeed = 500f;
    public bool moveOffScreen = false;

    private Vector2 originalPosition;
    private Vector2 offScreenPosition;
    private bool isOffScreen = false;

    public RectTransform barImage;

    void Start()
    {
        originalPosition = uiElement.anchoredPosition;

        offScreenPosition = new Vector2(-440.0f, originalPosition.y);
    }

    void Update()
    {
        if (moveOffScreen)
        {
            MoveOffScreen();
        }
        else
        {
            MoveBackOnScreen();
        }
    }

    public void MoveOffScreen()
    {
        if (!isOffScreen)
        {
            uiElement.anchoredPosition = Vector2.MoveTowards(uiElement.anchoredPosition, offScreenPosition, moveSpeed * Time.deltaTime * 3.0f);
            if (Vector2.Distance(uiElement.anchoredPosition, offScreenPosition) < 0.1f)
            {
                isOffScreen = true;
            }
        }
    }

    public void MoveBackOnScreen()
    {
        if (isOffScreen)
        {
            uiElement.anchoredPosition = Vector2.MoveTowards(uiElement.anchoredPosition, originalPosition, moveSpeed * Time.deltaTime * 3.0f);
            if (Vector2.Distance(uiElement.anchoredPosition, originalPosition) < 0.1f)
            {
                isOffScreen = false;
            }
        }
    }

    public void ToggleMove()
    {
        moveOffScreen = !moveOffScreen;
        barImage.localScale = new Vector3(-barImage.localScale.x, barImage.localScale.y, barImage.localScale.z);
    }
}
