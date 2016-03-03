using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameUI
{
	public class DialogPanel : BasePopupView 
	{
        public Action CallbackDialogFinish;

        public Image ImageBackground;
		public Text LabelTitle;
        public Text LabelPassage;

		private int index;
		private List<int> talkList;

		void Awake () 
		{
            EventTriggerListener.Get(ImageBackground.gameObject).onClick = OnDialogClick;
		}

		public void Init(int nameCode, List<int> talkList)
		{
			LabelTitle.text = TextDataManager.Instance.GetData(nameCode);

			index = 0;
			this.talkList = talkList;

			LabelPassage.text = TextDataManager.Instance.GetData(talkList[index++]);
		}
		
		private void OnDialogClick(GameObject go)
		{
			if (index < talkList.Count)
			{
				LabelPassage.text = TextDataManager.Instance.GetData(talkList[index++]);
			}
			else if (CallbackDialogFinish != null)
			{
				index = 0;
				CallbackDialogFinish();
			}
		}
	}
}
