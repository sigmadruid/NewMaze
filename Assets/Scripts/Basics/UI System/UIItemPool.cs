using UnityEngine;

using System;
using System.Collections.Generic;

namespace Base
{
	public class UIItemPool<T> where T : MonoBehaviour
	{
		private Transform rootParent;

		private T itemPrefab;

        private bool isDirty = true;

        public void Init(GameObject prefab, Transform rootParent)
        {
            isDirty = true;
            itemPrefab = prefab.GetComponent<T>();
            this.rootParent = rootParent;
        }
		public void Init(string path, Transform rootParent)
		{
			GameObject go = Resources.Load(path) as GameObject;
            Init(go, rootParent);
		}

        private T[] allItems;
        public T[] AllItems
        {
            get
            {
                if (isDirty)
                {
                    allItems = rootParent.GetComponentsInChildren<T>(true);
                    isDirty = false;
                }
                return allItems;
            }
        }

		public T AddItem()
		{
            isDirty = true;

			T item = default(T);
			if(TempParent.childCount > 0)
			{
				item = TempParent.GetChild(0).GetComponent<T>();
				item.gameObject.SetActive(true);
			}
			else
			{
				GameObject go = GameObject.Instantiate(itemPrefab.gameObject) as GameObject;
				item = go.GetComponent<T>();
			}
			item.transform.parent = rootParent;
			item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
			return item;
		}

		public T RemoveItem(T item)
		{
            isDirty = true;

			item.gameObject.SetActive(false);
			item.transform.parent = TempParent;
			return item;
		}

		public void RemoveAll()
		{
            isDirty = true;

			while(rootParent.childCount > 0)
			{
				Transform child = rootParent.GetChild(0);
				child.parent = TempParent;
                child.localPosition = Vector3.one * 99999f;
//				child.gameObject.SetActive(false);
			}
		}

		private static Transform tempParent;
		private static Transform TempParent
		{
			get
			{
				if (tempParent == null)
				{
					GameObject go = new GameObject();
					go.name = "TempUIItemParent";
					tempParent = go.transform;
					tempParent.parent = UICamera.currentCamera.transform;
                    tempParent.localScale = Vector3.one;
				}
				return tempParent;
			}
		}
	}
}

