using UnityEngine;

using System;
using System.Collections.Generic;

using StaticData;

namespace Base
{
	public class ResourceManager
	{
        public const float RESOURCE_UPDATE_INTERVAL = 1f;

		private AssetManager assetManager;
		private CacheManager cacheManager;
		private ObjectPool objectPool;

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

		public ResourceManager ()
		{
			assetManager = new AssetManager();
			cacheManager = new CacheManager();
			objectPool = new ObjectPool();
		}

		/// <summary>
		/// 载入一个AssetBundle
		/// </summary>
		/// <param name="path">路径</param>
		/// <param name="objectCallback">对获得的Asset的回调</param>
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
		
		/// <summary>
		/// 对指定游戏对象进行预加载
		/// </summary>
		/// <param name="type">类型.</param>
		/// <param name="path">指定的对象路径.</param>
		/// <param name="life">生存时间.</param>
		/// <param name="maxPreloadCount">最大预加载数量.</param>
		/// <param name="growth">对象数量不足时，一次的增长量.</param>
		public void PreloadAsset (ObjectType type, string path, float life = -1f, int maxPreloadCount = 1, int growth = 1)
		{
			assetManager.PreloadAsset(type, path, life, maxPreloadCount, growth);
		}
		
		/// <summary>
		/// 载入游戏对象
		/// </summary>
		/// <param name="type">游戏对象类型.</param>
		/// <param name="assetName">游戏对象名.</param>
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
			GameObject go = assetManager.LoadAsset(type, assetName);
			if (go == null)
			{
				BaseLogger.LogFormat("Cannot load prefab: {0} - {1}", type, assetName);
			}
			return go;
		}
		
		/// <summary>
		/// 回收游戏对象
		/// </summary>
		/// <param name="asset">游戏对象.</param>
		public void RecycleAsset (GameObject asset)
		{
			assetManager.RecycleAsset(asset);
		}

		public void DisposeAssets()
		{
			assetManager.Dispose();
		}
		
		/// <summary>
		/// 预加载缓存对象
		/// </summary>
		/// <param name="cache">缓存对象.</param>
		/// <param name="life">缓存时间.</param>
		public void SaveCache (UnityEngine.Object cache, float remainLife = -1f)
		{
			cacheManager.SaveCache(cache, remainLife);
		}
		
		/// <summary>
		/// 检索缓存对象
		/// </summary>
		/// <param name="type">缓存类型.</param>
		/// <param name="cacheName">缓存名.</param>
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

