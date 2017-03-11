using System;

using Base;

namespace StaticData
{
    public class ConfigManager
    {
		private static ConfigManager instance;
		public static ConfigManager Instance
		{
			get
			{
				if (instance == null) instance = new ConfigManager();
				return instance;
			}
		}

		public void InitAllData()
		{
			Utils.GetStartTime("Static Data deserializing");

            KeyboardDataManager.Instance.Init();
			AnimatorDataManager.Instance.Init();
			HeroDataManager.Instance.Init();
			ExplorationDataManager.Instance.Init();
			BlockDataManager.Instance.Init();
			ResourceDataManager.Instance.Init();
			NPCDataManager.Instance.Init();
			BulletDataManager.Instance.Init();
			MonsterDataManager.Instance.Init();
			MazeDataManager.Instance.Init();
			DropDataManager.Instance.Init();
			CurrencyDataManager.Instance.Init();
			TextDataManager.Instance.Init();
			HallDataManager.Instance.Init();
            BuffDataManager.Instance.Init();
            TrapDataManager.Instance.Init();
            ItemDataManager.Instance.Init();
            SkillDataManager.Instance.Init();
            AreaDataManager.Instance.Init();
            WeaponDataManager.Instance.Init();

            Utils.GetEndTime("Static Data deserializing");

			GC.Collect();
		}
    }
}

