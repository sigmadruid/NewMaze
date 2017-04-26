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

        private int preloadCount;
		private int growth;

		private List<GameObject> list;

		public AssetList (ObjectType type, string path, int preloadCount, int growth)
		{
			this.list = new List<GameObject>();

			this.path = path;
			this.type = type;
			this.preloadCount = Mathf.Max(preloadCount, 1);
			this.growth = Mathf.Max(growth, 1);

            assetPrototype = Resources.Load<GameObject>(path);
            if (assetPrototype == null)
            {
                BaseLogger.LogFormat("Null prototype. path: {0}", path);
            }
            instantiateAsset(preloadCount);
		}

		public void Dispose ()
		{
            for(int i = 0; i < list.Count; ++i)
            {
                GameObject go = list[i];
                GameObject.Destroy(go);
            }
			list.Clear();
		}

		public GameObject Get ()
		{
			if (list.Count == 0)
			{
                instantiateAsset(growth);
			}
			
			int lastIndex = list.Count - 1;
			GameObject asset = list[lastIndex];
			asset.SetActive(true);
			list.RemoveAt(lastIndex);
			return asset;
		}

		public void Recycle (GameObject asset)
		{
			list.Add(asset);
			asset.SetActive(false);
		}

		public void Tick (float deltaTime)
		{
		}

		private void instantiateAsset (int count)
		{
			for (int i = 0; i < count; ++i)
			{
				GameObject asset = GameObject.Instantiate(assetPrototype) as GameObject;
                asset.transform.SetParent(RootTransform.Instance.PoolRoot);
                asset.SetActive(false);
				
				AssetInfo assetInfo = asset.AddComponent<AssetInfo>();
				assetInfo.path = path;
                assetInfo.type = type;
				assetInfo.name = path;
                list.Add(asset);
	        }
		}
	}
}
