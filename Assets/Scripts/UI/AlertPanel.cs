using UnityEngine;

using System;

using Base;

namespace GameUI
{
    public class AlertPanel : BasePopupView
    {
		public Action CallbackOK;

		public UILabel LabelTitle;

		public UILabel LabelContent;

		public UIButton ButtonOK;

		public static void Show(string title, string content, Action callbackOK)
		{
			AlertPanel panel = PopupManager.Instance.CreateAndAddPopup<AlertPanel>(PopupMode.DEFAULT, PopupQueueMode.NoQueue);
			panel.LabelTitle.text = title;
			panel.LabelContent.text = content;
			panel.CallbackOK = callbackOK;
		}

		void Awake()
		{
			UIEventListener.Get(ButtonOK.gameObject).onClick = OnOK;
		}

		private void OnOK(GameObject go)
		{
			CallbackOK();
		}
    }
}

