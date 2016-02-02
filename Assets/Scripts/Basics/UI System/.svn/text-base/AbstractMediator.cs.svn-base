using UnityEngine;
using System.Collections;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public abstract class AbstractMediator : Mediator {

	public AbstractMediator(string mediatorName)
		: base(mediatorName, null) { }

	protected void Open ()
	{
		OnOpen();

		SendNotification(NotificationConst.HIDE_MAIN_CAMERA);
	}

	protected void Close ()
	{
		SendNotification(NotificationConst.SHOW_MAIN_CAMERA);

		OnClose();
	}

	protected void CloseForOpen (string notification, object param = null)
	{
		OnClose();

		SendNotification(notification, param);
	}

	protected abstract void OnOpen ();

	protected abstract void OnClose ();
}
