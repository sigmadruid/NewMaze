using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;

public class BlockScript : EntityScript 
{
	public enum PositionType
	{
		MonsterPositions,
		NPCPositions,
		ExplorationPositions,
	}

	private Dictionary<PositionType, PositionScript[]> positionListDic;

	void Awake () 
	{
		positionListDic = new Dictionary<PositionType, PositionScript[]>();
		InitPositionList(PositionType.MonsterPositions);
		InitPositionList(PositionType.NPCPositions);
		InitPositionList(PositionType.ExplorationPositions);

		InitRandomDecorations();
	}

	private void InitPositionList(PositionType type)
	{
		Transform posRoot = CachedTransform.FindChild(type.ToString());
		if (posRoot != null)
		{
			PositionScript[] positionArray = posRoot.GetComponentsInChildren<PositionScript>();
            int start = type == PositionType.ExplorationPositions ? 1 : 0;
            Utils.Shift<PositionScript>(positionArray, start);
			positionListDic[type] = positionArray;
		}
	}

    public PositionScript GetGlobalPosition(PositionType type)
    {
        if (type == PositionType.ExplorationPositions)
        {
            PositionScript[] positionArray = positionListDic[type];
            PositionScript position = positionArray[0];
            position.Available = false;
            return position;
        }
        else
        {
            return null;
        }
    }
	public PositionScript GetRandomPosition(PositionType type)
	{
		if (positionListDic.ContainsKey(type))
		{
			PositionScript[] positionArray = positionListDic[type];
            for (int i = 0; i < positionArray.Length; ++i)
			{
				PositionScript position = positionArray[i];
				if (position.Available)
				{
                    position.Available = false;
					return position;
				}
			}
		}
		return null;
	}

	public void Reset()
	{
		Dictionary<PositionType, PositionScript[]>.Enumerator enumerator = positionListDic.GetEnumerator();
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














