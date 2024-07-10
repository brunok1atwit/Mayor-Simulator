using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static DatabaseManager;

public class CityManager : MonoBehaviour
{
    private DatabaseManager _databaseManager;
    public int currentCityId;

    public List<BuildingType> buildingTypes;
    public GameObject[,] cityGrid; // Simple 2D grid for buildings
    public int citySize = 10; // Define the size of your city grid
    public float[,] happinessGrid;
    public float[,] economicGrid;
    public float[,] environmentalGrid;
    public float[,] safetyGrid;
    public float[,] healthcareGrid;
    public float[,] educationGrid;
    public float[,] recreationGrid;
    public float[,] housingGrid;

    public float funds = 10000.00f;
    public TextMeshProUGUI fundsText;
    public int population = 15;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI economText;
    public TextMeshProUGUI enviroText;
    public TextMeshProUGUI safetText;
    public TextMeshProUGUI healtText;
    public TextMeshProUGUI recreText;
    public TextMeshProUGUI houseText;
    public float discoverChance = 0.95f;
    public float moveInChance = 0.65f;
    public float taxRate = 0.05f;

    private Dictionary<Vector2Int, BuildingType> placedBuildings = new Dictionary<Vector2Int, BuildingType>();

    void Start()
    {
        _databaseManager = FindObjectOfType<DatabaseManager>();
        cityGrid = new GameObject[citySize, citySize];
        happinessGrid = new float[citySize, citySize];
        economicGrid = new float[citySize, citySize];
        environmentalGrid = new float[citySize, citySize];
        safetyGrid = new float[citySize, citySize];
        healthcareGrid = new float[citySize, citySize];
        educationGrid = new float[citySize, citySize];
        recreationGrid = new float[citySize, citySize];
        housingGrid = new float[citySize, citySize];

        fundsText.text = "Funds: $" + funds.ToString();
        StartCoroutine(CheckPopulation());
        StartCoroutine(Taxes());

        LoadCity(currentCityId);
    }

    private void Update()
    {
        fundsText.text = "Funds: $" + funds.ToString();
        populationText.text = "Population: " + population.ToString();
        happinessText.text = "Happiness: " + CalculateTotalHappiness().ToString();
        economText.text = "Economic: " + CalculateTotalScore(economicGrid).ToString();
        enviroText.text = "Environmental: " + CalculateTotalScore(environmentalGrid).ToString();
        safetText.text = "Safety: " + CalculateTotalScore(safetyGrid).ToString();
        healtText.text = "Healthcare: " + CalculateTotalScore(healthcareGrid).ToString();
        recreText.text = "Recreation: " + CalculateTotalScore(recreationGrid).ToString();
        houseText.text = "Housing: " + CalculateTotalScore(housingGrid).ToString();
    }

    void LoadCity(int cityId)
    {
        City city = _databaseManager.GetCity(cityId);
        if (city != null)
        {
            // Load city data
            population = city.Population;
            funds = city.Funds;

            // Load buildings
            List<Building> buildings = _databaseManager.GetBuildings(cityId);
            foreach (var building in buildings)
            {
                // Instantiate buildings based on stored data
                BuildingType buildingType = GetBuildingType(building.Type);
                PlaceBuilding(buildingType, building.X, building.Y);
            }
        }
    }

    public void SaveCity()
    {
        City city = new City
        {
            Id = currentCityId,
            Name = "Your City Name",
            Population = population,
            Funds = funds
        };
        _databaseManager.SaveCity(city);

        // Save buildings
        foreach (var kvp in placedBuildings)
        {
            Vector2Int position = kvp.Key;
            BuildingType buildingType = kvp.Value;

            Building building = new Building
            {
                CityId = city.Id,
                Type = buildingType.buildingName,
                X = position.x,
                Y = position.y
            };
            _databaseManager.SaveBuilding(building);
        }
    }

    private BuildingType GetBuildingType(string buildingName)
    {
        foreach (var buildingType in buildingTypes)
        {
            if (buildingType.buildingName == buildingName)
            {
                return buildingType;
            }
        }
        return null;
    }

