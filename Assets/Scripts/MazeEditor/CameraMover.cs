using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour 
{
    public float TranslateSpeed = 1f;
    public float ScrollSpeed = 1f;

    private Camera mainCamera;

    private Vector3 mouseOriginPosition;
    private Vector3 cameraOriginPosition;
    private bool isDragging;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseOriginPosition = Input.mousePosition;
            cameraOriginPosition = mainCamera.transform.position;
            isDragging = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if(isDragging)
        {
            Vector3 mouseDeltaPostion = Vector3.zero;
            mouseDeltaPostion = Input.mousePosition - mouseOriginPosition;
            mouseDeltaPostion *= TranslateSpeed;
            Vector3 velocity = new Vector3(-mouseDeltaPostion.x, 0, -mouseDeltaPostion.y);
            mainCamera.transform.position = cameraOriginPosition + velocity;
        }

        if(Input.mouseScrollDelta != Vector2.zero)
        {
            TranslateSpeed -= Input.mouseScrollDelta.y / 800;
            TranslateSpeed = Mathf.Max(TranslateSpeed, 0.05f);
            mainCamera.transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * ScrollSpeed, Space.Self);
        }
    }


}
