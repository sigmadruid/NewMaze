using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using DG.Tweening;

namespace GameUI
{
	public class TransitionPanel : BasePopupView
	{
        private const float LOOP_DURATION = 1f;

        public Action CallbackTransition;

        private CanvasGroup group;

        private float timer;
        private bool isPlaying;
        private bool hasCompleted;

	    void Awake()
	    {
            group = GetComponent<CanvasGroup>();
	    }

        void Update()
        {
            //Tweener's onStepComplete uses try...catch. It's hard to debug...
            if(isPlaying)
            {
                timer += Time.deltaTime;
                if(timer > LOOP_DURATION)
                {
                    OnStep();
                }
            }
        }

		public void Play()
		{
            timer = 0;
            isPlaying = true;
            hasCompleted = false;
            Tweener tweener = group.DOFade(1, LOOP_DURATION).SetLoops(2, LoopType.Yoyo);
		}

		private void OnStep()
		{
            if(!hasCompleted)
            {
                CallbackTransition();
            }
            else
            {
                isPlaying = false;
                PopupManager.Instance.RemovePopup(this);
            }
            
            hasCompleted = true;
		}

	}	
}

