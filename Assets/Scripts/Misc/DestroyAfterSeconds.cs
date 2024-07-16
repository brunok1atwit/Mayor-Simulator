using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter(seconds));
    }


    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
