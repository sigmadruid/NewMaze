using System;
using System.Collections;

/// <summary>
/// 面板的特殊行为。
/// </summary>
public abstract class BasePopupAction
{
	protected IPopupManagerDelegate popupManagerDelegate;

	public BasePopupAction (uint popupMode, IPopupManagerDelegate popupManagerDelegate)
	{
		Mode = popupMode;

		DefaultParam = null;

		this.popupManagerDelegate = popupManagerDelegate;
	}

	/// <summary>
	/// 对应的模式标识
	/// </summary>
	public uint Mode { get; protected set; }

	/// <summary>
	/// 默认参数值
	/// </summary>
	public object DefaultParam { get; protected set; }

	/// <summary>
	/// 面板展示时的行为
	/// </summary>
	public abstract void ExecuteShow (BasePopupView view, object param);

	/// <summary>
	/// 面板隐藏时的行为
	/// </summary>
	public abstract void ExecuteHide (BasePopupView view, object param);
}

