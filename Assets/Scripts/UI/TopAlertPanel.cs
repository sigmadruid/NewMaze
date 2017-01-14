using UnityEngine;

using System;

using Base;

namespace GameUI
{
	public class TopAlertPanel : BasePopupView
	{
//		public Transform ItemRoot;
//
//		private static TopAlertPanel current;
//
//		private UIItemPool<TopAlertItem> itemPool;
//
//		private float top;
//
//		private const float HEIGHT = 30;
//
//		void Awake()
//		{
//			itemPool = new UIItemPool<TopAlertItem>();
//			itemPool.Init("UI/Items/TopAlertItem", ItemRoot);
//		}
//
//		public void AddItem(string content)
//		{
//			TopAlertItem[] allItems = itemPool.AllItems;
//			for (int i = 0; i < allItems.Length; ++i)
//			{
//				allItems[i].transform.localPosition = allItems[i].transform.localPosition + Vector3.down * HEIGHT;
//			}
//
//			TopAlertItem item = itemPool.AddItem();
//			item.Init(content, OnItemDisappear, 1f);
//		}
//
//		public static void AddAlert(string content)
//		{
//			if (current == null)
//			{
//				current = PopupManager.Instance.CreateAndAddPopup<TopAlertPanel>(PopupMode.SHOW, PopupQueueMode.NoQueue);
//			}
//			current.AddItem(content);
//		}
//
//        private void OnItemDisappear(TopAlertItem item)
//		{
//            itemPool.RemoveItem(item);
//		}
	}
}

