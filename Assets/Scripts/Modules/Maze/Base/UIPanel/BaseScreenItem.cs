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
		mainCamera = Camera.main;
	}

	public void UpdatePosition(Vector3 position)
	{
		Vector3 screenPos = mainCamera.WorldToScreenPoint(position);
		screenPos.z = 0f;
		RectTransform.position = screenPos;
	}
}

