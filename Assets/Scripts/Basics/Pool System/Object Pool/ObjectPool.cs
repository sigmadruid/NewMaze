using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public enum ObjectKey
	{
		Block,
		Monster,
		NPC,
		Unknown,
		Bullet,
	}

	public class ObjectPool
	{
		private Dictionary<ObjectKey, LinkList> poolDic;

		public ObjectPool ()
		{
			poolDic = new Dictionary<ObjectKey, LinkList>();
		}

		public bool ContainsKey(ObjectKey key)
		{
			return poolDic.ContainsKey(key);
		}

		public void AddObject(ObjectKey key, object obj)
		{
			if (!poolDic.ContainsKey(key))
			{
				LinkList list = new LinkList();
				list.Add(obj);
				poolDic.Add(key, list);
			}
			else
			{
				LinkList list = poolDic[key];
				list.Add(obj);
			}
		}

		public object RemoveObject(ObjectKey key)
		{
			if (!poolDic.ContainsKey(key))
			{
				return null;
			}
			else
			{
				object obj = poolDic[key].Remove();
				return obj;
			}
		}
	}
}

