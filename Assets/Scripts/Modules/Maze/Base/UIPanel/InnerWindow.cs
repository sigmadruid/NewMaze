using UnityEngine;
using System.Collections;

namespace GameLogic
{
    public class InnerWindow : MonoBehaviour 
    {
    	public Transform BottomLeft;
    	public Transform TopRight;

        private CameraInnerScript innerCamera;

    	private bool initialized = false;

        void Start()
        {
            Display(true);
        }

    	private void Initialize ()
        {
            innerCamera = CameraInnerScript.Instance;
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
                innerCamera.Camera.pixelRect = new Rect();
    		}
            else
            {

                RectTransform rectTransform = transform as RectTransform;

                Vector2 startPosition = new Vector2(rectTransform.anchorMin.x * rectTransform.rect.width, 
                                                    rectTransform.anchorMin.y * rectTransform.rect.height);
                Vector2 endPosition   = new Vector2(rectTransform.anchorMax.x * rectTransform.rect.width, 
                                                    rectTransform.anchorMax.y * rectTransform.rect.height);
                Debug.Log(startPosition);
                Debug.Log(endPosition);
                Rect viewRect = new Rect(
    				startPosition.x, 
    				startPosition.y, 
                    endPosition.x - startPosition.x,
                    endPosition.y - startPosition.y
                    );
                innerCamera.Camera.pixelRect = viewRect;
            }
    	}

    	public Camera Camera
    	{
    		get
    		{
                return innerCamera.Camera;
    		}
    	}
    	public Transform CameraTransform
    	{
    		get
    		{
                return innerCamera.transform;
    		}
    	}
    }
}
