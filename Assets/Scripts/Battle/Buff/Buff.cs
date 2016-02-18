using System;
using System.Collections.Generic;

namespace Battle
{
    public enum BuffType
    {
        Attribute,
        Gradual,
        Special,
    }
    public enum BuffSpecialType
    {
        None,
        Invisible,
    }
	public class Buff
	{
        public BuffType Type;

        public BuffSpecialType SpecialType;

		private Dictionary<BattleAttribute, int> attributeDic;

		public Buff ()
		{
			attributeDic = new Dictionary<BattleAttribute, int>();
		}
	}
}

