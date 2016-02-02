using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameUI
{
	public class TransitionPanel : BasePopupView
	{
		public Utils.CallbackVoid CallbackTransition;

		private TweenAlpha tween;

		private EventDelegate callback;

	    void Awake()
	    {
			callback = new EventDelegate(OnTransition);
			tween = GetComponentInChildren<TweenAlpha>();
	    }

		public void StartTransition()
		{
			tween.PlayForward();
			tween.AddOnFinished(callback);
		}

		private void OnTransition()
		{
			CallbackTransition();
			tween.PlayReverse();
			tween.RemoveOnFinished(callback);
		}
	}	
}

