using UnityEngine;
using System;
using System.Collections;

namespace Base
{
	/// <summary>
	/// 一段时间间隔后自动隐藏弹窗。
	/// 参数：float 时间间隔 -1f
	/// </summary>
	public class AutoHidePopupAction : BasePopupAction
	{
		public AutoHidePopupAction (IPopupManagerDelegate popupManagerDelegate) : base (PopupMode.AUTO_HIDE, popupManagerDelegate)
		{
			DefaultParam = -1f;
		}
		
		public override void ExecuteShow (BasePopupView view, object param)
		{
			float autoRemoveDelay = (float)param;
			if (autoRemoveDelay > 0)
				view.StartCoroutine(AutoRemovePopup(view, autoRemoveDelay));
		}
		
		public override void ExecuteHide (BasePopupView view, object param)
		{
		}

		private IEnumerator AutoRemovePopup(BasePopupView view, float autoRemoveDelay)
		{
			yield return new WaitForSeconds(autoRemoveDelay);
			
			popupManagerDelegate.RemovePopup(view, true);
		}
	}
}
