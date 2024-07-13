using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public CityManager cityManager;


    void Start()
    {
        
    }

    public void SaveGame(int slotId)
    {
        cityManager.SaveCity(slotId);
        Debug.Log("Game Saved! with ID "+ slotId);
    }

    public void LoadGame(int slotId)
    {
        cityManager.LoadCity(slotId); 
        Debug.Log("Game Loaded! with ID "+ slotId);
    }
    public void Open(GameObject item)
    {
        item.SetActive(!item.activeSelf);
    }
}

