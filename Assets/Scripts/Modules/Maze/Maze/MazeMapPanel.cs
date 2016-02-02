using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

public class MazeMapPanel : BasePopupView
{
	public float DragSpeed;

	public float Height;
	public float Distance;

	public InnerWindow innerWindow;

	void Start()
	{
		UIEventListener.Get(innerWindow.gameObject).onDrag = OnDrag;
	}

	public void Show(bool show, Vector3 heroPosition)
	{
		innerWindow.Display(show);
		innerWindow.Camera.enabled = show;
		if (show)
		{
			innerWindow.CameraTransform.position = heroPosition + Vector3.forward * (-Distance) + Vector3.up * Height + Vector3.right * Distance;;
			innerWindow.CameraTransform.LookAt(heroPosition);
		}
	}

	private void OnDrag(GameObject go, Vector2 delta)
	{
		Vector3 offset = new Vector3(-delta.x, 0, -delta.y) * DragSpeed;
		offset = Quaternion.Euler(Vector3.up * (-45)) * offset;
		innerWindow.CameraTransform.position += offset;
	}
}

