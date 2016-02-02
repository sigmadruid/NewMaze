using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public class AssetManager
	{
		public float tickInterval = 1f;
		private WaitForSeconds secondDelay;

		private Dictionary<ObjectType, AssetPool> assetPoolDic;

		public AssetManager ()
		{
			secondDelay = new WaitForSeconds(1f);
			assetPoolDic = new Dictionary<ObjectType, AssetPool>();
			assetPoolDic.Add(ObjectType.GameObject, new AssetPool());
		}

		public void Dispose ()
		{
			foreach (AssetPool pool in assetPoolDic.Values)
			{
				pool.Dispose();
			}
			assetPoolDic.Clear();
		}

		public void PreloadAsset (ObjectType type, string path, float life, int maxPreloadCount, int growth)
		{
			AssetPool pool = null;
			assetPoolDic.TryGetValue(type, out pool);
			
			if (pool == null)
			{
				pool = new AssetPool();
			}
			pool.Preload(type, path, life, maxPreloadCount, growth);
			assetPoolDic[type] = pool;
		}

		public GameObject LoadAsset(ObjectType type, string assetName)
		{
			AssetPool pool = null;
			assetPoolDic.TryGetValue(type, out pool);
			
			if (pool != null)
			{
				GameObject asset = pool.Shift(assetName);
				return asset;
			}
			else
			{
				return null;
			}
		}

		public void RecycleAsset (GameObject asset)
		{
			AssetInfo assetInfo = asset.GetComponent<AssetInfo>();

			if (assetInfo != null)
			{
				AssetPool pool = null;
				assetPoolDic.TryGetValue(assetInfo.type, out pool);

				if (pool != null)
				{
					pool.Add(assetInfo);
				}
				else
				{
					Debug.LogError("Asset can't find its pool: " + asset.ToString() + " of type: " + assetInfo.type);
				}
			}
			else
			{
				Debug.LogError("Wrong asset to recycle: " + asset.ToString());
			}
		}

		public IEnumerator Tick()
		{
			while (true)
			{
				yield return secondDelay;

				foreach (AssetPool pool in assetPoolDic.Values)
				{
					pool.Tick(tickInterval);
				}
			}
		}
	}
}
