using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;

public class BlockScript : EntityScript 
{
	public enum PositionListType
	{
		MonsterPositions,
		NPCPositions,
		ExplorationPositions,
	}

	private Dictionary<PositionListType, PositionScript[]> positionListDic;

	void Awake () 
	{
		positionListDic = new Dictionary<PositionListType, PositionScript[]>();
		InitPositionList(PositionListType.MonsterPositions);
		InitPositionList(PositionListType.NPCPositions);
		InitPositionList(PositionListType.ExplorationPositions);

		InitRandomDecorations();
	}

	private void InitPositionList(PositionListType type)
	{
		Transform posRoot = CachedTransform.FindChild(type.ToString());
		if (posRoot != null)
		{
			PositionScript[] positionArray = posRoot.GetComponentsInChildren<PositionScript>();
            Utils.Shift<PositionScript>(positionArray);
			positionListDic[type] = positionArray;
		}
	}

    public PositionScript GetGlobalPosition(PositionListType type)
    {
        if (type == PositionListType.ExplorationPositions)
        {
            PositionScript[] positionArray = positionListDic[type];
            return positionArray[0];
        }
        else
        {
            return null;
        }
    }
	public PositionScript GetRandomPosition(PositionListType type)
	{
		if (positionListDic.ContainsKey(type))
		{
			PositionScript[] positionArray = positionListDic[type];
            int start = type == PositionListType.ExplorationPositions ? 1 : 0;
            for (int i = start; i < positionArray.Length; ++i)
			{
				PositionScript position = positionArray[i];
				if (position.Available)
				{
					return position;
				}
			}
		}
		return null;
	}

	public void Reset()
	{
		Dictionary<PositionListType, PositionScript[]>.Enumerator enumerator = positionListDic.GetEnumerator();
		while (enumerator.MoveNext())
		{
			PositionScript[] positionList = enumerator.Current.Value;
			for (int i = 0; i < positionList.Length; ++i)
			{
				positionList[i].Available = true;
			}
		}
	}

	public void InitRandomDecorations()
	{
		Transform randomDecorationRoot = CachedTransform.FindChild("RandomDecorations");
		if (randomDecorationRoot != null)
		{
			for (int i = 0; i < randomDecorationRoot.childCount; ++i)
			{
				Transform decorationTrans = randomDecorationRoot.GetChild(i);
				decorationTrans.gameObject.SetActive(RandomUtils.Value() < 0.5f);
			}
		}
	}

}














