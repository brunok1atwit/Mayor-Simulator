using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject sidePanel;
    public GameObject buildingPanel;
    public Button housingButton;
    public Text happinessText;
    public Slider happinessSlider;
    public Text populationText;
    public Text fundsText;

    private void Start()
    {
        // Ensure the building panel is initially hidden
        buildingPanel.SetActive(false);

        // Assign functions to the category buttons
        housingButton.onClick.AddListener(ShowHousingBuildings);
    }

    private void Update()
    {
        // Hide the building panel if clicking outside
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(buildingPanel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            buildingPanel.SetActive(false);
        }
    }

    private void ShowHousingBuildings()
    {
        // Show the building panel and populate it with housing buildings
        buildingPanel.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in buildingPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new buttons for housing buildings
        // CreateButtonForBuilding(BuildingType buildingType);
    }

    public void UpdateHappiness(float happiness)
    {
        happinessText.text = $"Happiness: {happiness}";
        happinessSlider.value = happiness;
    }

    public void UpdatePopulation(int population)
    {
        populationText.text = $"Population: {population}";
    }

    public void UpdateFunds(float funds)
    {
        fundsText.text = $"Funds: {funds}";
    }
}
