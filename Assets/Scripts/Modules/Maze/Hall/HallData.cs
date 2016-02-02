using System;

using Base;

namespace StaticData
{
    public class HallData : EntityData
    {
		public string Res3D;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				resPath = Utils.CombinePath("Halls", Res3D);
			}
			return resPath;
		}
    }
}

