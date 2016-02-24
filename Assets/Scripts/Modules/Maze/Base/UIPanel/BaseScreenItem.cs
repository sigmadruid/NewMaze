using UnityEngine;
using System.Collections;

using Base;
using GameLogic;

public class BaseScreenItem : MonoBehaviour
{
    [HideInInspector]
    public RectTransform RectTransform;

    protected Camera mainCamera;

    protected virtual void Awake()
    {
        RectTransform = transform as RectTransform;
        mainCamera = Camera3DScript.Instance.Camera;
    }

	public void UpdatePosition(Vector3 position)
	{
		Vector3 screenPos = mainCamera.WorldToScreenPoint(position);
        RectTransform.anchoredPosition = screenPos;
	}
}

