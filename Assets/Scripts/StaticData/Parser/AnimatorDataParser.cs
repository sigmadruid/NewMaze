using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class AnimatorDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, Dictionary<int, AnimatorData>> kvDic)
		{
            LoadFile(CONFIG_PATH + name);

			kvDic = new Dictionary<int, Dictionary<int, AnimatorData>>();
            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    AnimatorData data = new AnimatorData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.NameHash = Animator.StringToHash(data.Name);
                    data.IsLoop = StaticReader.ReadBool(GetContent(col++));
                    data.Priority = StaticReader.ReadEnum<AnimatorPriorityEnum>(GetContent(col++));
                    data.NormalTime = StaticReader.ReadFloat(GetContent(col++));
                    data.ParamDic = StaticReader.ReadDictionary<AnimatorParamKey, string>(GetContent(col++));

                    if (!kvDic.ContainsKey(data.Kid))
                    {
                        kvDic.Add(data.Kid, new Dictionary<int, AnimatorData>());
                    }
                    kvDic[data.Kid].Add(data.NameHash, data);

                    NextLine();
                }
            }
            catch(Exception e)
            {
                col--;
                BaseLogger.LogFormat("WRONG FORMAT IN CONFIG!! str={0},row={1},col={2},file={3}", GetContent(col), RowIndex, col, this.ToString());
            }
		}
    }
}

