using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using StaticData;

namespace Base
{
	public class PopupManager : IPopupManagerDelegate
	{
	    private static PopupManager instance;

		private BasePopupView currentQueuePopup;

        private Dictionary<string, BasePopupView> popupDic = new Dictionary<string, BasePopupView>();
        private List<BasePopupView> popupList = new List<BasePopupView>();
		private BasePopupAction[] actionList;

	    public static PopupManager Instance
	    {
	        get
	        {
	            if (instance == null) instance = new PopupManager();
	            return instance;
	        }
	    }

        public void Init()
        {
            Scaler.referenceResolution = new Vector2(1920f, 1080f);
            InputManager.Instance.UpdateRaycasters();
            actionList = new BasePopupAction[]
                {
//                    new AddMaskPopupAction(this),
//                    new HideCameraPopupAction(this),
//                    new AutoHidePopupAction(this),
                };
        }

		public void Clear()
		{
			currentQueuePopup = null;
			popupDic.Clear();
            popupList = new List<BasePopupView>();
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

		public T CreateAndAddPopup<T>() where T : BasePopupView
	    {
            string panelName = typeof(T).Name;
            UIData data = UIDataManager.Instance.GetData(panelName);
            string path = string.Format(PopupConst.UI_PANEL_PATH, panelName);

			T view = null;
            if (!popupDic.ContainsKey(data.PrefabPath))
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


            if(data.QueueMode == PopupQueueMode.NoQueue)
            {
                ShowPopup(view);
            }
            else
            {
                if (currentQueuePopup == null)
                {
                    currentQueuePopup = view;
                    ShowPopup(currentQueuePopup);
                }
                else if (data.QueueMode == PopupQueueMode.QueueBack)
                {
                    popupList.Add(view);
                }
                else if (data.QueueMode == PopupQueueMode.QueueFront)
                {
                    popupList.Insert(0, view);
                }
            }
            InputManager.Instance.UpdateRaycasters();

	        return view;
	    }

	    public void RemovePopup(BasePopupView view, bool cleanup = false)
	    {
			if (view == null)	return;

			HidePopup(view, cleanup);
	    }

		private void ShowPopup (BasePopupView view)
		{
            int index = GetMaxViewIndex();
            view.transform.SetSiblingIndex(index);
			view.gameObject.SetActive(true);
            view.OnEnter();
		}

        private void HidePopup (BasePopupView view, bool cleanup)
		{
            view.OnExit ();
            view.gameObject.SetActive(false);

            if (cleanup)
            {
                popupDic.Remove(view.PrefabPath);
                view.OnDispose();
                ResourceManager.Instance.DestroyAsset(view.gameObject);
            }

            if(view == currentQueuePopup)
            {
                if(popupList.Count > 0)
                {
                    currentQueuePopup = popupList[0];
                    popupList.RemoveAt(0);
                }
                else
                {
                    currentQueuePopup = null;
                }
                if (currentQueuePopup != null)
                {
                    ShowPopup(currentQueuePopup);
                }
            }
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
