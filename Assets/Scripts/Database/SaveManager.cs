using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public CityManager cityManager;
    public GameObject savePad;
    public GameObject save;
    public GameObject load;

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
        save.SetActive(!save.activeSelf);
        load.SetActive(!load.activeSelf);
        savePad.SetActive(!savePad.activeSelf);
    }
}

