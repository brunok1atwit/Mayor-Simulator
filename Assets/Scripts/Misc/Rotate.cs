using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 0.1f;
    GameObject gameObject;

    private bool mouseDown;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;

    private bool doRot = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject = this.transform.GameObject();
        rotation = Vector3.zero;
    }

    void Update()
    {
        if (mouseDown)
        {
            mouseOffset = (Input.mousePosition - mouseReference);


            rotation.x = (mouseOffset.y) * speed;
            rotation.y = -(mouseOffset.x) * speed;
            
            transform.Rotate(rotation);
            
            mouseReference = Input.mousePosition;
        }
        else
        {
            if (transform.rotation.x != 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 10f * Time.deltaTime);
                if (transform.rotation.x <= 0.01f && transform.rotation.y <= 0.01f && transform.rotation.z <= 0.01f) 
                {
                    transform.rotation = Quaternion.identity;
                }
            }
            else
                gameObject.transform.Rotate(0.0f, speed, 0.0f, Space.Self);
            
        }
        print(transform.rotation.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //gameObject.transform.Rotate(0.0f, speed * Time.deltaTime, 0.0f, Space.Self);
    }

    private void OnMouseDown()
    {
        mouseDown = true;
        mouseReference = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        mouseDown = false;
    }
}
