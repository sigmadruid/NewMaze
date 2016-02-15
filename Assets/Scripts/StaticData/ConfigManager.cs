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
			Utils.GetStartTime();

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

			Utils.GetEndTime();

			GC.Collect();
		}
    }
}

