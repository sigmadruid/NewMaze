using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public class AssetManager
	{
        public const float TICK_INTERVAL = 1f;
        private readonly WaitForSeconds TICK_SECONDS = new WaitForSeconds(TICK_INTERVAL);

        private Dictionary<ObjectType, AssetPool> assetPoolDic = new Dictionary<ObjectType, AssetPool>();

        public void Init ()
		{
            AssetPool goPool = new AssetPool();
            goPool.Init(ObjectType.GameObject);
            assetPoolDic.Add(ObjectType.GameObject, goPool);
		}

		public void Dispose ()
		{
            var enumerator = assetPoolDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                enumerator.Current.Value.Dispose();
            }
			assetPoolDic.Clear();
		}

		public void PreloadAsset (ObjectType type, string path, int preloadCount, int growth)
		{
            if(assetPoolDic.ContainsKey(type))
            {
                AssetPool pool = assetPoolDic[type];
                pool.Preload(path, preloadCount, growth);
            }
            else
            {
                BaseLogger.LogFormat("Can't find asset pool:{0}", type);
            }
		}

		public GameObject Get(ObjectType type, string assetName)
		{
            if(assetPoolDic.ContainsKey(type))
			{
                AssetPool pool = assetPoolDic[type];
				GameObject asset = pool.Get(assetName);
				return asset;
			}
			else
			{
                BaseLogger.LogFormat("Can't find asset pool:{0}", type);
				return null;
			}
		}

		public void Recycle (GameObject asset)
		{
			AssetInfo assetInfo = asset.GetComponent<AssetInfo>();

			if (assetInfo != null)
			{
                if(assetPoolDic.ContainsKey(assetInfo.type))
                {
                    AssetPool pool = assetPoolDic[assetInfo.type];
                    pool.Recycle(assetInfo);
                }
                else
				{
                    BaseLogger.LogFormat("Asset:{0} can't find its pool:{1} of type: ", assetInfo.path, assetInfo.type);
				}
			}
			else
			{
                BaseLogger.LogFormat("Wrong asset to recycle: {0}", asset);
			}
		}

		public IEnumerator Tick()
		{
			while (true)
			{
                yield return TICK_SECONDS;

                var enumerator = assetPoolDic.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    enumerator.Current.Value.Tick(TICK_INTERVAL);
                }
			}
		}
	}
}
