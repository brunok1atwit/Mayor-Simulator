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
    // Add other impacts relevant to your game

    // Add proximity effects if needed
    public float proximityBonusRange;
    public float proximityBonusImpact;
}

