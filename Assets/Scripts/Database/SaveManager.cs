using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public CityManager cityManager;


    void Start()
    {
        
    }

    public void SaveGame(slotId)
    {
        cityManager.SaveCity();
        Debug.Log("Game Saved!");
    }

    public void LoadGame(int slotId)
    {
        cityManager.LoadCity(id); 
        Debug.Log("Game Loaded!");
    }
}

