using UnityEngine;
using System.Collections.Generic;

namespace Base
{
	public class AssetPool
	{
        public ObjectType Type { get; private set; }

        private Dictionary<string, AssetList> assetListDic = new Dictionary<string, AssetList>();

        public void Init(ObjectType type)
        {
            Type = type;
        }

        public void Dispose ()
		{
            var enumerator = assetListDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                enumerator.Current.Value.Dispose();
            }
			assetListDic.Clear();
		}

		public void Preload (string path, int preloadCount, int growth)
		{
			if (!assetListDic.ContainsKey(path))
			{
                AssetList assetList = new AssetList(Type, path, preloadCount, growth);
				assetListDic[path] = assetList;
			}
            else
            {
                BaseLogger.LogFormat("Already got assetlist:{0}, in pool:{1}", path, Type);
            }
		}

		public GameObject Get (string path)
		{
            if(assetListDic.ContainsKey(path))
            {
                AssetList assetList = assetListDic[path];
                GameObject asset = assetList.Get();
                return asset;
            }
            else
            {
                BaseLogger.LogFormat("Can't find assetlist:{0}, in pool:{1}", path, Type);
                return null;
            }
		}

		public void Recycle (AssetInfo assetInfo)
		{
            if(assetListDic.ContainsKey(assetInfo.path))
            {
                AssetList assetList = assetListDic[assetInfo.path];
                assetList.Recycle(assetInfo.gameObject);
            }
            else
            {
                BaseLogger.LogFormat("Can't find assetlist:{0}, in pool:{1}", assetInfo.path, Type);
            }
		}

		public void Tick (float deltaTime)
		{
//            var enumerator = assetListDic.GetEnumerator();
//            while(enumerator.MoveNext())
//            {
//                enumerator.Current.Value.Tick(deltaTime);
//            }
		}
	}

}