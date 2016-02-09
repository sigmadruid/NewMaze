using System;
using System.Collections.Generic;

using Base;
using StaticData;
using AI;

namespace GameLogic
{
	public class StageMaze : BaseStage
	{
		private BlockProxy blockProxy;
        private HallProxy hallProxy;
		private MonsterProxy monsterProxy;
		private BulletProxy bulletProxy;
		private NPCProxy npcProxy;
		private ExplorationProxy explorationProxy;
		private BattleProxy battleProxy;

        public StageMaze () : base(StageEnum.Maze) {}

		public override void Start ()
		{
			Maze.Instance.Init();

			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
			blockProxy.Init();

            hallProxy = ApplicationFacade.Instance.RetrieveProxy<HallProxy>();
			monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
			bulletProxy = ApplicationFacade.Instance.RetrieveProxy<BulletProxy>();
			npcProxy = ApplicationFacade.Instance.RetrieveProxy<NPCProxy>();
			explorationProxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
			battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();

			PreloadAssets();

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_UI_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_INIT);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.ENVIRONMENT_INIT);

			Game.Instance.Looper.SetAllActive(true);
		}
		public override void End ()
		{
			Game.Instance.Looper.SetAllActive(false);

            PopupManager.Instance.Clear();

			Hero.Recycle();

			monsterProxy.Dispose();
			bulletProxy.Dispose();
			blockProxy.Dispose();
            hallProxy.Dispose();
			npcProxy.Dispose();
			explorationProxy.Dispose();
			battleProxy.Dispose();
			
			ResourceManager.Instance.DisposeAssets();
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		private void PreloadAssets()
		{
//			MazeProxy mazeProxy = ApplicationFacade.Instance.RetrieveProxy<MazeProxy>();
			int mazeKid = IDManager.Instance.GetID(IDType.Maze, 1);
			List<ResourceData> resourceDataList = ResourceDataManager.Instance.GetResourceDataList(mazeKid);

			ResourceManager resManager = ResourceManager.Instance;
			for (int i = 0; i < resourceDataList.Count; ++i)
			{
				ResourceData resourceData = resourceDataList[i];
				IDType idType = IDManager.Instance.GetIDType(resourceData.EntityKid);
				EntityManager manager = GlobalConfig.Instance.GetManager(idType);
				EntityData data = manager.GetData(resourceData.EntityKid);
				resManager.PreloadAsset(ObjectType.GameObject, data.GetResPath(), resourceData.Life, resourceData.PreloadCount);
			}

			//UI
			resManager.PreloadAsset(ObjectType.GameObject, "UI/Items/BarItem", -1, 20);
			resManager.PreloadAsset(ObjectType.GameObject, "UI/Items/NumberItem", -1, 20);
			resManager.PreloadAsset(ObjectType.GameObject, "UI/Items/NPCIcon", -1, 5);
			resManager.PreloadAsset(ObjectType.GameObject, "UI/Items/ExplorationIcon", -1, 5);

			ResourceManager.Instance.PreloadAsset(ObjectType.GameObject, GlobalConfig.BlockConfig.MockPassagePath, -1, 20);
			ResourceManager.Instance.PreloadAsset(ObjectType.GameObject, GlobalConfig.BlockConfig.MockLinkPath, -1, 20);
			ResourceManager.Instance.PreloadAsset(ObjectType.GameObject, GlobalConfig.BlockConfig.MockRoomPath, -1, 10);
		}

	}
}

