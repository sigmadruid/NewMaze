using System;

using Base;
using StaticData;

namespace GameLogic
{
    public class RuneProxy : Proxy
    {
        public static RuneType GetRuneType(ItemData itemData)
        {
            if(itemData.Type == ItemType.Rune)
            {
                RuneType type = (RuneType)Enum.Parse(typeof(RuneType), itemData.Param1);
                return type;
            }
            return RuneType.None;
        }
    }
}

