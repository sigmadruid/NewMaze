using UnityEngine;

using System;
using System.Collections.Generic;

using StaticData;

namespace Base
{
	public class ResourceManager
	{
        public const float RESOURCE_UPDATE_INTERVAL = 1f;

        private AssetManager assetManager = new AssetManager();
        private CacheManager cacheManager = new CacheManager();
        private ObjectPool objectPool = new ObjectPool();

		private static ResourceManager instance;
		public static ResourceManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ResourceManager();
				}
				return instance;
			}
		}

//		public void LoadAssetBundle(string path, AssetBundleManager.AssetBundleCompleteCallback callback, bool autoDestroy = true)
//		{
//			assetBundleManager.Load(path, callback, autoDestroy);
//		}

		public void Tick()
		{
			assetManager.Tick();
			cacheManager.Tick();
		}

		public T CreateAsset<T>(string path) where T : MonoBehaviour
		{
			GameObject go = CreateGameObject(path);
			T asset = go.GetComponent<T>();
			if (asset == null)
			{
				BaseLogger.LogFormat("Prefab {0} doesn't hava script: {1}", path, typeof(T).ToString());
			}
			return asset;
		}
		public GameObject CreateGameObject(string path)
		{
			GameObject prefab = Resources.Load(path) as GameObject;
			if (prefab == null)
			{
				BaseLogger.LogFormat("Can't create prefab: {0}", path);
			}
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			return go;
		}
		public void DestroyAsset(GameObject go)
		{
			GameObject.Destroy(go);
		}

        public void InitAssets()
        {
            assetManager.Init();
        }
		
		public void PreloadAsset (ObjectType type, string path, int maxPreloadCount = 1, int growth = 1)
		{
			assetManager.PreloadAsset(type, path, maxPreloadCount, growth);
		}
		
		public T LoadAsset<T> (ObjectType type, string assetName) where T : MonoBehaviour
		{
			GameObject go = LoadGameObject(type, assetName);
			T asset = go.GetComponent<T>();
			if (asset == null)
			{
				BaseLogger.LogFormat("Prefab {0} doesn't hava script: {1}", assetName, typeof(T).ToString());
			}
			return asset;
		}

		public GameObject LoadGameObject (ObjectType type, string assetName)
		{
			GameObject go = assetManager.Get(type, assetName);
			if (go == null)
			{
				BaseLogger.LogFormat("Cannot load prefab: {0} - {1}", type, assetName);
			}
			return go;
		}
		
		public void RecycleAsset (GameObject asset)
		{
			assetManager.Recycle(asset);
		}

		public void DisposeAssets()
		{
			assetManager.Dispose();
		}
		
		public void SaveCache (UnityEngine.Object cache, float remainLife = -1f)
		{
			cacheManager.SaveCache(cache, remainLife);
		}
		
		public UnityEngine.Object RetrieveCache (ObjectType type, string cacheName)
		{
			return cacheManager.RetrieveCache(type, cacheName);
		}

		public bool ContainsObjectKey(ObjectKey key)
		{
			return objectPool.ContainsKey(key);
		}

		public void AddObject(ObjectKey key, object obj)
		{
			objectPool.AddObject(key, obj);
		}

		public object RemoveObject(ObjectKey key)
		{
			return objectPool.RemoveObject(key);
		}
	}
}

