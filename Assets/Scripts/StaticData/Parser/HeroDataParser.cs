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
				data.Name = ReadInt(col++);
				data.Res3D = ReadString(col++);
				data.HP = ReadInt(col++);
				data.Attack = ReadInt(col++);
				data.Defense = ReadInt(col++);
				data.Critical = ReadInt(col++);
				data.Dodge = ReadInt(col++);
				data.MoveSpeed = ReadFloat(col++);
				data.DetectRange = ReadFloat(col++);
				data.AttackType = ReadEnum<AttackType>(col++);
				data.AttackDelay = ReadFloat(col++);
				data.AttackRange = ReadFloat(col++);
				data.DodgeDelay = ReadFloat(col++);
				data.DodgeRange = ReadFloat(col++);
				data.BulletKid = ReadInt(col++);
				
				kvDic.Add(data.Kid, data);
				NextLine();
			}
		}
    }
}

