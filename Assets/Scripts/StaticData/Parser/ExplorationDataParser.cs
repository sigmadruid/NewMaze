using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class ExplorationDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, ExplorationData> kvDic, out Dictionary<ExplorationType, List<ExplorationData>> typeDic)
		{
            LoadFile(CONFIG_PATH + name);

			kvDic = new Dictionary<int, ExplorationData>();

			Array values = Enum.GetValues(typeof(ExplorationType));
			typeDic = new Dictionary<ExplorationType, List<ExplorationData>>();
			foreach(var val in values)
			{
				typeDic.Add((ExplorationType)val, new List<ExplorationData>());
			}

            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;
                    ExplorationData data = new ExplorationData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Type = StaticReader.ReadEnum<ExplorationType>(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));
                    data.MazeKid = StaticReader.ReadInt(GetContent(col++));
                    data.IsGlobal = StaticReader.ReadBool(GetContent(col++));
                    data.Param1 = StaticReader.ReadString(GetContent(col++));
                    data.Param2 = StaticReader.ReadString(GetContent(col++));
                    data.Param3 = StaticReader.ReadString(GetContent(col++));
                    kvDic.Add(data.Kid, data);
                    typeDic[data.Type].Add(data);
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

