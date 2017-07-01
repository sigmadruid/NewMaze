using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;

namespace StaticData
{
    public enum BattleAttribute
    {
        HP = 1,
        Attack = 2,
        Defense = 3,
        Critical = 4,
        Dodge = 5,
        MoveSpeed = 6,
        AttackSpeed = 7,
    }
    public enum Side
    {
        Hero,
        Monster,
        Neutral,
    }
    public class CameraVibration
    {
        public enum Type
        {
            None = 0,
            Light,
            Normal,
            Heavy,
        }
        public float duration;
        public float scope;
        public CameraVibration(float duration, float scope)
        {

            this.duration = duration;
            this.scope = scope;
        }
    }

    public class HeroConfig
    {
        public readonly float AdamRadius = 0.5f;
        public readonly float MonsterClickSearchDistance = 1f;
        public readonly float TownAdamWalkSpeed = 3f;
    }

    public class MonsterConfig
    {
        public readonly float SmallRadius = 0.5f;
        public readonly float MediumRadius = 0.7f;
        public readonly float BigRadius = 1f;
    }

	public class BattleConfig
	{
		public readonly float OutBattleDelay = 5f;
        public readonly float BuffTransitionDuration = 3f;
	}

    public class BlockConfig
    {
        public readonly int PassagePreloadCount = 10;
		public readonly int RoomPreloadCount = 10;

		public readonly int RefreshScope = 3;

		public readonly string MockPassagePath = "MazeBlocks/Mock/MockPassage";
		public readonly string MockLinkPath = "MazeBlocks/Mock/MockLink";
        public readonly string MockRoomPath = "MazeBlocks/Mock/MockRoom";
        public readonly string IndicatorPath = "MazeBlocks/Indicator/Indicator_3";
		public readonly float MockCubeSize = 3f;
		public readonly float MockLinkSize = 2f;
		public readonly float MockBlockPosY = -10000f;
    }

	public class HallConfig
	{
		public readonly Vector3 HallPosition = new Vector3(10000, 0, 10000);
	}

    public class PathfindingConfig
    {
        public readonly int SnapTimes = 5;
        public readonly string WalkSuffix = "_Walk";
        public readonly string WalkDataSuffix = "_WalkData";
    }

	public class DropConfig
	{
		public readonly float NearSqrDistance = 4f;
	}

    public class ExplorationConfig
    {
        public readonly float NearSqrDistance = 4f;
    }

    public class InputConfig
    {
        public readonly Quaternion DirectionAngleOffset = Quaternion.Euler(Vector3.up * (-45f));
        public readonly float NearSqrDistance = 0.1f;
        public readonly int MouseHitMask = Layers.LayerWalkSurface | Layers.LayerMonster | Layers.LayerItem;
        public readonly int MouseHoverMask = Layers.LayerBlock | Layers.LayerMonster | Layers.LayerNPC | Layers.LayerItem;
        public readonly float DragThreshold = 100f;
    }

	public class CameraConfig
	{
		public readonly WaitForSeconds CheckObstacleDelay = new WaitForSeconds(1f);
	}

	public class EnvironmentConfig
	{
		public readonly float ConvertingDuration = 5f;
		public readonly float NightDuration = 30;
		public readonly float DayDuration = 30f;
		public readonly Color DayLightColor = new Color(165f/255f, 165f/255f, 165f/255f, 1f);
		public readonly Color NightLightColor = new Color(20f/255f, 20f/255f, 20f/255f, 1f);
        public readonly int NightFuryBuffID = 120001;
	}

    public class StaticDataConfig
    {
        public readonly bool CheckType = true;
    }

    public class DemoConfig
    {
        public int InitLevel = 10;
        public List<int> InitialHeroKids = new List<int>(){30000, 30001, 30002, 30003};
    }

    public class GlobalConfig
    {
        public static HeroConfig HeroConfig = new HeroConfig();
        public static MonsterConfig MonsterConfig = new MonsterConfig();
        public static BlockConfig BlockConfig = new BlockConfig();
		public static BattleConfig BattleConfig = new BattleConfig();
        public static DropConfig DropConfig = new DropConfig();
        public static ExplorationConfig ExplorationConfig = new ExplorationConfig();
        public static InputConfig InputConfig = new InputConfig();
		public static CameraConfig CameraConfig = new CameraConfig();
		public static EnvironmentConfig EnvironmentConfig = new EnvironmentConfig();
		public static HallConfig HallConfig = new HallConfig();
        public static PathfindingConfig PathfindingConfig = new PathfindingConfig();
        public static StaticDataConfig StaticDataConfig = new StaticDataConfig();

        public static DemoConfig DemoConfig = new DemoConfig();


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
            managerDic.Add(IDType.Item, ItemDataManager.Instance);
            managerDic.Add(IDType.Weapon, WeaponDataManager.Instance);
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

