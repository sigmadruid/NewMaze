using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class SkillDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, SkillData> kvDic)
        {
            LoadFile(CONFIG_PATH + name);

            kvDic = new Dictionary<int, SkillData>();

            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;

                    SkillData data = new SkillData();
                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.CD = StaticReader.ReadFloat(GetContent(col++));
                    data.IsDamage = StaticReader.ReadBool(GetContent(col++));
                    data.NeedTarget = StaticReader.ReadBool(GetContent(col++));
                    data.CanMove = StaticReader.ReadBool(GetContent(col++));
                    data.Range = StaticReader.ReadFloat(GetContent(col++));
                    data.EffectList = StaticReader.ReadIntList(GetContent(col++));
                    data.CostSP = StaticReader.ReadInt(GetContent(col++));

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

