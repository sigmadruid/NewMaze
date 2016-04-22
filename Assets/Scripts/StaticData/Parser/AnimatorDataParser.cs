using UnityEngine;

using System;
using System.Collections.Generic;

namespace StaticData
{
    public class AnimatorDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, Dictionary<int, AnimatorData>> kvDic)
		{
			LoadFile(name);

			kvDic = new Dictionary<int, Dictionary<int, AnimatorData>>();
			while(!EndOfRow)
			{
				int col = 0;
				AnimatorData data = new AnimatorData();
				data.Kid = ReadInt(col++);
				data.Name = ReadString(col++);
				data.NameHash = Animator.StringToHash(data.Name);
				data.IsLoop = ReadBool(col++);
				data.Priority = ReadEnum<AnimatorPriorityEnum>(col++);
				data.NormalTime = ReadFloat(col++);
				data.ParamDic = ReadDictionary<AnimatorParamKey, int>(col++);

				if (!kvDic.ContainsKey(data.Kid))
				{
					kvDic.Add(data.Kid, new Dictionary<int, AnimatorData>());
				}
				kvDic[data.Kid].Add(data.NameHash, data);

				NextLine();
			}
		}
    }
}

