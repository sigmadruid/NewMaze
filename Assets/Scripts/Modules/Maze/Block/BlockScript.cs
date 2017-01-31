using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;

public class BlockScript : EntityScript 
{
    private const string POSITION_ROOT_NAME = "{0}Positions";

	private Dictionary<PositionType, PositionScript[]> positionListDic;

	void Awake () 
	{
		positionListDic = new Dictionary<PositionType, PositionScript[]>();
		InitPositionList(PositionType.Monster);
		InitPositionList(PositionType.NPC);
		InitPositionList(PositionType.Exploration);

		InitRandomDecorations();
	}

	private void InitPositionList(PositionType type)
	{
        Transform posRoot = transform.FindChild(string.Format(POSITION_ROOT_NAME, type));
		if (posRoot != null)
		{
			PositionScript[] positionArray = posRoot.GetComponentsInChildren<PositionScript>();
            //The first pos of exploration is reserved for global exploration
            int start = type == PositionType.Exploration ? 1 : 0;
            Utils.Shift<PositionScript>(positionArray, start);
			positionListDic[type] = positionArray;
		}
	}

    public PositionScript GetGlobalPosition(PositionType type)
    {
        if (type == PositionType.Exploration)
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
		Transform randomDecorationRoot = transform.FindChild("RandomDecorations");
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

