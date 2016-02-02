using UnityEngine;
using System.Collections;

using Base;
using GameLogic;

public class ScreenItem : MonoBehaviour
{
	[HideInInspector]
	public Transform CachedTransform;

	protected Camera mainCamera;

	protected virtual void Awake()
	{
		CachedTransform = transform;
		mainCamera = Camera.main;
	}

	public void UpdatePosition(Vector3 position)
	{
		Vector3 screenPos = mainCamera.WorldToScreenPoint(position);
		screenPos.z = 0f;
		screenPos = UICamera.currentCamera.ScreenToWorldPoint(screenPos);
		CachedTransform.position = screenPos;
	}
}

