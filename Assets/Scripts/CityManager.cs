using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DatabaseManager;

public class CityManager : MonoBehaviour
{
    private DatabaseManager _databaseManager;
    public int currentCityId;

    public List<BuildingType> buildingTypes;
    public GameObject[,] cityGrid;
    public ZoneType[,] zoneGrid;
    public int citySize = 10;
    public float[,] happinessGrid;
    public float[,] economicGrid;
    public float[,] environmentalGrid;
    public float[,] safetyGrid;
    public float[,] healthcareGrid;
    public float[,] educationGrid;
    public float[,] recreationGrid;
    public float[,] housingGrid;

    public int housingCap;
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
    public TextMeshProUGUI eduText;

    public float discoverChance = 0.95f;
    public float taxRate = 0.05f;

    public Slider econSlider;
    public Slider safetySlider;
    public Slider healthSlider;
    public Slider recSlider;
    public Slider houseSlider;
    public Slider eduSlider;
    public Slider environmentSlider;

    private readonly float economicWeight = 0.25f;
    private readonly float safetyWeight = 0.20f;
    private readonly float housingWeight = 0.20f;
    private readonly float environmentalWeight = 0.15f;
    private readonly float healthcareWeight = 0.10f;
    private readonly float educationWeight = 0.5f;
    private readonly float recreationWeight = 0.5f;

    public TextMeshProUGUI TextFact;

    private Dictionary<Vector2Int, BuildingType> placedBuildings = new Dictionary<Vector2Int, BuildingType>();

    public GameObject smoke;

    void Start()
    {
        _databaseManager = FindObjectOfType<DatabaseManager>();
        InitializeCityGrids();
        fundsText.text = "Funds: $" + funds.ToString();
        StartCoroutine(CheckPopulation());
        StartCoroutine(Taxes());
        TextFact.text = "To achieve a balanced urban space, planning must consider economic, social, and environmental factors. \r\n\r\nImpact on Economic Scores: Integrated urbanization projects that consider economic, social, and environmental factors can enhance economic growth and improve economic scores.\r\n\r\nImpact on Safety: Sustainable urbanization includes investing in safety infrastructure, which can improve safety ratings.<sup>4</sup>";
    }

    private void Update()
    {
        fundsText.text = "Funds: $" + funds.ToString();
        populationText.text = "Population: " + population.ToString();
        happinessText.text = "Happiness: ";
        economText.text = "Economic: ";
        enviroText.text = "Environmental: ";
        safetText.text = "Safety: ";
        healtText.text = "Healthcare: ";
        recreText.text = "Recreation: ";
        houseText.text = "Housing: ";
        eduText.text = "Education: ";

        houseSlider.value = CalculateTotalScore(housingGrid);
        econSlider.value = CalculateTotalScore(economicGrid);
        healthSlider.value = CalculateTotalScore(healthcareGrid);
        recSlider.value = CalculateTotalScore(recreationGrid);
        safetySlider.value = CalculateTotalScore(safetyGrid);
        eduSlider.value = CalculateTotalScore(educationGrid);
        environmentSlider.value = CalculateTotalScore(environmentalGrid);
    }

    private void InitializeCityGrids()
    {
        cityGrid = new GameObject[citySize, citySize];
        zoneGrid = new ZoneType[citySize, citySize];
        happinessGrid = new float[citySize, citySize];
        economicGrid = new float[citySize, citySize];
        environmentalGrid = new float[citySize, citySize];
        safetyGrid = new float[citySize, citySize];
        healthcareGrid = new float[citySize, citySize];
        educationGrid = new float[citySize, citySize];
        recreationGrid = new float[citySize, citySize];
        housingGrid = new float[citySize, citySize];
    }

    private void ClearCurrentState()
    {
        for (int x = 0; x < citySize; x++)
        {
            for (int y = 0; y < citySize; y++)
            {
                if (cityGrid[x, y] != null)
                {
                    Destroy(cityGrid[x, y]);
                    cityGrid[x, y] = null;
                }
            }
        }

        InitializeCityGrids();
        placedBuildings.Clear();
        funds = 10000.00f;
        population = 0;
        housingCap = 0;
    }

