using UnityEngine;
using System;

public class ExpandPopupAnimation : BasePopupAnimation
{
	private const float DURATION = 0.2f;

	private const float ORIGIN_SCALE = 0.2f;

	public ExpandPopupAnimation () : base(PopupAnimationType.Expand, DURATION)
	{
	}

	public override void StartAnimation (BasePopupView popupView, float duration, bool reverse = false)
	{
		base.StartAnimation (popupView, duration, reverse);

		if (!reverse)
		{
			popupView.transform.localScale = Vector3.one * ORIGIN_SCALE;
			TweenScale.Begin(popupView.gameObject, duration, Vector3.one);
		}
		else
		{
			popupView.transform.localScale = Vector3.one;
			TweenScale.Begin(popupView.gameObject, duration, Vector3.one * ORIGIN_SCALE);
		}

	}
}

