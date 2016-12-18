using UnityEngine;
using UnityEngine.UI;

using System;

using Base;

namespace GameUI
{
    public class ConfirmPanel : BasePopupView
    {
        public Action CallbackConfirm;
        public Action CallbackCancel;
        
        public Text LabelTitle;
        public Text LabelContent;
        
        public Button ButtonConfirm;
        public Button ButtonCancel;
        
		public static void Show(string title, string content, Action callbackConfirm, Action callbackCancel = null)
        {
			ConfirmPanel panel = PopupManager.Instance.CreateAndAddPopup<ConfirmPanel>(PopupMode.DEFAULT, PopupQueueMode.NoQueue);
			panel.LabelTitle.text = title;
			panel.LabelContent.text = content;
			panel.CallbackConfirm = callbackConfirm;
			panel.CallbackCancel = callbackCancel;
        }
        
        void Awake()
        {
            EventTriggerListener.Get(ButtonConfirm.gameObject).onClick = OnConfirm;
            EventTriggerListener.Get(ButtonCancel.gameObject).onClick = OnCancel;
        }
        
        private void OnConfirm(GameObject go)
        {
            if (CallbackConfirm != null)
                CallbackConfirm();
            Dispose();
        }
		private void OnCancel(GameObject go)
		{
            if (CallbackCancel != null)
			    CallbackCancel();
            Dispose();
		}

        private void Dispose()
        {
            PopupManager.Instance.RemovePopup(this);
            CallbackConfirm = null;
            CallbackCancel = null;
        }
    }
}

