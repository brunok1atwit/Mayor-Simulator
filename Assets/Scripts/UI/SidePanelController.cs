using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SidePanelController : MonoBehaviour
{
    public GameObject sidePanel;
    public Button toggleButton;
    public Button[] categoryButtons; // Assign buttons in the editor
    public GameObject buttonPrefab; // Assign the prefab for the building buttons
    public Transform buttonContainer; // Assign the container where buttons will be instantiated
    public BuildingType[] buildings; // Assign all building types in the editor
    public BuildingType[] zones; // Add this line to assign all zone types in the editor
    private bool isPanelVisible = false;
    private float panelWidth;
    private List<GameObject> currentButtons = new List<GameObject>();

    void Start()
    {
        // Set the initial state of the panel
        RectTransform panelRectTransform = sidePanel.GetComponent<RectTransform>();
        panelWidth = panelRectTransform.sizeDelta.x;
        panelRectTransform.sizeDelta = new Vector2(0, panelRectTransform.sizeDelta.y);

        // Assign the TogglePanel method to the button's onClick event
        toggleButton.onClick.AddListener(TogglePanel);

        // Assign category buttons to switch categories
        foreach (Button categoryButton in categoryButtons)
        {
            categoryButton.onClick.AddListener(() => SwitchCategory(categoryButton.name));
        }
    }

    public void TogglePanel()
    {
        isPanelVisible = !isPanelVisible;
        StartCoroutine(AnimatePanel());
    }

    private IEnumerator AnimatePanel()
    {
        RectTransform panelRectTransform = sidePanel.GetComponent<RectTransform>();
        float targetWidth = isPanelVisible ? panelWidth : 0;
        float currentWidth = panelRectTransform.sizeDelta.x;
        float duration = 0.3f; // Animation duration in seconds
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newWidth = Mathf.Lerp(currentWidth, targetWidth, elapsedTime / duration);
            panelRectTransform.sizeDelta = new Vector2(newWidth, panelRectTransform.sizeDelta.y);
            yield return null;
        }

        panelRectTransform.sizeDelta = new Vector2(targetWidth, panelRectTransform.sizeDelta.y);
    }

    private void SwitchCategory(string categoryName)
    {
        BuildingCategory selectedCategory;
        if (System.Enum.TryParse(categoryName, out selectedCategory))
        {
            // Clear existing buttons
            foreach (GameObject button in currentButtons)
            {
                Destroy(button);
            }
            currentButtons.Clear();

            // Add new buttons based on the selected category
            foreach (BuildingType building in buildings)
            {
                if (building.category == selectedCategory)
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
                    newButton.GetComponentInChildren<Text>().text = building.buildingName;
                    newButton.GetComponent<Image>().sprite = building.buildingPrefab.GetComponent<SpriteRenderer>().sprite; // Assuming prefab has a SpriteRenderer
                    newButton.GetComponent<BuildingUIButton>().buildingPlacer = FindObjectOfType<BuildingPlacer>();
                    newButton.GetComponent<BuildingUIButton>().buildingType = building;
                    currentButtons.Add(newButton);
                }
            }

            // Add zone buttons
            if (selectedCategory == BuildingCategory.Zoning) // Ensure you have a Zoning category or similar
            {
                foreach (BuildingType zone in zones)
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
                    newButton.GetComponentInChildren<Text>().text = zone.buildingName;
                    newButton.GetComponent<Image>().sprite = zone.zonePrefab.GetComponent<SpriteRenderer>().sprite; // Assuming prefab has a SpriteRenderer
                    newButton.GetComponent<BuildingUIButton>().buildingPlacer = FindObjectOfType<BuildingPlacer>();
                    newButton.GetComponent<BuildingUIButton>().buildingType = zone;
                    currentButtons.Add(newButton);
                }
            }
        }
    }
}
