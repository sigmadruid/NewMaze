using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
    public class UIAnimationParam
    {
        public BasePopupView View;

        public bool Cleanup;

        public UIAnimationParam(BasePopupView view, bool cleanup)
        {
            View = view;
            Cleanup = cleanup;
        }
    }
	public class PopupManager : IPopupManagerDelegate
	{
	    private static PopupManager instance;

        private BasePopupView mask;
		private BasePopupView currentQueuePopup;

        private Dictionary<string, BasePopupView> popupDic = new Dictionary<string, BasePopupView>();
        private PopupQueue popupQueue = new PopupQueue();
		private BasePopupAction[] actionList;

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

        public void Init()
        {
            Scaler.referenceResolution = new Vector2(1920f, 1080f);
            actionList = new BasePopupAction[]
                {
                    new AddMaskPopupAction(this),
                    new HideCameraPopupAction(this),
                    new AutoHidePopupAction(this),
                };
        }

		public void Clear()
		{
			currentQueuePopup = null;
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
                view.gameObject.SetActive(false);
                view.transform.SetParent(RootTransform.Instance.UIPanelRoot);
                view.transform.localPosition = Vector3.zero;
                view.transform.localScale = Vector3.one;
                view.RectTransform.offsetMin = Vector2.zero;
                view.RectTransform.offsetMax = Vector2.zero;
				view.PrefabPath = path;
				view.OnInitialize();
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

                if(queueMode == PopupQueueMode.NoQueue)
                {
                    showPopup(view);
                }
                else
                {
                    if (currentQueuePopup == null)
                    {
                        currentQueuePopup = view;
                        showPopup(currentQueuePopup);
                    }
                    else if (queueMode == PopupQueueMode.QueueBack)
                    {
                        popupQueue.AddToBack(view);
                    }
                    else if (queueMode == PopupQueueMode.QueueFront)
                    {
                        popupQueue.AddToFront(view);
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

            if (!view.IsAnimationPlaying)
				hidePopup(view, cleanup);
	    }

		private void showPopup (BasePopupView view)
		{
            int index = GetMaxViewIndex();
            view.transform.SetSiblingIndex(index);
			view.gameObject.SetActive(true);
			executeAction(view, true);
			view.BeforeEnter();

			bool isAnimated = checkMode(view.popupMode, PopupMode.ANIMATED);
            UIAnimationParam param = new UIAnimationParam(view, false);
			if (isAnimated)
			{
				view.StartAnimation(showPopupCallback, param);
			}
			else
			{
				showPopupCallback(param);
			}
		}

        private void showPopupCallback (UIAnimationParam param)
		{
            BasePopupView view = param.View;
            view.transform.localScale = Vector3.one;
			
            view.OnEnter();
            if (view.UpdateShowDelegate != null)
                view.UpdateShowDelegate.Invoke();
		}

        private void hidePopup (BasePopupView view, bool cleanup)
		{
			view.BeforeExit();
			if (view.UpdateHideDelegate != null)
				view.UpdateHideDelegate.Invoke();

			bool isAnimated = checkMode(view.popupMode, PopupMode.ANIMATED);
            UIAnimationParam param = new UIAnimationParam(view, cleanup);
			if (isAnimated)
			{
                view.StartAnimation(hidePopupCallback, param);
			}
			else
			{
				hidePopupCallback(param);
			}
		}

        private void hidePopupCallback (UIAnimationParam param)
		{
            BasePopupView view = param.View;
            view.OnExit ();
            executeAction(view, false);
            view.gameObject.SetActive(false);

            if (param.Cleanup)
			{
                popupDic.Remove(view.PrefabPath);
                view.OnDispose();
                ResourceManager.Instance.DestroyAsset(view.gameObject);
			}

            if(view == currentQueuePopup)
            {
                currentQueuePopup = popupQueue.Unshift();
                if (currentQueuePopup != null)
                {
                    showPopup(currentQueuePopup);
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
					AddMaskParam dto = new AddMaskParam();
					dto.clickHide = (bool)param;
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
		}

		private bool checkMode (uint popupMode, uint mode)
		{
			return (popupMode & mode) > 0;
		}


        private CanvasScaler scaler;
        public CanvasScaler Scaler
        {
            get
            {
                if(scaler == null)
                {
                    scaler = GameObject.FindObjectOfType<CanvasScaler>();
                }            
                return scaler;
            }
        }

        private int GetMaxViewIndex()
        {
            int count = RootTransform.Instance.UIPanelRoot.childCount;
            return count - 1;
        }

	}
}
