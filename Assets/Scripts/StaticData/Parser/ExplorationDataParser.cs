using System;
using System.Collections.Generic;
namespace StaticData
{
    public class ExplorationDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, ExplorationData> kvDic, out Dictionary<ExplorationType, List<ExplorationData>> typeDic)
		{
			LoadFile(name);

			kvDic = new Dictionary<int, ExplorationData>();

			Array values = Enum.GetValues(typeof(ExplorationType));
			typeDic = new Dictionary<ExplorationType, List<ExplorationData>>();
			foreach(var val in values)
			{
				typeDic.Add((ExplorationType)val, new List<ExplorationData>());
			}

			while (!EndOfRow)
			{
				int col = 0;
				ExplorationData data = new ExplorationData();
				data.Kid = ReadInt(col++);
				data.Type = ReadEnum<ExplorationType>(col++);
				data.Res3D = ReadString(col++);
				data.MazeKid = ReadInt(col++);
                data.IsGlobal = ReadBool(col++);
                data.Param1 = ReadString(col++);
                data.Param2 = ReadString(col++);
                data.Param3 = ReadString(col++);
				kvDic.Add(data.Kid, data);
				typeDic[data.Type].Add(data);
				NextLine();
			}
		}

    }
}

