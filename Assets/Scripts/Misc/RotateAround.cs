using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSpeed = 10.0f;

    void Update()
    {
        if (centerPoint != null)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
