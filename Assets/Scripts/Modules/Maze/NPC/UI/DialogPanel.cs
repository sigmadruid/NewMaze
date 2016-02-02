using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameUI
{
	public class DialogPanel : BasePopupView 
	{
		public Utils.CallbackVoid CallbackClick;

		public UISprite SpriteBackground;
		public UILabel LabelTitle;
		public UILabel LabelPassage;

		private int index;
		private List<int> talkList;

		void Awake () 
		{
			UIEventListener.Get(SpriteBackground.gameObject).onClick = OnDialogClick;
		}

		public void InitData(int nameCode, List<int> talkList)
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
			else if (CallbackClick != null)
			{
				index = 0;
				CallbackClick();
			}
		}
	}
}
