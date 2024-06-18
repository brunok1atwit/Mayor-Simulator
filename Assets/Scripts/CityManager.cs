using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public List<BuildingType> buildingTypes;
    public GameObject[,] cityGrid; // Simple 2D grid for buildings
    public int citySize = 10; // Define the size of your city grid
    public float[,] happinessGrid; // Grid to store happiness values
    public float funds = 10000.00f; // Total funds for user
    public TextMeshProUGUI fundsText;
    public int population = 15;
    public TextMeshProUGUI populationText;
    public float delay = 5f;
    public float discoverChance = 0.5f;
    public float moveInChance = 0.3f;
    public TextMeshProUGUI happinessText;

    void Start()
    {
        cityGrid = new GameObject[citySize, citySize];
        happinessGrid = new float[citySize, citySize];
        fundsText.text = "Funds: $" + funds.ToString();
        StartCoroutine(CheckPopulation());
    }

    private void Update()
    {
        fundsText.text = "Funds: $" + funds.ToString();
        populationText.text = "Population: " + population.ToString();
        happinessText.text = "Happiness: " + CalculateTotalHappiness().ToString();
    }


    public void PlaceBuilding(BuildingType buildingType, int x, int y)
    {
        if (x >= 0 && x < citySize && y >= 0 && y < citySize)
        {
            
            if (cityGrid[x,y] != null)
            {
                print("Building Here.");
            }
            else
            {
                GameObject building = Instantiate(buildingType.buildingPrefab, new Vector3(x, 0, y), Quaternion.identity);
                cityGrid[x, y] = building;
                // Update city ratings
                UpdateCityRatings(buildingType, x, y, false);
                funds -= buildingType.cost / 2.0f;
            }
                


        }
        else
        {
            Debug.LogWarning("Position out of bounds");
        }
    }

    void UpdateCityRatings(BuildingType buildingType, int x, int y, bool isRemoving)
    {
        float happinessImpact = buildingType.happinessImpact;
        // Apply direct impact
        if (isRemoving)
        {
            happinessImpact *= -1.0f;
        }

        happinessGrid[x, y] += happinessImpact;

        // Apply proximity effect
        float proximityImpact = isRemoving ? -1 * buildingType.proximityBonusImpact : buildingType.proximityBonusImpact;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int newX = x + i;
                int newY = y + j;

                if (newX >= 0 && newX < citySize && newY >= 0 && newY < citySize)
                {
                    happinessGrid[newX, newY] += proximityImpact;
                }
            }
        }

        
    }

    public float CalculateTotalHappiness()
    {
        float totalHappiness = 0f;
        foreach (float value in happinessGrid)
        {
            totalHappiness += value;
        }
        return totalHappiness;
    }

    public void RemoveBuilding(int x, int y)
    {
        if (x >= 0 && x < citySize && y >= 0 && y < citySize)
        {
            // Check if there's a building to remove
            if (cityGrid[x, y] != null) 
            {
                GameObject building = cityGrid[x, y];
                BuildingType buildingType = GetBuildingTypeFromPrefab(building);

                Destroy(building);
                cityGrid[x, y] = null;

                // Update city ratings by reversing the impact
                funds += buildingType.cost / 2.0f;
                UpdateCityRatings(buildingType, x, y, true);
            }
            else
            {
                Debug.LogWarning("No building at this position to remove");
            }
        }
        else
        {
            Debug.LogWarning("Position out of bounds");
        }
    }

    BuildingType GetBuildingTypeFromPrefab(GameObject building)
    {
        foreach (BuildingType bt in buildingTypes)
        {
            string Name = bt.buildingPrefab.name + "(Clone)";
            if (Name == building.name)
            {
                return bt;
            }
        }
        return null;
    }
    IEnumerator CheckPopulation() {
        //TODO: add leave city chance
        //TODO: check to see if they can enter the city
        //TODO: check available space
        //TODO: happiness rating effects chance to move in
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            //random.value goes from 0.0 -> 1.0
            if(Random.value < discoverChance)
            {
                if(Random.value < moveInChance)
                {
                    population++;
                    Debug.Log("Discovered and moved in");
                }
                else
                {
                    Debug.Log("Discovered but did not move in");
                }
            }

        }

    
    }
}