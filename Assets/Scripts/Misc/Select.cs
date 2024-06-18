using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject uiPanel;

    void OnMouseDown()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
        }
    }
}
