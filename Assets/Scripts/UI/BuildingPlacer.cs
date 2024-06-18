using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public CityManager cityManager;
    public BuildingType selectedBuildingType;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                int x = Mathf.RoundToInt(point.x);
                int y = Mathf.RoundToInt(point.z);

                //check if can place
                Debug.Log(hit.transform.gameObject.tag);
                if (!(hit.transform.gameObject.CompareTag("Street")))
                {
                    cityManager.PlaceBuilding(selectedBuildingType, x, y);
                    //deduct money
                    //cityManager.funds -= selectedBuildingType.cost;
                }
                else
                {
                    Debug.Log("Cant place on street");
                    //test
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                int x = Mathf.RoundToInt(point.x);
                int y = Mathf.RoundToInt(point.z);

                //check if can place
                Debug.Log(hit.transform.gameObject.tag);
                if (!(hit.transform.gameObject.CompareTag("Street")))
                {
                    cityManager.RemoveBuilding(x, y);
                    //deduct money
                    //cityManager.funds += selectedBuildingType.cost/2;
                    //cityManager.fundsText.text = cityManager.funds;


                }
                else
                {
                    Debug.Log("Cant place on street");
                }
            }
        }
    }

    public void SetSelectedBuilding(BuildingType buildingType)
    {
        selectedBuildingType = buildingType;
    }
}