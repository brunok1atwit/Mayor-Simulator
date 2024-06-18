using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;
    public Button button1;
    public Button button2;
    public Button button3;
    // Start is called before the first frame update
    void Start()
    {
        // Ensure the UI Panel is initially hidden
        uiPanel.SetActive(false);

        // Assign functions to the buttons
        button1.onClick.AddListener(Button1Action);
        button2.onClick.AddListener(Button2Action);
        button3.onClick.AddListener(Button3Action);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(uiPanel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            uiPanel.SetActive(false);
        }
    }

    void Button1Action()
    {
        Debug.Log("Button 1 Clicked");
    }

    void Button2Action()
    {
        Debug.Log("Button 2 Clicked");
    }

    void Button3Action()
    {
        Debug.Log("Button 3 Clicked");
    }
}