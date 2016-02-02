using UnityEngine;
using System.Collections;

using PureMVC.Patterns;
using PureMVC.Interfaces;

public abstract class UIMediator : Mediator 
{
	protected IPopupManagerDelegate popupManagerDelegate;

	public UIMediator(string mediatorName): base(mediatorName, null) { }

	public override void OnRegister ()
	{
		popupManagerDelegate = PopupManager.Instance;
	}

	protected abstract void showView (object param = null);

	protected abstract void hideView (object param = null);

	protected abstract void updateViewShow ();

	protected abstract void updateViewHide ();
}
