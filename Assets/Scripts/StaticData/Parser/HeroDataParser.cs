using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class HeroDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, HeroData> kvDic)
		{
            LoadFile(CONFIG_PATH + name);

			kvDic = new Dictionary<int, HeroData>();

            int col = 0;
            try
            {
                while (!EndOfRow)
                {
                    col = 0;
                    HeroData data = new HeroData();

                    data.Kid = StaticReader.ReadInt(GetContent(col++));
                    data.Name = StaticReader.ReadString(GetContent(col++));
                    data.Res2D = StaticReader.ReadString(GetContent(col++));
                    data.Res3D = StaticReader.ReadString(GetContent(col++));
                    data.HP = StaticReader.ReadInt(GetContent(col++));
                    data.Attack = StaticReader.ReadInt(GetContent(col++));
                    data.Defense = StaticReader.ReadInt(GetContent(col++));
                    data.Critical = StaticReader.ReadInt(GetContent(col++));
                    data.Dodge = StaticReader.ReadInt(GetContent(col++));
                    data.MoveSpeed = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackSpeed = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackType = StaticReader.ReadEnum<AttackType>(GetContent(col++));
                    data.SkillList = StaticReader.ReadIntList(GetContent(col++));

                    data.HPRaise = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackRaise = StaticReader.ReadFloat(GetContent(col++));
                    data.DefenseRaise = StaticReader.ReadFloat(GetContent(col++));
                    data.CriticalRaise = StaticReader.ReadFloat(GetContent(col++));
                    data.DodgeRaise = StaticReader.ReadFloat(GetContent(col++));
                    data.MoveSpeedRaise = StaticReader.ReadFloat(GetContent(col++));
                    data.AttackSpeedRaise = StaticReader.ReadFloat(GetContent(col++));

                    data.Trigger = StaticReader.ReadString(GetContent(col++));
                    data.LeftWeapon = StaticReader.ReadInt(GetContent(col++));
                    data.RightWeapon = StaticReader.ReadInt(GetContent(col++));

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

