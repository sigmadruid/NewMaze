using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class MazeDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, MazeData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);
			
			kvDic = new Dictionary<int, MazeData>();
			
            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;
                    MazeData data = new MazeData();

                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.StartCol = StaticReader.ReadInt(GetContent(col++));
                    data.StartRow = StaticReader.ReadInt(GetContent(col++));
                    data.BlockSize = StaticReader.ReadFloat(GetContent(col++));
                    data.Cols = StaticReader.ReadInt(GetContent(col++));
                    data.Rows = StaticReader.ReadInt(GetContent(col++));
                    data.LinkRate = StaticReader.ReadFloat(GetContent(col++));
                    data.PassageRate = StaticReader.ReadFloat(GetContent(col++));
                    data.MonsterMaxCount = StaticReader.ReadInt(GetContent(col++));
                    data.NPCMaxCount = StaticReader.ReadInt(GetContent(col++));
                    data.ExplorationMaxCount = StaticReader.ReadInt(GetContent(col++));
                    data.GlobalExplorationCountDic = StaticReader.ReadDictionary<ExplorationType, int>(GetContent(col++));
                    data.LimitedLevel = StaticReader.ReadInt(GetContent(col++));
                    data.MaxLevel = StaticReader.ReadInt(GetContent(col++));

                    kvDic.Add(data.Kid, data);
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

