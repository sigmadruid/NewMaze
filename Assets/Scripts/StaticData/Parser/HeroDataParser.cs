using System;
using System.Collections.Generic;

namespace StaticData
{
	public class HeroDataParser : BaseParser
    {
		public void Parse(string name, out Dictionary<int, HeroData> kvDic)
		{
			LoadFile(name);

			kvDic = new Dictionary<int, HeroData>();

			while (!EndOfRow)
			{
				int col = 0;
				HeroData data = new HeroData();

				data.Kid = ReadInt(col++);
                data.Name = ReadString(col++);
                data.Res2D = ReadString(col++);
				data.Res3D = ReadString(col++);
				data.HP = ReadInt(col++);
				data.Attack = ReadInt(col++);
				data.Defense = ReadInt(col++);
				data.Critical = ReadInt(col++);
				data.Dodge = ReadInt(col++);
                data.MoveSpeed = ReadFloat(col++);
                data.AttackSpeed = ReadFloat(col++);
				data.AttackType = ReadEnum<AttackType>(col++);
                data.SkillList = ReadIntList(col++);
                data.Trigger = ReadString(col++);
				
				kvDic.Add(data.Kid, data);
				NextLine();
			}
		}
    }
}

