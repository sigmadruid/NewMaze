using System;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

using DG.Tweening;

namespace GameLogic
{
	public class StageMaze : BaseStage
	{
        private HeroProxy heroProxy;
		private BlockProxy blockProxy;
        private HallProxy hallProxy;
        private MonsterProxy monsterProxy;
        private DropProxy dropProxy;
		private BulletProxy bulletProxy;
		private NPCProxy npcProxy;
		private ExplorationProxy explorationProxy;
		private BattleProxy battleProxy;

        public StageMaze () : base(StageEnum.Maze) {}

		public override void Start ()
		{
            DOTween.Init();

			Maze.Instance.Init();

            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
            heroProxy.Init();
			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
			blockProxy.Init();
            hallProxy = ApplicationFacade.Instance.RetrieveProxy<HallProxy>();
            monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
            dropProxy = ApplicationFacade.Instance.RetrieveProxy<DropProxy>();
			bulletProxy = ApplicationFacade.Instance.RetrieveProxy<BulletProxy>();
			npcProxy = ApplicationFacade.Instance.RetrieveProxy<NPCProxy>();
			explorationProxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
			battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();

            PreloadAssets(IDManager.Instance.GetKid(IDType.Maze, 1));

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_UI_INIT);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_INIT, heroProxy.Record);
            if (Hero.Instance.Info.IsInHall)
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

            //For test
            ApplicationFacade.Instance.RetrieveProxy<PackProxy>().Init();
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
            dropProxy.Dispose();
            bulletProxy.Dispose();
			npcProxy.Dispose();
			explorationProxy.Dispose();
			battleProxy.Dispose();
			
            InputManager.Instance.Enable = false;
			ResourceManager.Instance.DisposeAssets();
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
		}

	}
}

