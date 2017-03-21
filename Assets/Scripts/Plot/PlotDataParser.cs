using System;
using System.Collections.Generic;

namespace StaticData
{
    public class PlotDataParser : BaseParser
    {
        public void Parse(string name, out List<SegmentData> dataList)
        {
            LoadFile(PLOT_PATH + name);

            dataList = new List<SegmentData>();
            int step = 1;

            while(!EndOfRow)
            {
                int col = 0;
                SegmentData data = new SegmentData();

                data.Step = step++;
                data.StartTime = ReadFloat(col++);
                data.EndTime = ReadFloat(col++);
                data.Type = ReadEnum<SegmentType>(col++);
                data.Target = ReadString(col++);
                data.Position = ReadVector3(col++);
                data.Orientation = ReadVector3(col++);
                data.Param1 = ReadString(col++);
                data.Param2 = ReadString(col++);
                data.Param3 = ReadString(col++);

                dataList.Add(data);
                NextLine();
            }
        }
    }
}

