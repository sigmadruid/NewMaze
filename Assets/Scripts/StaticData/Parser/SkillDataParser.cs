using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class SkillDataParser : BaseParser
    {
        public void Parse(string name, out Dictionary<int, SkillData> kvDic)
        {
            LoadFile(name);

            kvDic = new Dictionary<int, SkillData>();

            while (!EndOfRow)
            {
                int col = 0;

                SkillData data = new SkillData();
                data.Kid = ReadInt(col++);
                data.CD = ReadFloat(col++);
                data.IsDamage = ReadBool(col++);
                data.Ratio = ReadFloat(col++);
                data.CanMove = ReadBool(col++);
                data.AreaKid = ReadInt(col++);
                data.NeedTarget = ReadBool(col++);
                data.Range = ReadFloat(col++);
                data.BulletKid = ReadInt(col++);

                kvDic.Add(data.Kid, data);

                NextLine();
            }
        }
    }
}

