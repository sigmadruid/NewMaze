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
	public class TalkPanel : BasePopupView 
	{
        public Action CallbackDialogFinish;

        public float TweenDuration = 0.3f;

        public Image ImageClick;
        public Image ImageBackground;
        public Text LabelTitle;
        public Text LabelPassage;
        public CanvasGroup TweenGroup;

		private int index;
        private List<string> talkList;
        private bool isTweening;

		void Awake () 
		{
            UILocalizer.LocalizeByName(transform);
		}

        public void Init(string nameID, List<string> talkList)
		{
            LabelTitle.text = TextDataManager.Instance.GetData(nameID);

			index = 0;
			this.talkList = talkList;

			LabelPassage.text = TextDataManager.Instance.GetData(talkList[index++]);
            Tween();
		}

        public void Continue()
        {
            if(isTweening)
                return;
            
            if (index < talkList.Count)
            {
                LabelPassage.text = TextDataManager.Instance.GetData(talkList[index++]);
                Tween();
            }
            else if (CallbackDialogFinish != null)
            {
                index = 0;
                CallbackDialogFinish();
            }
        }
		
        private void Tween()
        {
            isTweening = true;
            Tweener tweener = TweenGroup.DOFade(0, TweenDuration).From();
            tweener.OnComplete(() => { isTweening = false; });
        }
	}
}
