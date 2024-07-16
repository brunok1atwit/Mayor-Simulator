using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject fade;
    public void LoadGame()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        GameObject go = Instantiate(fade, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(go);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }
}
