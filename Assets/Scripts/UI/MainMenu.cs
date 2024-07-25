using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject fade;
    public void LoadGame(int scene)
    {
        StartCoroutine(LoadScene(scene));
    }

    IEnumerator LoadScene(int scene)
    {
        GameObject go = Instantiate(fade, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(go);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(scene);
    }
}
