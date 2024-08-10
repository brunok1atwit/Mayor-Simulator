using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SidePanelController : MonoBehaviour
{
    public GameObject sidePanel;
    public Button toggleButton;
    public Button[] categoryButtons;
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public BuildingType[] buildings;
    public BuildingType[] zones;
    private bool isPanelVisible = false;
    private float panelWidth;
    private List<GameObject> currentButtons = new List<GameObject>();

    void Start()
    {
        RectTransform panelRectTransform = sidePanel.GetComponent<RectTransform>();
        panelWidth = panelRectTransform.sizeDelta.x;
        panelRectTransform.sizeDelta = new Vector2(0, panelRectTransform.sizeDelta.y);
        toggleButton.onClick.AddListener(TogglePanel);
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
        float duration = 0.3f;
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
            foreach (GameObject button in currentButtons)
            {
                Destroy(button);
            }
            currentButtons.Clear();
            foreach (BuildingType building in buildings)
            {
                if (building.category == selectedCategory)
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
                    newButton.GetComponentInChildren<Text>().text = building.buildingName;
                    newButton.GetComponent<Image>().sprite = building.buildingPrefab.GetComponent<SpriteRenderer>().sprite;
                    newButton.GetComponent<BuildingUIButton>().buildingPlacer = FindObjectOfType<BuildingPlacer>();
                    newButton.GetComponent<BuildingUIButton>().buildingType = building;
                    currentButtons.Add(newButton);
                }
            }

            if (selectedCategory == BuildingCategory.Zoning)
            {
                foreach (BuildingType zone in zones)
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
                    newButton.GetComponentInChildren<Text>().text = zone.buildingName;
                    newButton.GetComponent<Image>().sprite = zone.zonePrefab.GetComponent<SpriteRenderer>().sprite;
                    newButton.GetComponent<BuildingUIButton>().buildingPlacer = FindObjectOfType<BuildingPlacer>();
                    newButton.GetComponent<BuildingUIButton>().buildingType = zone;
                    currentButtons.Add(newButton);
                }
            }
        }
    }
}
