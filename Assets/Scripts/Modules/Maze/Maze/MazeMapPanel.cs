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
    private Vector3 heroPosition;
    private float heroAngle;

    public override void OnInitialize()
    {
        base.OnInitialize();
        mazeMapHero = ResourceManager.Instance.CreateGameObject("Heroes/MazeMapHero");
        mazeMapHero.SetActive(false);
    }

    public override void OnDispose()
    {
        base.OnDispose();
        ResourceManager.Instance.DestroyAsset(mazeMapHero);
        mazeMapHero = null;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        DoShowMap(true);
        EventTriggerListener.Get(innerWindow.gameObject).onDrag = OnDrag;
    }

    public override void BeforeExit()
    {
        base.BeforeExit();
        DoShowMap(false);
        EventTriggerListener.Get(innerWindow.gameObject).onDrag = null;
    }

	public void SetData(Vector3 position, float angle)
	{
        heroPosition = position;
        heroAngle = angle;
	}

    private void DoShowMap(bool show)
    {
        innerWindow.Display(show);
        innerWindow.Camera.enabled = show;
        mazeMapHero.SetActive(show);

        if (show)
        {
            innerWindow.CameraTransform.position = heroPosition + Vector3.forward * (-Distance) + Vector3.up * Height + Vector3.right * Distance;;
            innerWindow.CameraTransform.LookAt(heroPosition);
            mazeMapHero.transform.position = heroPosition;
            mazeMapHero.transform.localEulerAngles = Vector3.up * heroAngle;
        }
    }

	private void OnDrag(GameObject go, Vector2 delta)
	{
		Vector3 offset = new Vector3(-delta.x, 0, -delta.y) * DragSpeed;
		offset = Quaternion.Euler(Vector3.up * (-45)) * offset;
		innerWindow.CameraTransform.position += offset;
	}
}

