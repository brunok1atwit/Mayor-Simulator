using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningCamera : MonoBehaviour
{
    public Transform centerPoint;
    public float dragSpeed = 2.0f;
    public float zoomSpeed = 2.0f;
    public float rotateSpeed = 100.0f;
    public float minZoomDistance = 5.0f;
    public float maxZoomDistance = 25.0f;
    public float resetDuration = 2.0f;

    private Camera cam;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isRotating = false;
    private bool isResetting = false;

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
        bool inputReceived = false;

        if (Input.GetMouseButton(2))
        {
            DragCamera();
            inputReceived = true;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            ZoomCamera();
            inputReceived = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            RotateAroundCenterPoint();
            inputReceived = true;
            isRotating = true;
            LockCursor(true);
        }
        else
        {
            isRotating = false;
            LockCursor(false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isResetting)
        {
            StartCoroutine(ResetCamera());
            inputReceived = true;
        }
    }

    void DragCamera()
    {
        float h = -Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;
        float v = -Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
        transform.Translate(new Vector3(h, v, 0), Space.Self);
    }

    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float distance = Vector3.Distance(transform.position, centerPoint.position);
        distance = Mathf.Clamp(distance - scroll * zoomSpeed, minZoomDistance, maxZoomDistance);
        transform.position = Vector3.MoveTowards(transform.position, centerPoint.position, scroll * zoomSpeed);
    }

    void RotateAroundCenterPoint()
    {
        if (centerPoint != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            transform.RotateAround(centerPoint.position, Vector3.up, mouseX);
            transform.RotateAround(centerPoint.position, transform.right, -mouseY);
        }
    }

    IEnumerator ResetCamera()
    {
        isResetting = true;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsedTime = 0.0f;

        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPosition, originalPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        isResetting = false;
    }

    void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
