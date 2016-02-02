using UnityEngine;
using System.Collections;

public class InnerWindow : MonoBehaviour 
{
	public Transform BottomLeft;
	public Transform TopRight;

	private Camera innerCamera;
	private Transform innerCameraTransform;

	private bool initialized = false;

	private void Initialize ()
    {
        innerCamera = GameObject.Find("InnerCamera").GetComponent<Camera>();
		innerCameraTransform = innerCamera.transform;
        initialized = true;
    }

	public void Display(bool state)
	{
        if (!initialized)
        {
            Initialize();
        }

        if (!state)
		{
			innerCamera.pixelRect = new Rect();
		}
        else
        {
			UIWidget widget = GetComponent<UIWidget>();

			TopRight.localPosition = widget.cachedTransform.localPosition + new Vector3(widget.width, widget.height, 0);

			Vector3 startPosition = UICamera.mainCamera.WorldToScreenPoint(BottomLeft.position);
			Vector3 endPosition = UICamera.mainCamera.WorldToScreenPoint(TopRight.position);
            Rect viewRect = new Rect(
				startPosition.x, 
				startPosition.y, 
				(endPosition.x - startPosition.x) * 2,
				(endPosition.y - startPosition.y) * 2
                );
            innerCamera.pixelRect = viewRect;
        }
	}

	public Camera Camera
	{
		get
		{
			return innerCamera;
		}
	}
	public Transform CameraTransform
	{
		get
		{
			return innerCameraTransform;
		}
	}
}
