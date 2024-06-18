using UnityEngine;
using UnityEngine.UI;

public class BuildingUIButton : MonoBehaviour
{
    public BuildingPlacer buildingPlacer;
    public BuildingType buildingType;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => buildingPlacer.SetSelectedBuilding(buildingType));
    }
}