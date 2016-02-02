using UnityEngine;
using System;
using System.Collections.Generic;

namespace Base
{
	public class CachePool
	{
		private ObjectType type;
		
		private Dictionary<string, CacheObject> assetDic;

		private Dictionary<int, CacheObject> indexDic;

		private Dictionary<string, CacheObject> recycleDic;

		public CachePool ()
		{
			assetDic = new Dictionary<string, CacheObject>();
			indexDic = new Dictionary<int, CacheObject>();
			recycleDic = new Dictionary<string, CacheObject>();
		}

		public void Dispose ()
		{
			assetDic.Clear();
			indexDic.Clear();
			recycleDic.Clear();
		}

		public void Cache (UnityEngine.Object obj, float life)
		{
			CacheObject cache = null;
			assetDic.TryGetValue(obj.name, out cache);

			if (cache == null)
			{
				cache = new CacheObject(life);
				cache.id = obj.GetInstanceID();
				cache.objName = obj.name;
				cache.obj = obj;
				cache.remainingLife = life;
				cache.referCount = 1;

				assetDic[cache.objName] = cache;
				indexDic[cache.id] = cache;
			}
			else
			{
				cache.referCount++;
			}
		}

		public UnityEngine.Object Retrieve (string objName)
		{
			CacheObject cache = null;
			assetDic.TryGetValue(objName, out cache);

			if (cache != null)
			{
				cache.referCount++;
				return cache.obj;
			}
			else 
			{
				recycleDic.TryGetValue(objName, out cache);

				if (cache != null)
				{
					recycleDic.Remove(objName);
					assetDic[objName] = cache;
					indexDic[cache.id] = cache;

					cache.referCount = 1;
					cache.remainingLife = cache.Life;

					if (cache.obj is GameObject)
					{
						GameObject asset = cache.obj as GameObject;
						asset.SetActive(true);
					}
					return cache.obj;
				}
				else
				{
					return null;
				}
			}

		}

		public void Release (UnityEngine.Object obj, CacheObject.DestroyDelegate destroyCallback)
		{
			int id = obj.GetInstanceID();

			CacheObject cache = null;
			indexDic.TryGetValue(id, out cache);

			if (cache != null)
			{
				cache.referCount--;
				cache.onDestroy = destroyCallback;
				if (cache.Life > 0 && cache.referCount <= 0)
				{
					recycleDic[cache.objName] = cache;
					assetDic.Remove(cache.objName);
					indexDic.Remove(cache.id);
					if (cache.obj is GameObject)
					{
						GameObject gameObject = cache.obj as GameObject;
						gameObject.SetActive(false);
					}
				}
			}
		}

		public void Tick (float deltaTime)
		{
			List<string> nameList = new List<string>();

			foreach (CacheObject cache in recycleDic.Values)
			{
				cache.remainingLife -= deltaTime;
				if (cache.remainingLife <= 0)
					nameList.Add(cache.objName);
			}

			foreach (string name in nameList)
			{
				CacheObject cache = recycleDic[name];
				recycleDic.Remove(name);

				if (cache.onDestroy != null)
					cache.onDestroy.Invoke();

				BaseLogger.LogFormat("Destroy {0}", name);
				UnityEngine.Object.Destroy(cache.obj);
			}
		}

	}
}

