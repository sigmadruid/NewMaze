using UnityEngine;
using UnityEngine.UI;
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
        CanvasScaler scaler = PopupManager.Instance.Scaler;
        float ratio = Screen.width / scaler.referenceResolution.x;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(position) / ratio;
        RectTransform.anchoredPosition = screenPos;
	}
}

