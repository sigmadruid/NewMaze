using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;

public class HallScript : EntityScript 
{
    private const string POSITION_ROOT_NAME = "{0}Positions";

	public Transform EntryPos;

    private Dictionary<PositionType, PositionScript[]> positionListDic;

    void Awake () 
    {
        positionListDic = new Dictionary<PositionType, PositionScript[]>();
        InitPositionList(PositionType.Monster);
        InitPositionList(PositionType.NPC);
        InitPositionList(PositionType.Exploration);
    }

    private void InitPositionList(PositionType type)
    {
        Transform posRoot = transform.FindChild(string.Format(POSITION_ROOT_NAME, type));
        if (posRoot != null)
        {
            PositionScript[] positionArray = posRoot.GetComponentsInChildren<PositionScript>();
            positionListDic[type] = positionArray;
        }
    }

    public PositionScript[] GetPositionList(PositionType type)
    {
        if(positionListDic.ContainsKey(type))
        {
            return positionListDic[type];
        }
        return null;
    }
}