using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class CurrencyData : EntityData
    {
        public string Name;

        public string Desc;

        public string Res2D;
        
		public List<string> Res3DList;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				string res3D = Res3DList[RandomUtils.Range(0, Res3DList.Count)];
				resPath = Base.Utils.CombinePath("Drop", res3D);
			}
			return resPath;
		}
    }
}

