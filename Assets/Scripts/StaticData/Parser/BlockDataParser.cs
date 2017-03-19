using System;
using System.Collections.Generic;

namespace StaticData
{
    public class BlockDataParser : BaseParser
    {
		public void Parse(string name, 
		                  out Dictionary<PassageType, List<BlockData>> passageDic, 
		                  out List<BlockData> roomList, 
		                  out Dictionary<int, BlockData> kvDic)
		{
            LoadFile(CONFIG_PATH + name);

			passageDic = new Dictionary<PassageType, List<BlockData>>();
			passageDic.Add(PassageType.One, new List<BlockData>());
			passageDic.Add(PassageType.TwoLine, new List<BlockData>());
			passageDic.Add(PassageType.TwoTurn, new List<BlockData>());
			passageDic.Add(PassageType.Three, new List<BlockData>());
			passageDic.Add(PassageType.Four, new List<BlockData>());

			roomList = new List<BlockData>();
			kvDic = new Dictionary<int, BlockData>();

			while (!EndOfRow)
			{
				int col = 0;

				BlockData data = new BlockData();
				data.Kid = ReadInt(col++);
				data.BlockType = ReadEnum<BlockType>(col++);
				data.PassageType = ReadEnum<PassageType>(col++);
				data.Cols = ReadInt(col++);
				data.Rows = ReadInt(col++);
				data.LeftOffset = ReadInt(col++);
				data.Res3D = ReadString(col++);

				if (data.BlockType == BlockType.Passage)
				{
					passageDic[data.PassageType].Add(data);
				}
				else
				{
					roomList.Add(data);
				}
				kvDic.Add(data.Kid, data);

				NextLine();
			}
		}
    }
}

