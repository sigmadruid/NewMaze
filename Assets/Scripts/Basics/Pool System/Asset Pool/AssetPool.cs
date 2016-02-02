using UnityEngine;
using System.Collections.Generic;

namespace Base
{
	public class AssetPool
	{
		private ObjectType type;

		private Dictionary<string, AssetList> assetListDic;

		public AssetPool ()
		{
			assetListDic = new Dictionary<string, AssetList>();
		}

		public void Dispose ()
		{
			foreach (AssetList list in assetListDic.Values)
			{
				list.Dispose();
			}
			assetListDic.Clear();
		}

		public void Preload (ObjectType type, string path, float maxLife, int maxPreloadCount, int growth)
		{
			if (!assetListDic.ContainsKey(path))
			{
				AssetList assetList = new AssetList(type, path, maxLife, maxPreloadCount, growth);
				assetListDic[path] = assetList;
			}
		}

		public GameObject Shift (string assetName)
		{
			AssetList assetList = null;
			assetListDic.TryGetValue(assetName, out assetList);

			if (assetList != null)
			{
				GameObject asset = assetList.Shift();
				return asset;
			}
			else
			{
				return null;
			}
		}

		public void Add (AssetInfo assetInfo)
		{
			AssetList assetList = null;
			assetListDic.TryGetValue(assetInfo.assetName, out assetList);

			if (assetList != null)
			{
				assetList.Add(assetInfo.gameObject);
			}
			else
			{
				Debug.LogError("Asset can't find the list: " + assetInfo.gameObject.ToString());
			}
		}

		public void Tick (float deltaTime)
		{
			foreach (AssetList assetList in assetListDic.Values)
			{
				assetList.Tick(deltaTime);
			}
		}
	}

}