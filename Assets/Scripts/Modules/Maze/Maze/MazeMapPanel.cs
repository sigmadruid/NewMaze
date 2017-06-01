using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        DragEventTrigger.Get(innerWindow.gameObject).onDrag = OnDrag;
    }

    public override void OnExit()
    {
        base.OnExit();
        DragEventTrigger.Get(innerWindow.gameObject).onDrag = null;
    }

    public void Show(bool show, Vector3 position, float angle)
    {
        innerWindow.Display(show);
        innerWindow.Camera.enabled = show;
        mazeMapHero.SetActive(show);

        if (show)
        {
            innerWindow.CameraTransform.position = position + Vector3.forward * (-Distance) + Vector3.up * Height + Vector3.right * Distance;;
            innerWindow.CameraTransform.LookAt(position);
            mazeMapHero.transform.position = position;
            mazeMapHero.transform.localEulerAngles = Vector3.up * angle;
        }
    }

    public void OnDrag(GameObject go, Vector2 delta)
	{
        Vector3 offset = new Vector3(-delta.x, 0, -delta.y) * DragSpeed;
		offset = Quaternion.Euler(Vector3.up * (-45)) * offset;
		innerWindow.CameraTransform.position += offset;
	}
}

