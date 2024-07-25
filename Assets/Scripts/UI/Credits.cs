using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 30.0f;  // Speed at which the credits scroll
    private RectTransform rectTransform;
    private Vector3 initialPosition;
    public GameObject fade;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.localPosition;
    }

    void Update()
    {
        // Scroll the text upwards
        rectTransform.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;
        //rectTransform.localPosition -= Vector3.back * scrollSpeed * Time.deltaTime;

        // If the text has moved completely off the screen, reset its position
        if (rectTransform.localPosition.y >= rectTransform.rect.height + Screen.height * 4)
        {
            rectTransform.localPosition = initialPosition;
            StartCoroutine(LoadScene());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        GameObject go = Instantiate(fade, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(go);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }
}
