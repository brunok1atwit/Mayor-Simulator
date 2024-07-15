using System.Collections.Generic;
using UnityEngine;

public enum BuildingCategory
{
    Education,
    Recreation,
    Infrastructure,
    Healthcare,
    Security,
    Commercial,
    Residential
}

[CreateAssetMenu(fileName = "BuildingType", menuName = "ScriptableObjects/BuildingType", order = 1)]
public class BuildingType : ScriptableObject
{
    public BuildingCategory category;
    public string buildingName;
    public GameObject buildingPrefab;
    public float happinessImpact;
    public float educationImpact;
    public float cost;

 
    public float proximityBonusRange;
    public float proximityBonusImpact;

   
    public float economicImpact;
    public float environmentalImpact;
    public float safetyImpact;
    public float healthcareImpact;
    public float recreationImpact;
    public float housingImpact;

    private void OnEnable()
    {
        InitializeImpactScores();
    }

    void InitializeImpactScores()
    {
        switch (buildingName)
        {
            case "Apartment":
                economicImpact = 200.0f;
                environmentalImpact = -100.0f;
                safetyImpact = -50.0f;
                healthcareImpact = 100.0f;
                educationImpact = 50.0f;
                recreationImpact = 100.0f;
                housingImpact = 300.0f;
                break;
            case "Police Station":
                economicImpact = 150.0f;
                environmentalImpact = -50.0f;
                safetyImpact = 300.0f;
                healthcareImpact = 50.0f;
                educationImpact = 30.0f;
                recreationImpact = 20.0f;
                housingImpact = 0.0f;
                break;

            case "Fire Station":
                economicImpact = 120.0f;
                environmentalImpact = -40.0f;
                safetyImpact = 350.0f;
                healthcareImpact = 40.0f;
                educationImpact = 20.0f;
                recreationImpact = 10.0f;
                housingImpact = 0.0f;
                break;

            case "House":
                economicImpact = 100.0f;
                environmentalImpact = -20.0f;
                safetyImpact = -10.0f;
                healthcareImpact = 20.0f;
                educationImpact = 10.0f;
                recreationImpact = 50.0f;
                housingImpact = 250.0f;
                break;

            case "Bus Station":
                economicImpact = 80.0f;
                environmentalImpact = -30.0f;
                safetyImpact = 50.0f;
                healthcareImpact = 10.0f;
                //test edu
                educationImpact = 50000.0f;
                recreationImpact = 10.0f;
                housingImpact = 20.0f;
                break;

            case "Park":
                economicImpact = 100.0f;
                environmentalImpact = 500.0f;
                safetyImpact = 100.0f;
                healthcareImpact = 200.0f;
                educationImpact = 50.0f;
                recreationImpact = 500.0f;
                housingImpact = 100.0f;
                break;
            case "School":
                economicImpact = 300.0f;
                environmentalImpact = 50.0f;
                safetyImpact = 200.0f;
                healthcareImpact = 100.0f;
                educationImpact = 800.0f;
                recreationImpact = 200.0f;
                housingImpact = 200.0f;
                break;
            case "Street":
                economicImpact = 200.0f;
                environmentalImpact = -200.0f;
                safetyImpact = -100.0f;
                healthcareImpact = 00.0f;
                educationImpact = 50.0f;
                recreationImpact = 100.0f;
                housingImpact = 50.0f;
                break;
        }

    }
}