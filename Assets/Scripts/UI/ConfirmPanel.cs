using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using GameLogic;

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
            ConfirmPanel panel = PopupManager.Instance.CreateAndAddPopup<ConfirmPanel>(PopupMode.SHOW | PopupMode.ADD_MASK);
			panel.LabelTitle.text = title;
			panel.LabelContent.text = content;
			panel.CallbackConfirm = callbackConfirm;
			panel.CallbackCancel = callbackCancel;
        }
        
        void Awake()
        {
            ClickEventTrigger.Get(ButtonConfirm.gameObject).onClick = OnConfirm;
            ClickEventTrigger.Get(ButtonCancel.gameObject).onClick = OnCancel;

            UILocalizer.Replace(transform);
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

