using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
 
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void ButtonScript(GameObject Categories)
    {
        foreach(Transform children in Categories.transform)
        {
            children.gameObject.SetActive(false);
        }
    }
}
