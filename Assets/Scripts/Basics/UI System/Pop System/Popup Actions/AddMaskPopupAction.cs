using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public struct AddMaskParam
	{
		public bool clickHide;
	}

	/// <summary>
	/// 添加遮罩行为.
	/// 参数：AddMaskDTO
	/// </summary>
	public class AddMaskPopupAction : BasePopupAction
	{
		private const int Z_MASK_INCREMENT = 5;

        private MaskPanel mask;

		private BasePopupView currentPopup;

		public AddMaskPopupAction (IPopupManagerDelegate popupManagerDelegate) : base(PopupMode.ADD_MASK, popupManagerDelegate)
		{
			DefaultParam = false;
		}

		public override void ExecuteShow (BasePopupView view, object param = null)
		{
			if (param != null)
			{
                AddMaskParam maskParam = (AddMaskParam)param;
				currentPopup = view;
				
                if(mask == null)
                    CreateMask();

                int index = view.transform.GetSiblingIndex();
                mask.transform.SetSiblingIndex(index - 1);
                mask.gameObject.SetActive(true);
				
				if (maskParam.clickHide)
                    ClickEventTrigger.Get(mask.gameObject).onClick = onBlackMaskClick;
			}
		}

		public override void ExecuteHide (BasePopupView view, object param = null)
		{
            mask.gameObject.SetActive(false);
            ClickEventTrigger.Get(mask.gameObject).onClick = null;
		}

		private void onBlackMaskClick(GameObject go)
		{
            ClickEventTrigger.Get(go).onClick = null;
			popupManagerDelegate.RemovePopup(currentPopup);
		}


        private void CreateMask()
        {
            string path = string.Format(PopupConst.UI_PANEL_PATH, "MaskPanel");
            mask = ResourceManager.Instance.CreateAsset<MaskPanel>(path);
            mask.transform.SetParent(RootTransform.Instance.UIPanelRoot);
            mask.transform.localPosition = Vector3.zero;
            mask.transform.localScale = Vector3.one;
            mask.RectTransform.offsetMin = Vector2.zero;
            mask.RectTransform.offsetMax = Vector2.zero;
        }
	}
}

