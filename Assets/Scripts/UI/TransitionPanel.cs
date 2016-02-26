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

        private EventDelegate transitionCallback;
        private EventDelegate finishCallback;

	    void Awake()
	    {
            transitionCallback = new EventDelegate(OnTransition);
            finishCallback = new EventDelegate(OnReverseFinished);
			tween = GetComponentInChildren<TweenAlpha>();
	    }

		public void StartTransition()
		{
            tween.AddOnFinished(transitionCallback);
            tween.PlayForward();
		}

		private void OnTransition()
		{
			CallbackTransition();
            tween.RemoveOnFinished(transitionCallback);
            tween.AddOnFinished(finishCallback);
            tween.PlayReverse();
		}

        private void OnReverseFinished()
        {
            tween.RemoveOnFinished(finishCallback);
            PopupManager.Instance.RemovePopup(this);
        }
	}	
}

