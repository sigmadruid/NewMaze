using System;
using System.Collections;

namespace Base
{
	/// <summary>
	/// 显示弹窗时是否隐藏摄像机
	/// 参数：无
	/// </summary>
	public class HideCameraPopupAction : BasePopupAction
	{
		public HideCameraPopupAction (IPopupManagerDelegate popupManagerDelegate) : base (PopupMode.HIDE_CAMERA, popupManagerDelegate)
		{
		}
		
		public override void ExecuteShow (BasePopupView view, object param = null)
		{
//		ApplicationFacade.Instance.DispatchNotification(NotificationConst.HIDE_MAIN_CAMERA);
		}
		
		public override void ExecuteHide (BasePopupView view, object param = null)
		{
//		ApplicationFacade.Instance.DispatchNotification(NotificationConst.SHOW_MAIN_CAMERA);
		}
	}
}
