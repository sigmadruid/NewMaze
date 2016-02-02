using UnityEngine;

using System;
using System.Collections;

using Base;

namespace GameUI
{
	public class TopAlertItem : MonoBehaviour
	{
		public Utils.CallbackParam CallbackDisappear;

		private UILabel label;

		private TweenAlpha tween;

		private Color originColor;

		private float hideDelay;

		void Awake()
		{
			label = GetComponent<UILabel>();
			tween = GetComponent<TweenAlpha>();
			
			originColor = label.color;
		}

		public void Init(string content, Utils.CallbackParam callbackDisappear, float delay = 1f)
		{
			hideDelay = delay;

			tween.delay = 0f;
			tween.SetOnFinished(OnStart);
			tween.PlayForward();
			
			label.color = originColor;
			label.text = content;
			CallbackDisappear = callbackDisappear;
		}

		private void OnStart()
		{
			tween.delay = hideDelay;
			tween.SetOnFinished(OnEnd);
			tween.PlayReverse();
		}
		private void OnEnd()
		{
			CallbackDisappear(this);
		}
	}
}

