using UnityEngine;

using System;

using Base;

namespace GameUI
{
    public class ConfirmPanel : BasePopupView
    {
        public Action CallbackConfirm;
        public Action CallbackCancel;
        
        public UILabel LabelTitle;
        public UILabel LabelContent;
        
        public UIButton ButtonConfirm;
        public UIButton ButtonCancel;
        
		public static void Show(string title, string content, Action callbackConfirm, Action callbackCancel)
        {
			ConfirmPanel panel = PopupManager.Instance.CreateAndAddPopup<ConfirmPanel>(PopupMode.DEFAULT, PopupQueueMode.NoQueue);
			panel.LabelTitle.text = title;
			panel.LabelContent.text = content;
			panel.CallbackConfirm = callbackConfirm;
			panel.CallbackCancel = callbackCancel;
        }
        
        void Awake()
        {
            UIEventListener.Get(ButtonConfirm.gameObject).onClick = OnConfirm;
        }
        
        private void OnConfirm(GameObject go)
        {
            CallbackConfirm();
        }
		private void OnCancel(GameObject go)
		{
			CallbackCancel();
		}
    }
}

