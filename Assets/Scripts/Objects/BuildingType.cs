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
    Residential,
    Zoning
}

public enum ZoneType
{
    Residential,
    Commercial,
    Industrial,
    PublicUse,
    MixedUse
}

[CreateAssetMenu(fileName = "BuildingType", menuName = "ScriptableObjects/BuildingType", order = 1)]
public class BuildingType : ScriptableObject
{
    public BuildingCategory category;
    public string buildingName;
    public GameObject buildingPrefab;
    public GameObject zonePrefab;
    public float happinessImpact;
    public float educationImpact;
    public float cost;
    public ZoneType zoneType;


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
            case "Police":
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

            case "Hospital":
                economicImpact = 160.0f;
                environmentalImpact = -30.0f;
                safetyImpact = 450.0f;
                healthcareImpact = 80.0f;
                educationImpact = 10.0f;
                recreationImpact = 5.0f;
                housingImpact = 10.0f;
                break;

            case "Restaurant":
                economicImpact = 200.0f;
                environmentalImpact = -60.0f;
                safetyImpact = 50.0f;
                healthcareImpact = -30.0f;
                educationImpact = -10.0f;
                recreationImpact = 20.0f;
                housingImpact = 0.0f;
                break;

            case "House":
                economicImpact = 100.0f;
                environmentalImpact = -20.0f;
                safetyImpact = -10.0f;
                healthcareImpact = 20.0f;
                educationImpact = 10.0f;
                recreationImpact = 50.0f;
                housingImpact = 50.0f;
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
                environmentalImpact = 1500.0f;
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
                //800
                educationImpact = 800.0f;
                recreationImpact = 200.0f;
                housingImpact = 200.0f;
                break;
            case "Street":
                economicImpact = 200.0f;
                environmentalImpact = -200.0f;
                safetyImpact = -100.0f;
                healthcareImpact = 20.0f;
                educationImpact = 50.0f;
                recreationImpact = 100.0f;
                housingImpact = 50.0f;
                break;
            case "Four-way Intersection":
                economicImpact = 200.0f;
                environmentalImpact = -200.0f;
                safetyImpact = -100.0f;
                healthcareImpact = 20.0f;
                educationImpact = 50.0f;
                recreationImpact = 100.0f;
                housingImpact = 50.0f;
                break;
            case "T-Intersection":
                economicImpact = 200.0f;
                environmentalImpact = -200.0f;
                safetyImpact = -100.0f;
                healthcareImpact = 20.0f;
                educationImpact = 50.0f;
                recreationImpact = 100.0f;
                housingImpact = 50.0f;
                break;
            case "End Piece":
                economicImpact = 100.0f;
                environmentalImpact = -200.0f;
                safetyImpact = -100.0f;
                healthcareImpact = 10.0f;
                educationImpact = 50.0f;
                recreationImpact = 100.0f;
                housingImpact = 100.0f;
                break;
            case "Curve":
                economicImpact = 200.0f;
                environmentalImpact = -200.0f;
                safetyImpact = -100.0f;
                healthcareImpact = 20.0f;
                educationImpact = 50.0f;
                recreationImpact = 100.0f;
                housingImpact = 50.0f;
                break;
            case "Condos":
                economicImpact = 160.0f;
                environmentalImpact = -100.0f;
                safetyImpact = -10.0f;
                healthcareImpact = 10.0f;
                educationImpact = 100.0f;
                recreationImpact = 100.0f;
                housingImpact = 20.0f;
                break;

            case "College":
                economicImpact = 200.0f;
                environmentalImpact = -50.0f;
                safetyImpact = 20.0f;
                healthcareImpact = -80.0f;
                educationImpact = 100.0f;
                recreationImpact = 20.0f;
                housingImpact = 10.0f;
                break;

            case "Museum":
                economicImpact = 100.0f;
                environmentalImpact = -10.0f;
                safetyImpact = 0.0f;
                healthcareImpact = 0.0f;
                educationImpact = 100.0f;
                recreationImpact = 100.0f;
                housingImpact = 00.0f;
                break;

            case "Library":
                economicImpact = 20.0f;
                environmentalImpact = -10.0f;
                safetyImpact = 0.0f;
                healthcareImpact = 0.0f;
                educationImpact = 150.0f;
                recreationImpact = 50.0f;
                housingImpact = 00.0f;
                break;
        }

    }
}