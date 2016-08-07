using System;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
	public class StageMaze : BaseStage
	{
        private HeroProxy heroProxy;
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

            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
            heroProxy.Init();
			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
			blockProxy.Init();
            hallProxy = ApplicationFacade.Instance.RetrieveProxy<HallProxy>();
			monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
			bulletProxy = ApplicationFacade.Instance.RetrieveProxy<BulletProxy>();
			npcProxy = ApplicationFacade.Instance.RetrieveProxy<NPCProxy>();
			explorationProxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
			battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();

            PreloadAssets(IDManager.Instance.GetID(IDType.Maze, 1));

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_UI_INIT);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_INIT, heroProxy.Record);
            if (Hero.Instance.IsInHall)
                ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_INIT, hallProxy.Record);
            else
                ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.ENVIRONMENT_INIT);

			Game.Instance.Looper.SetAllActive(true);

            InputManager.Instance.Init();
            InputManager.Instance.SetKeyboardAction(KeyboardActionType.MazeMap, () =>
                {
                    ApplicationFacade.Instance.DispatchNotification(NotificationEnum.MAZE_MAP_SHOW);
                });
		}
		public override void End ()
		{
			Game.Instance.Looper.SetAllActive(false);

            PopupManager.Instance.Clear();

			Hero.Recycle();

            heroProxy.Dispose();
            blockProxy.Dispose();
            hallProxy.Dispose();
            monsterProxy.Dispose();
            bulletProxy.Dispose();
			npcProxy.Dispose();
			explorationProxy.Dispose();
			battleProxy.Dispose();
			
            InputManager.Instance.Enable = false;
			ResourceManager.Instance.DisposeAssets();
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		private void PreloadAssets()
		{
			int mazeKid = IDManager.Instance.GetID(IDType.Maze, 1);
			List<ResourceData> resourceDataList = ResourceDataManager.Instance.GetResourceDataList(mazeKid);

			ResourceManager resManager = ResourceManager.Instance;
			for (int i = 0; i < resourceDataList.Count; ++i)
			{
				ResourceData resourceData = resourceDataList[i];
                if(resourceData.EntityKid != 0)
                {
                    IDType idType = IDManager.Instance.GetIDType(resourceData.EntityKid);
                    EntityManager manager = GlobalConfig.Instance.GetManager(idType);
                    EntityData data = manager.GetData(resourceData.EntityKid);
                    resManager.PreloadAsset(ObjectType.GameObject, data.GetResPath(), resourceData.Life, resourceData.PreloadCount);
                }
                else
                {
                    resManager.PreloadAsset(ObjectType.GameObject, resourceData.Path, resourceData.Life, resourceData.PreloadCount);
                }
			}

		}

	}
}

