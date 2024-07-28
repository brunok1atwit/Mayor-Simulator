using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public CityManager cityManager;
    public BuildingType selectedBuildingType;
    public Camera defaultCamera;
    public Camera rotateCamera;
    public GameObject defaultCanvas;
    public GameObject rotateCanvas;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = defaultCamera;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mainCamera.name != "Rotate Camera")
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                int x = Mathf.RoundToInt(point.x);
                int y = Mathf.RoundToInt(point.z);

                if (selectedBuildingType.zonePrefab != null && selectedBuildingType.isZone)
                {
                    if (cityManager.zoneGrid[x, y] == ZoneType.None)
                        cityManager.PlaceZone(selectedBuildingType, x, y);
                    else
                        Debug.Log("Zone already here!");

                }
                else if (IsValidPlacement(selectedBuildingType, x, y))
                {
                    cityManager.PlaceBuilding(selectedBuildingType, x, y);
                }
                else
                {
                    Debug.Log("Invalid placement according to zoning regulations.");
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && mainCamera.name != "Rotate Camera")
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                int x = Mathf.RoundToInt(point.x);
                int y = Mathf.RoundToInt(point.z);

                Debug.Log(hit.transform.gameObject.tag);
                if (!(hit.transform.gameObject.CompareTag("Street")))
                {
                    cityManager.RemoveBuilding(x, y);
                }
                else
                {
                    Debug.Log("Can't place on street");
                }
            }
        }
    }

    public void SetSelectedBuilding(BuildingType buildingType)
    {
        selectedBuildingType = buildingType;
        cityManager.DisplayFact(buildingType);
    }


    bool IsValidPlacement(BuildingType buildingType, int x, int y)
    {
        var zoneAtLocation = cityManager.zoneGrid[x, y];
        return zoneAtLocation == buildingType.zoneType || (zoneAtLocation == ZoneType.MixedUse && buildingType.zoneType != ZoneType.Industrial);
    }

    public void toRotate()
    {
        defaultCanvas.SetActive(false);
        rotateCanvas.SetActive(true);
        rotateCamera.gameObject.SetActive(true);
        defaultCamera.gameObject.SetActive(false);
        mainCamera = rotateCamera;
    }

    public void toMain()
    {
        defaultCanvas.SetActive(true);
        rotateCanvas.SetActive(false);
        rotateCamera.gameObject.SetActive(false);
        defaultCamera.gameObject.SetActive(true);
        mainCamera = defaultCamera;
    }
}
