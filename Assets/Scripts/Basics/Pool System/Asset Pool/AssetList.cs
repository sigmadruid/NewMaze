using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public class AssetList
	{
		private GameObject assetPrototype;

		private ObjectType type;
		private string path;

		private float maxLife;
		private float assetLife;

		private int maxCacheNum;
		private int growth;

		private List<GameObject> list;

		public AssetList (ObjectType type, string path, float maxLife, int maxCacheNum, int growth)
		{
			this.list = new List<GameObject>();

			this.path = path;
			this.type = type;
			this.maxLife = maxLife;
			this.maxCacheNum = Mathf.Max(maxCacheNum, 1);
			this.growth = Mathf.Max(growth, 1);
		}

		public void Dispose ()
		{
			list.Clear();
		}

		public void Cache (int cacheCount)
		{
			instantiateAsset(cacheCount);
		}

		public GameObject Shift ()
		{
			if (list.Count == 0)
			{
				increaseListSize();
			}
			
			int lastIndex = list.Count - 1;
			GameObject asset = list[lastIndex];
			asset.SetActive(true);
			list.RemoveAt(lastIndex);
			assetLife = maxLife;
			return asset;
		}

		public void Add (GameObject asset)
		{
	//		if (list.Count < maxCacheNum)
			{
				list.Add(asset);
				asset.SetActive(false);
				assetLife = maxLife;
			}
	//		else
	//		{
	//			Debug.Log("too many asset overflow: " + asset.name);
	//		}
		}

		public void Tick (float deltaTime)
		{
			if (maxLife > 0)
			{
				assetLife -= deltaTime;
				if (assetLife <= 0)
				{
					decreaseListSize();
					assetLife = maxLife;
				}
			}
		}

		private void increaseListSize ()
		{
			instantiateAsset(growth);
		}

		private void decreaseListSize ()
		{
			if (list.Count > growth)
			{
				for (int i = 0; i < growth; ++i)
				{
					list.RemoveAt(list.Count - 1);
				}
			}
		}

		private void instantiateAsset (int count)
		{
			for (int i = 0; i < count; ++i)
			{
				if (list.Count < maxCacheNum)
				{
					if (assetPrototype == null)
					{
						assetPrototype = Resources.Load<GameObject>(path);
						if (assetPrototype == null)
						{
							BaseLogger.LogFormat("Null prototype. path: {0}", path);
						}
					}
					GameObject asset = GameObject.Instantiate(assetPrototype) as GameObject;
					
					AssetInfo assetInfo = asset.AddComponent<AssetInfo>();
					assetInfo.assetName = path;
	                assetInfo.type = type;
					assetInfo.name = path;
	                list.Add(asset);
	            }
	        }
		}
	}
}
