using System;

namespace Base
{
	/// <summary>
	/// 面板队列模式
	/// </summary>
	public enum PopupQueueMode
	{
        NoQueue,
		QueueBack,
		QueueFront,
	}

	public enum PopupAnimationType
	{
		Expand,
		Floating,
	}

	/// <summary>
	/// 面板行为标识
	/// </summary>
	public class PopupMode
	{
		/// <summary>
		/// 是否立即展示面板
		/// </summary>
		public const uint SHOW = 1 << 0;

		/// <summary>
		/// 是否添加遮罩层
		/// </summary>
		public const uint ADD_MASK = 1 << 1;

		/// <summary>
		/// 是否隐藏摄像机
		/// </summary>
		public const uint HIDE_CAMERA = 1 << 2;

		/// <summary>
		/// 面板是否在指定时间后自动隐藏
		/// </summary>
		public const uint AUTO_HIDE = 1 << 3;

		/// <summary>
		/// 面板是否展示动画
		/// </summary>
		public const uint ANIMATED = 1 << 10;

		public const uint DEFAULT = SHOW;
	}

	public class PopupConst
	{
        public const string UI_ITEM_PATH = "NewUI/Items/{0}";
        public const string UI_PANEL_PATH = "NewUI/{0}";
	}
}

