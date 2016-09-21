using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public class PopupManager : IPopupManagerDelegate
	{
	    private static PopupManager instance;

		/// <summary>
		/// 当前弹窗的深度值
		/// </summary>
		private int popupDepth = 0;

		/// <summary>
		/// 深度增量值
		/// </summary>
		private const int DEPTH_INCREMENT = 10;

		/// <summary>
		/// 当前展示的面板
		/// </summary>
		private BasePopupView currentPopup;

		/// <summary>
		/// 用一个字典缓存使用过的面板。键值为面板的路径。
		/// </summary>
		private Dictionary<string, BasePopupView> popupDic;

		/// <summary>
		/// 面板队列
		/// </summary>
		private PopupQueue popupQueue;

		/// <summary>
		/// T面板特殊行为列表。所有特殊行为都要加入进来。
		/// </summary>
		private readonly BasePopupAction[] actionList;

		/// <summary>
		/// 动画管理器
		/// </summary>
		private PopupAnimationManager animationManager;

	    public static PopupManager Instance
	    {
	        get
	        {
	            if (instance == null)
	            {
	                instance = new PopupManager();
	            }
	            return instance;
	        }
	    }

		public PopupManager ()
		{
			popupDic = new Dictionary<string, BasePopupView>();
			popupQueue = new PopupQueue();

			actionList = new BasePopupAction[]
			{
				new AddMaskPopupAction(this),
				new HideCameraPopupAction(this),
				new AutoHidePopupAction(this),
			};

			animationManager = new PopupAnimationManager();
		}

		/// <summary>
		/// 清理PopupManager。每次切换场景时都应该调用该方法。
		/// </summary>
		public void Clear()
		{
			currentPopup = null;
			popupDic.Clear();
			popupQueue = new PopupQueue();
		}

        public T CreateItem<T>(Transform parentTrans) where T : MonoBehaviour
        {
            string path = string.Format(PopupConst.UI_ITEM_PATH, typeof(T).Name);
            T item = ResourceManager.Instance.LoadAsset<T>(ObjectType.GameObject, path);
            item.transform.SetParent(parentTrans);
            item.transform.localScale = Vector3.one;
            return item;
        }

        public void RemoveItem(GameObject item)
        {
            ResourceManager.Instance.RecycleAsset(item);
        }

		/// <summary>
		/// 创建并展示一个面板。面板会被缓存，因此该方法可重复调用。
		/// </summary>
		/// <param name="path">弹窗路径.</param>
		/// <param name="popupMode">模式标识.</param>
		/// <param name="queueMode">队列标识.</param>POP_CANVAS_LAYER_NAME
		/// <param name="paramDic">参数字典.</param>
		/// <returns>The and add popup.</returns>
		public T CreateAndAddPopup<T>(uint popupMode = PopupMode.DEFAULT, 
		                              PopupQueueMode queueMode = PopupQueueMode.QueueBack, 
		                              Dictionary<uint, object> paramDic = null) where T : BasePopupView
	    {
			bool show = checkMode(popupMode, PopupMode.SHOW);
            string path = string.Format(PopupConst.UI_PANEL_PATH, typeof(T).Name);

			T view = null;
			if (!popupDic.ContainsKey(path))
			{
				view = ResourceManager.Instance.CreateAsset<T>(path);
                view.transform.SetParent(RootTransform.Instance.UIPanelRoot);
                view.transform.localPosition = Vector3.zero;
                view.RectTransform.offsetMin = Vector2.zero;
                view.RectTransform.offsetMax = Vector2.zero;
				view.PrefabPath = path;
				view.onInitialize();
				popupDic[path] = view;
			}
			else
			{
				view = popupDic[path] as T;
			}

	        if (show)
	        {
				view.popupMode = popupMode;
				view.paramDic = paramDic;
				if (queueMode == PopupQueueMode.NoQueue)
				{
					showPopup(view);
				}
				else 
				{
					if (currentPopup == null)
					{
						currentPopup = view;
						showPopup(currentPopup);
					}
					else if (queueMode == PopupQueueMode.QueueBack)
					{
						popupQueue.AddToBack(view);
					}
					else if (queueMode == PopupQueueMode.QueueFront)
					{
						popupQueue.AddToFront(view);
	                }
	                else if (queueMode == PopupQueueMode.QueueFrontShow)
	                {
						if (currentPopup.popupAnimation.IsPlayingReverse)
						{
							popupQueue.AddToFront(view);
						}
						else
						{
							popupQueue.AddToFront(currentPopup);
							popupQueue.AddToFront(view);
							hidePopup(currentPopup);
						}
					}
				}
	        }

	        return view;
	    }

		/// <summary>
		/// 移除一个已知面板。
		/// </summary>
		/// <param name="view">面板.</param>
		/// <param name="cleanup">是否析构该面板。</param>
	    public void RemovePopup(BasePopupView view, bool cleanup = false)
	    {
			if (view == null)	return;

			if (view.popupAnimation == null || !view.popupAnimation.IsPlayingReverse)
				hidePopup(view, cleanup);
	    }

		private void showPopup (BasePopupView view, object param = null)
		{
			popupDepth += DEPTH_INCREMENT;
			setPanelDepths(view, popupDepth);

			view.gameObject.SetActive(true);
			executeAction(view, true);
			view.beforeEnter ();

			bool isAnimated = checkMode(view.popupMode, PopupMode.ANIMATED);
			if (isAnimated)
			{
				view.popupAnimation = animationManager.CreateAnimation(view.AnimationType);
				view.StartAnimation(false, showPopupCallback, param);
			}
			else
			{
				showPopupCallback(view, param);
			}
		}

		private void showPopupCallback (BasePopupView view, object param)
		{
			view.transform.localScale = Vector3.one;
			
			view.onEnter();
			if (view.UpdateShowDelegate != null)
				view.UpdateShowDelegate.Invoke();
		}

		private void hidePopup (BasePopupView view, object param = null)
		{
			view.onExit();
			if (view.UpdateHideDelegate != null)
				view.UpdateHideDelegate.Invoke();

			bool isAnimated = checkMode(view.popupMode, PopupMode.ANIMATED);
			if (isAnimated)
			{
				view.popupAnimation = animationManager.CreateAnimation(view.AnimationType);
				view.StartAnimation(true, hidePopupCallback, param);
			}
			else
			{
				hidePopupCallback(view, param);
			}
		}

		private void hidePopupCallback (BasePopupView view, object param)
		{
			view.afterExit ();
			executeAction(view, false);
			view.gameObject.SetActive(false);

			setPanelDepths(view, -popupDepth);
			popupDepth -= DEPTH_INCREMENT;

			bool cleanup = param != null ? (bool)param : false;
			if (cleanup)
			{
				popupDic.Remove(view.PrefabPath);
				view.onDispose();
				ResourceManager.Instance.DestroyAsset(view.gameObject);
			}

			if (ReferenceEquals(view, currentPopup))
			{
				currentPopup = popupQueue.Unshift();
				if (currentPopup != null)
				{
					showPopup(currentPopup);
				}
			}
		}

		private void executeAction(BasePopupView view, bool show)
		{
			for (int i = 0, imax = actionList.Length; i < imax; ++i)
			{
				BasePopupAction action = actionList[i];

				if (checkMode(view.popupMode, action.Mode))
				{
					object param = generateParam(action, view.paramDic);
					if (show)
	                	action.ExecuteShow(view, param);
					else
						action.ExecuteHide(view, param);
	            }
	        }
	    }

		/// <summary>
		/// 根据行为类型和参数字典，生成面板行为所需的特殊参数。
		/// </summary>
		private object generateParam(BasePopupAction action, Dictionary<uint, object> paramDic)
		{
			object resultParam = null;
			object param;

			if (paramDic == null || !paramDic.ContainsKey(action.Mode))
			{
				param = action.DefaultParam;
			}
			else
			{
				param = paramDic[action.Mode];
			}

			switch (action.Mode)
			{
				case PopupMode.ADD_MASK:
				{
					AddMaskDTO dto = new AddMaskDTO();
					dto.depth = popupDepth;
					dto.clickHide = (bool)param;
					dto.popupRoot = RootTransform.Instance.UIPanelRoot;
					resultParam = dto;
					break;
				}
				default:
				{
					resultParam = param;
					break;
				}
			}
	//		Debug.Log(action.ToString() + ": " + param);
			return resultParam;
		}

		private void setPanelDepths(BasePopupView view, int depth)
		{
//			view.GetComponent<UIPanel>().depth += depth;
		}

		private bool checkMode (uint popupMode, uint mode)
		{
			return (popupMode & mode) > 0;
		}

	}
}
