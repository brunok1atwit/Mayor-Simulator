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

        buildingPanel.SetActive(false);

     
        housingButton.onClick.AddListener(ShowHousingBuildings);
    }

    private void Update()
    {
     
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(buildingPanel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            buildingPanel.SetActive(false);
        }
    }

    private void ShowHousingBuildings()
    {
     
        buildingPanel.SetActive(true);

     
        foreach (Transform child in buildingPanel.transform)
        {
            Destroy(child.gameObject);
        }

 
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
