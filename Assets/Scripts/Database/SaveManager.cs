using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public CityManager cityManager;


    void Start()
    {
        
    }

    public void SaveGame()
    {
        cityManager.SaveCity();
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        cityManager.LoadCity(cityManager.currentCityId); 
        Debug.Log("Game Loaded!");
    }
}

