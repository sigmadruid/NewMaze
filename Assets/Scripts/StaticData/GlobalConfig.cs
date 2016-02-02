using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class BattleConfig
	{
		public readonly float OutBattleDelay = 5f;
	}

    public class BlockConfig
    {
        public readonly int PassagePreloadCount = 10;
		public readonly int RoomPreloadCount = 10;

		public readonly int RefreshScope = 10;

		public readonly string MockPassagePath = "MazeBlocks/Mock/MockPassage";
		public readonly string MockLinkPath = "MazeBlocks/Mock/MockLink";
		public readonly string MockRoomPath = "MazeBlocks/Mock/MockRoom";
		public readonly float MockCubeSize = 1.5f;
		public readonly float MockLinkSize = 1f;
		public readonly float MockBlockPosY = -10000f;
    }

	public class HallConfig
	{
		public readonly Vector3 HallPosition = new Vector3(10000, 0, 10000);
	}

	public class DropConfig
	{
		public readonly float NearSqrDistance = 4f;
	}

	public class CameraConfig
	{
		public readonly WaitForSeconds CheckObstacleDelay = new WaitForSeconds(1f);
	}

	public class EnvironmentConfig
	{
		public readonly float ConvertingDuration = 5f;
		public readonly float NightDuration = 60f;
		public readonly float DayDuration = 60f;
		public readonly Color DayLightColor = new Color(165f/255f, 165f/255f, 165f/255f, 1f);
		public readonly Color NightLightColor = new Color(20f/255f, 20f/255f, 20f/255f, 1f);
	}

    public class GlobalConfig
    {
        public static BlockConfig BlockConfig = new BlockConfig();
		public static BattleConfig BattleConfig = new BattleConfig();
		public static DropConfig DropConfig = new DropConfig();
		public static CameraConfig CameraConfig = new CameraConfig();
		public static EnvironmentConfig EnvironmentConfig = new EnvironmentConfig();
		public static HallConfig HallConfig = new HallConfig();

		private Dictionary<IDType, EntityManager> managerDic;

        private static GlobalConfig instance;
        public static GlobalConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalConfig();
                }
                return instance;
            }
        }

		public GlobalConfig()
		{
			managerDic = new Dictionary<IDType, EntityManager>();
			managerDic.Add(IDType.Block, BlockDataManager.Instance);
			managerDic.Add(IDType.Hero, HeroDataManager.Instance);
			managerDic.Add(IDType.Monster, MonsterDataManager.Instance);
			managerDic.Add(IDType.NPC, NPCDataManager.Instance);
			managerDic.Add(IDType.Exploration, ExplorationDataManager.Instance);
			managerDic.Add(IDType.Bullet, BulletDataManager.Instance);
			managerDic.Add(IDType.Currency, CurrencyDataManager.Instance);
			managerDic.Add(IDType.Hall, HallDataManager.Instance);
		}
		public EntityManager GetManager(IDType type)
		{
			if (!managerDic.ContainsKey(type))
			{
				BaseLogger.LogFormat("No such manager: {0}", type);
			}
			return managerDic[type];
		}

    }
}

