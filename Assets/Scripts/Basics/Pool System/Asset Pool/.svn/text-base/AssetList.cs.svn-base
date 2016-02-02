using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetList
{
	private GameObject assetPrototype;

	private ObjectType type;

	private float maxLife;
	private float assetLife;

	private int maxCacheNum;
	private int growth;

	private List<GameObject> list;

	public AssetList (GameObject assetPrototype, ObjectType type, float maxLife, int maxCacheNum, int growth)
	{
		this.list = new List<GameObject>();

		this.assetPrototype = assetPrototype;
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
		list.RemoveAt(lastIndex);
		assetLife = maxLife;
		return asset;
	}

	public void Add (GameObject asset)
	{
		if (list.Count < maxCacheNum)
		{
			list.Add(asset);
			assetLife = maxLife;
		}
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
				GameObject asset = GameObject.Instantiate(assetPrototype) as GameObject;
				
				AssetInfo assetInfo = asset.AddComponent<AssetInfo>();
				assetInfo.assetName = assetPrototype.name;
                assetInfo.type = type;
                list.Add(asset);
            }
        }
	}
}
