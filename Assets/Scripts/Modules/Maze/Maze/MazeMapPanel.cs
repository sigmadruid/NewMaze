using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

public class MazeMapPanel : BasePopupView
{
	public float DragSpeed;

	public float Height;
	public float Distance;

	public InnerWindow innerWindow;

    private GameObject mazeMapHero;

    void Awake()
    {
        mazeMapHero = ResourceManager.Instance.CreateGameObject("Heroes/MazeMapHero");
        mazeMapHero.SetActive(false);
    }

	void Start()
	{
        EventTriggerListener.Get(innerWindow.gameObject).onDrag = OnDrag;
	}

    void OnDestroy()
    {
        Destroy(mazeMapHero);
        mazeMapHero = null;
    }


	public void Show(bool show, Vector3 heroPosition, float angle)
	{
		innerWindow.Display(show);
        innerWindow.Camera.enabled = show;
        mazeMapHero.SetActive(show);

		if (show)
		{
			innerWindow.CameraTransform.position = heroPosition + Vector3.forward * (-Distance) + Vector3.up * Height + Vector3.right * Distance;;
			innerWindow.CameraTransform.LookAt(heroPosition);
            mazeMapHero.transform.position = heroPosition;
            mazeMapHero.transform.localEulerAngles = Vector3.up * angle;
        }

	}

	private void OnDrag(GameObject go, Vector2 delta)
	{
		Vector3 offset = new Vector3(-delta.x, 0, -delta.y) * DragSpeed;
		offset = Quaternion.Euler(Vector3.up * (-45)) * offset;
		innerWindow.CameraTransform.position += offset;
	}
}

