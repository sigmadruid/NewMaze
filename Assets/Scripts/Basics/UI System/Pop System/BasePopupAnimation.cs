using UnityEngine;
using System;
using System.Collections;

namespace Base
{
	public class BasePopupAnimation
	{
		public delegate void AnimationDelegate (BasePopupView view, object param);

		public AnimationDelegate OnAnimationEnds;
		public object endParam;

		public PopupAnimationType AnimationType { get; protected set; }
		
		public float DefaultDuration { get; protected set; }

		public bool IsPlaying { get; private set; }
		public bool IsPlayingReverse { get; private set; }

		private bool reverse;

		public BasePopupAnimation (PopupAnimationType animationType, float duration)
		{
			this.AnimationType = animationType;
			DefaultDuration = duration;
		}

		public virtual void StartAnimation (BasePopupView popupView, float duration, bool reverse = false)
		{
			this.reverse = reverse;

			setPlayingState(true);

			popupView.StopAllCoroutines();
			popupView.StartCoroutine(AnimationEnds(popupView, duration));

		}

		protected virtual IEnumerator AnimationEnds(BasePopupView popupView, float duration)
		{
			yield return new WaitForSeconds(duration);
			setPlayingState(false);

			if (OnAnimationEnds != null && popupView != null) 
				OnAnimationEnds(popupView, endParam);
		}

		private void setPlayingState(bool state)
		{
			if (!reverse)
				IsPlaying = state;
			else
				IsPlayingReverse = state;
		}
	}
}