    public void PlaceBuilding(BuildingType buildingType, int x, int y)
    {
        if (x >= 0 && x < citySize && y >= 0 && y < citySize)
        {
            if (cityGrid[x, y] != null)
            {
                print("Building Here.");
            }
            else
            {
                GameObject building = Instantiate(buildingType.buildingPrefab, new Vector3(x, 0, y), Quaternion.identity);
                cityGrid[x, y] = building;
                Vector2Int position = new Vector2Int(x, y);
                placedBuildings[position] = buildingType;
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
        float multiplier = isRemoving ? -1.0f : 1.0f;

        economicGrid[x, y] += buildingType.economicImpact * multiplier;
        environmentalGrid[x, y] += buildingType.environmentalImpact * multiplier;
        safetyGrid[x, y] += buildingType.safetyImpact * multiplier;
        healthcareGrid[x, y] += buildingType.healthcareImpact * multiplier;
        educationGrid[x, y] += buildingType.educationImpact * multiplier;
        recreationGrid[x, y] += buildingType.recreationImpact * multiplier;
        housingGrid[x, y] += buildingType.housingImpact * multiplier;
        ApplyDiminishingReturns(x, y, buildingType, isRemoving);

        if (isRemoving && cityGrid[x, y] == null)
        {
            economicGrid[x, y] = 0;
            environmentalGrid[x, y] = 0;
            safetyGrid[x, y] = 0;
            healthcareGrid[x, y] = 0;
            educationGrid[x, y] = 0;
            recreationGrid[x, y] = 0;
            housingGrid[x, y] = 0;
        }

        happinessGrid[x, y] = CalculateHappiness(x, y);
    }

    void ApplyDiminishingReturns(int x, int y, BuildingType buildingType, bool isRemoving)
    {
        float diminishingFactor = isRemoving ? 1.0f / 0.9f : 0.9f; // 0.9 for adding, 1/0.9 for removing

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int newX = x + i;
                int newY = y + j;

                if (newX >= 0 && newX < citySize && newY >= 0 && newY < citySize && cityGrid[newX, newY] != null)
                {
                    BuildingType nearbyBuildingType = GetBuildingTypeFromPrefab(cityGrid[newX, newY]);
                    if (nearbyBuildingType != null && nearbyBuildingType.category == buildingType.category)
                    {
                        economicGrid[newX, newY] *= diminishingFactor;
                        environmentalGrid[newX, newY] *= diminishingFactor;
                        safetyGrid[newX, newY] *= diminishingFactor;
                        healthcareGrid[newX, newY] *= diminishingFactor;
                        educationGrid[newX, newY] *= diminishingFactor;
                        recreationGrid[newX, newY] *= diminishingFactor;
                        housingGrid[newX, newY] *= diminishingFactor;
                    }

                    // Ensure grid values are reset to zero if no buildings are present
                    if (isRemoving && cityGrid[newX, newY] == null)
                    {
                        economicGrid[newX, newY] = 0;
                        environmentalGrid[newX, newY] = 0;
                        safetyGrid[newX, newY] = 0;
                        healthcareGrid[newX, newY] = 0;
                        educationGrid[newX, newY] = 0;
                        recreationGrid[newX, newY] = 0;
                        housingGrid[newX, newY] = 0;
                    }
                }
            }
        }
    }

    float CalculateHappiness(int x, int y)
    {
        float happiness = 0f;

        float economicWeight = 0.15f;
        float environmentalWeight = 0.15f;
        float safetyWeight = 0.15f;
        float healthcareWeight = 0.1f;
        float educationWeight = 0.1f;
        float recreationWeight = 0.15f;
        float housingWeight = 0.2f;

        happiness += economicGrid[x, y] * economicWeight;
        happiness += environmentalGrid[x, y] * environmentalWeight;
        happiness += safetyGrid[x, y] * safetyWeight;
        happiness += healthcareGrid[x, y] * healthcareWeight;
        happiness += educationGrid[x, y] * educationWeight;
        happiness += recreationGrid[x, y] * recreationWeight;
        happiness += housingGrid[x, y] * housingWeight;

        return happiness;
    }

    public float CalculateTotalHappiness()
    {
        float totalHappiness = 0f;
        foreach (float value in happinessGrid)
        {
            totalHappiness += value;
        }
        return totalHappiness / (citySize * citySize);
    }

    public float CalculateTotalScore(float[,] grid)
    {
        float totalScore = 0f;
        foreach (float value in grid)
        {
            totalScore += value;
        }
        return totalScore / (citySize * citySize);
    }

    public void RemoveBuilding(int x, int y)
    {
        if (x >= 0 && x < citySize && y >= 0 && y < citySize)
        {
            if (cityGrid[x, y] != null)
            {
                GameObject building = cityGrid[x, y];
                Vector2Int position = new Vector2Int(x, y);
                if (placedBuildings.TryGetValue(position, out BuildingType buildingType))
                {
                    Destroy(building);
                    cityGrid[x, y] = null;
                    funds += buildingType.cost / 2.0f;
                    UpdateCityRatings(buildingType, x, y, true);
                    placedBuildings.Remove(position);
                }
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

    IEnumerator CheckPopulation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (Random.value < discoverChance)
            {
                if (Random.value < moveInChance)
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

    IEnumerator Taxes()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            funds += population * taxRate * 100.0f;
        }
    }
}
