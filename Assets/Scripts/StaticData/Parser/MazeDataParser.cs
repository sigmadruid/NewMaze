using System;
using System.Collections.Generic;

namespace StaticData
{
    public class MazeDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, MazeData> kvDic)
        {
			LoadFile(name);
			
			kvDic = new Dictionary<int, MazeData>();
			
			while(!EndOfRow)
			{
				int col = 0;
				MazeData data = new MazeData();
				
				data.Kid = ReadInt(col++);
				data.StartCol = ReadInt(col++);
				data.StartRow = ReadInt(col++);
				data.BlockSize = ReadFloat(col++);
				data.Cols = ReadInt(col++);
				data.Rows = ReadInt(col++);
				data.LinkRate = ReadFloat(col++);
				data.PassageRate = ReadFloat(col++);
				data.MonsterMaxCount = ReadInt(col++);
				data.NPCMaxCount = ReadInt(col++);
				data.ExplorationMaxCount = ReadInt(col++);
                data.GlobalExplorationCountDic = ReadDictionary<ExplorationType, int>(col, col + 1);
				
				kvDic.Add(data.Kid, data);
				NextLine();
			}
        }
    }
}

