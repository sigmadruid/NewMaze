using System;
using System.Collections.Generic;

using Base;

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

            int col = 0;
            try
            {
                while(!EndOfRow)
                {
                    col = 0;

                    BlockData data = new BlockData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.BlockType = StaticReader.ReadEnum<BlockType>(GetContent(col++));
                    data.PassageType = StaticReader.ReadEnum<PassageType>(GetContent(col++));
                    data.Cols = StaticReader.ReadInt(GetContent(col++));
                    data.Rows = StaticReader.ReadInt(GetContent(col++));
                    data.LeftOffset = StaticReader.ReadInt(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));

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
            catch(Exception e)
            {
                col--;
                BaseLogger.LogFormat("WRONG FORMAT IN CONFIG!! str={0},row={1},col={2},file={3}", GetContent(col), RowIndex, col, this.ToString());
            }
		}
    }
}