    public void LoadCity(int cityId)
    {
        ClearCurrentState();
        currentCityId = cityId;
        City city = _databaseManager.GetCity(cityId);
        if (city != null)
        {
            population = city.Population;
            funds = city.Funds;

            List<Building> buildings = _databaseManager.GetBuildings(cityId);
            foreach (var building in buildings)
            {
                print(building.Type);
                BuildingType buildingType = GetBuildingType(building.Type);

                if (buildingType != null)
                {
                    PlaceBuildingFromLoad(buildingType, building.X, building.Y);
                }
            }
        }

        UpdateUI();
    }

    public void SaveCity(int cityId)
    {
        currentCityId = cityId;
        City city = new City
        {
            Id = cityId,
            Name = "City " + cityId,
            Population = population,
            Funds = funds
        };
        _databaseManager.SaveCity(city);

        _databaseManager.ClearBuildingsForCity(cityId);

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
            print(buildingType.name);
            if (buildingType.buildingName == buildingName)
            {
                return buildingType;
            }
        }
        return null;
    }

    public void SetZone(int x, int y, ZoneType zoneType)
    {
        if (x >= 0 && x < citySize && y >= 0 && y < citySize)
        {
            zoneGrid[x, y] = zoneType;
        }
    }

    public void PlaceBuilding(BuildingType buildingType, int x, int y)
    {
        if (x >= 0 && x < citySize && y >= 0 && y < citySize)
        {
            if (zoneGrid[x, y] != buildingType.zoneType && !(zoneGrid[x, y] == ZoneType.MixedUse && buildingType.zoneType != ZoneType.Industrial))
            {
                Debug.LogWarning("Cannot place building in this zone type.");
                return;
            }

            if (cityGrid[x, y] != null)
            {
                Debug.Log("Building Here.");
            }
            else
            {
                if (buildingType.buildingName == "Apartments")
                {
                    housingCap += 50;
                }
                if (buildingType.buildingName == "3 Family")
                {
                    housingCap += 9;
                }
                if (buildingType.buildingName == "House")
                {
                    housingCap += 4;
                }
                if (buildingType.buildingName == "Condos")
                {
                    housingCap += 19;
                }
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

    public void PlaceBuildingFromLoad(BuildingType buildingType, int x, int y)
    {
        if (x >= 0 && x < citySize && y >= 0 && y < citySize)
        {
            if (cityGrid[x, y] == null)
            {
                GameObject building = Instantiate(buildingType.buildingPrefab, new Vector3(x, 0, y), Quaternion.identity);
                if (building.name == "Apartments(Clone)")
                {
                    housingCap += 50;
                }
                cityGrid[x, y] = building;
                Vector2Int position = new Vector2Int(x, y);
                placedBuildings[position] = buildingType;
                UpdateCityRatings(buildingType, x, y, false);
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
        float diminishingFactor = isRemoving ? 1.0f / 0.7f : 0.7f;
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
                    if (buildingType.buildingName == "Apartments")
                    {
                        housingCap -= 50;
                        Debug.Log("Removed Apartment");
                        if (population > housingCap)
                        {
                            population = housingCap;
                        }
                    }
                    Destroy(building);
                    cityGrid[x, y] = null;
                    funds += buildingType.cost * 0.25f;
                    UpdateCityRatings(buildingType, x, y, true);
                    placedBuildings.Remove(position);
                    Instantiate(smoke, new Vector3(x, 0, y), Quaternion.identity);
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
            print(bt.name);
            if(bt.isZone)
            {
                continue;
            }
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
            float totalWeightedScore = CalculateTotalScore(economicGrid) * economicWeight +
                           CalculateTotalScore(environmentalGrid) * environmentalWeight +
                           CalculateTotalScore(safetyGrid) * safetyWeight +
                           CalculateTotalScore(healthcareGrid) * healthcareWeight +
                           CalculateTotalScore(educationGrid) * educationWeight +
                           CalculateTotalScore(recreationGrid) * recreationWeight +
                           CalculateTotalScore(housingGrid) * housingWeight;

            if (totalWeightedScore <= 10)
            {
                yield return new WaitForSeconds(3.5f);
            }
            else
            {
                yield return new WaitForSeconds(3.5f / Mathf.Log(totalWeightedScore));
            }

            if (Random.value < discoverChance)
            {
                if (Random.value < totalWeightedScore)
                {
                    if (population < housingCap)
                    {
                        population++;
                    }
                }
            }
        }
    }

    IEnumerator Taxes()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            funds += population * taxRate * 100.0f;
        }
    }

    public void DisplayFact(BuildingType buildingType)
    {
        List<string> facts = new List<string>();

        // Fact 1: For zoning type buildings
        if (buildingType.isZone)
        {
            facts.Add("Urban spaces that are planned accordingly can provide better access to amenities, leading to efficient population growth.\r\n\r\nImpact on Population: Urban spaces that are planned accordingly can provide better access to amenities, leading to efficient population growth.<sup>1</sup>");
        }

        // Fact 2: For power plants (future implementation)
        if (buildingType.buildingName == "Power Plant")
        {
            facts.Add("Urbanization often leads to higher carbon emissions due to increased energy consumption.\r\n\r\nImpact on Environmental Scores: Urbanization can negatively impact environmental ratings if not managed sustainably. Investments in green infrastructure and renewable energy can mitigate these effects, improving overall environmental quality.\r\n\r\nImpact on Happiness: Environmental quality is a significant factor in happiness. Investments in green spaces and pollution reduction tend to leave residents happier.<sup>2</sup>");
        }

        // Fact 3: For housing buildings
        if (buildingType.category == BuildingCategory.Residential)
        {
            facts.Add("Population growth can increase economic ratings through increased consumer demand and labor supply. But this poses a new challenge of proper resource allocation and infrastructure development.\r\n\r\nImpact on Economic Scores: Effectively managing population growth can boost economic scores through increased labor and demand.\r\n\r\nImpact on Safety: Rapid population growth without proper infrastructure and public services can lead to safety issues. Thoughtful planning and proper investment in safety infrastructure are crucial to maintaining safety ratings.<sup>3</sup>");
        }

        // Fact 4: For any building type
        facts.Add("To achieve a balanced urban space, planning must consider economic, social, and environmental factors. \r\n\r\nImpact on Economic Scores: Integrated urbanization projects that consider economic, social, and environmental factors can enhance economic growth and improve economic scores.\r\n\r\nImpact on Safety: Sustainable urbanization includes investing in safety infrastructure, which can improve safety ratings.<sup>4</sup>");

        // Fact 5: For recreation and housing buildings
        if (buildingType.category == BuildingCategory.Recreation || buildingType.category == BuildingCategory.Residential)
        {
            facts.Add("Balanced urban growth enhances the quality of life; this includes investments in recreational facilities and housing. \r\n\r\nImpact on Recreation: Investments in recreational facilities improve the quality of life and contribute to higher happiness scores.\r\n\r\nImpact on Housing: Providing affordable and adequate housing is essential for urban development and contributes to overall happiness and well-being.<sup>5</sup>");
        }

        // Fact 6: For recreation buildings
        if (buildingType.category == BuildingCategory.Recreation)
        {
            facts.Add("Well-designed tax policies can stimulate economic growth and fund essential public services. Impact on Economic Scores: Effective taxation policies can boost economic scores by providing the necessary funds for economic development projects and public services.\r\n\r\nImpact on Public Services: Taxes are crucial for funding public services, including healthcare, education, safety, and infrastructure, all of which contribute to overall city ratings.<sup>6</sub>");
        }

        if (facts.Count > 0)
        {
            int randomIndex = Random.Range(0, facts.Count);
            TextFact.text = facts[randomIndex];
        }
    }


    private void UpdateUI()
    {
        fundsText.text = "Funds: $" + funds.ToString();
        populationText.text = "Population: " + population.ToString();
        happinessText.text = "Happiness: " + CalculateTotalHappiness().ToString();
        economText.text = "Economic: " + CalculateTotalScore(economicGrid).ToString("f1");
        enviroText.text = "Environmental: " + CalculateTotalScore(environmentalGrid).ToString("f1");
        safetText.text = "Safety: " + CalculateTotalScore(safetyGrid).ToString("f1");
        healtText.text = "Healthcare: " + CalculateTotalScore(healthcareGrid).ToString("f1");
        recreText.text = "Recreation: " + CalculateTotalScore(recreationGrid).ToString("f1");
        houseText.text = "Housing: " + CalculateTotalScore(housingGrid).ToString("f1");
    }
}
