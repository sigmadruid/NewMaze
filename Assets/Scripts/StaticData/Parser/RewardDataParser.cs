using System;
using System.Collections.Generic;

namespace StaticData
{
    public class RewardDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, RewardData> kvDic)
        {
			LoadFile(name);
			
			kvDic = new Dictionary<int, RewardData>();
			
			while (!EndOfRow)
			{
				int col = 0;

				RewardData data = new RewardData();
				data.Kid = ReadInt(col++);
				data.GoodsKid = ReadInt(col++);
				data.MinCount = ReadInt(col++);
				data.MaxCount = ReadInt(col++);
				data.Weight = ReadInt(col++);
				data.Possibility = ReadFloat(col++);
				kvDic.Add(data.Kid, data);
				
				NextLine();
			}
        }
    }
}

