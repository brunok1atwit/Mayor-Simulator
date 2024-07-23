using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSpeed = 10.0f;
    public float zoomSpeed = 2.0f;
    public float panSpeed = 10.0f;
    public float mouseSensitivity = 100.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 50.0f;
    public float transitionSpeed = 2.0f;

    private bool isRotating = true;
    private bool transitioning = false;
    private Camera cam;
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component not found!");
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isRotating)
            {
                isRotating = false;
                ToggleCursorLock();
            }
            else
            {
                transitioning = true;
            }
        }

        if (isRotating)
        {
            RotateAroundCenterPoint();
        }
        else if (transitioning)
        {
            SmoothTransitionToOriginal();
        }
        else
        {
            ManualMovement();
            MouseLook();
        }

        Zoom();
    }

    void RotateAroundCenterPoint()
    {
        if (centerPoint != null)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    void ManualMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * panSpeed * Time.deltaTime, Space.Self);
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float distance = Vector3.Distance(transform.position, centerPoint.position);
        distance = Mathf.Clamp(distance - scroll * zoomSpeed, minZoom, maxZoom);
        transform.position = Vector3.MoveTowards(transform.position, centerPoint.position, scroll * zoomSpeed);
    }

    void SmoothTransitionToOriginal()
    {
        transform.position = Vector3.Lerp(transform.position, originalPosition, transitionSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, transitionSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, originalPosition) < 0.1f && Quaternion.Angle(transform.rotation, originalRotation) < 1.0f)
        {
            transitioning = false;
            isRotating = true;
            ToggleCursorLock();
        }
    }

    void ToggleCursorLock()
    {
        if (isRotating)
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
