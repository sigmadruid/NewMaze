using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using DG.Tweening;

namespace GameUI
{
	public class TransitionPanel : BasePopupView
	{
		public Utils.CallbackVoid CallbackTransition;

        private CanvasGroup group;

        private bool hasCompleted;

	    void Awake()
	    {
            group = GetComponent<CanvasGroup>();
	    }

		public void StartTransition()
		{
            hasCompleted = false;
            group.alpha = 0f;
            Tweener tweener =  group.DOFade(1f, 1f).SetLoops(2, LoopType.Yoyo);
            tweener.OnStepComplete(OnStep);
		}

		private void OnStep()
		{
            if (!hasCompleted)
			    CallbackTransition();
            else
                PopupManager.Instance.RemovePopup(this);
            
            hasCompleted = true;
		}

	}	
}

