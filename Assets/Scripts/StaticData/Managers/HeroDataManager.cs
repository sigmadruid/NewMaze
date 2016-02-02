using System;
using System.Collections.Generic;
namespace StaticData
{
	public class HeroDataManager : EntityManager
	{
		private Dictionary<int, HeroData> kvDic;

		private HeroData currentHero;

		private static HeroDataManager instance;
		public static HeroDataManager Instance
		{
			get
			{
				if (instance == null) instance = new HeroDataManager();
				return instance;
			}
		}

		public void Init()
		{
			HeroDataParser parser = new HeroDataParser();
			parser.Parse("HeroDataConfig.csv", out kvDic);
		}

		public HeroData CurrentHeroData
		{
			get
			{
				if (currentHero == null)
				{
					int id = IDManager.Instance.GetID(IDType.Hero, 1);
					currentHero = kvDic[id];
				}
				return currentHero;
			}
		}

		public override EntityData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				UnityEngine.Debug.Log("No such hero kid: " + kid);
			}
			return kvDic[kid];
		}
	}
}

