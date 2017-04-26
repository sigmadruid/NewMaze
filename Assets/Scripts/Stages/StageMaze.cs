using System;
using System.Collections;
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

        public override IEnumerator Start ()
		{
            DOTween.Init();

			Maze.Instance.Init();

            ApplicationFacade facade = ApplicationFacade.Instance;
            heroProxy = facade.RetrieveProxy<HeroProxy>();
            heroProxy.Init();
            blockProxy = facade.RetrieveProxy<BlockProxy>();
			blockProxy.Init();
            hallProxy = facade.RetrieveProxy<HallProxy>();
            monsterProxy = facade.RetrieveProxy<MonsterProxy>();
            dropProxy = facade.RetrieveProxy<DropProxy>();
            bulletProxy = facade.RetrieveProxy<BulletProxy>();
            npcProxy = facade.RetrieveProxy<NPCProxy>();
            explorationProxy = facade.RetrieveProxy<ExplorationProxy>();
            battleProxy = facade.RetrieveProxy<BattleProxy>();
            battleProxy.Init();

            yield return PreloadAssets(IDManager.Instance.GetKid(IDType.Maze, 1));

            yield return Loading.Instance.SetProgress(LoadingState.StartStage, 0);
            facade.DispatchNotification(NotificationEnum.HERO_INIT, heroProxy.Record);
            if (Adam.Instance.Info.IsInHall)
                facade.DispatchNotification(NotificationEnum.HALL_INIT, hallProxy.Record);
            facade.DispatchNotification(NotificationEnum.BLOCK_INIT);
            facade.DispatchNotification(NotificationEnum.NPC_INIT);
            facade.DispatchNotification(NotificationEnum.BATTLE_UI_INIT);

			Game.Instance.TaskManager.SetAllActive(true);

            InputManager.Instance.Init();
            InputManager.Instance.SetKeyboardAction(KeyboardActionType.MazeMap, () =>
                {
                    ApplicationFacade.Instance.DispatchNotification(NotificationEnum.MAZE_MAP_SHOW);
                });

            yield return Loading.Instance.SetProgress(LoadingState.StartStage, 5);
            if (!Adam.Instance.Info.IsInHall)
                facade.DispatchNotification(NotificationEnum.BLOCK_REFRESH, Adam.Instance.WorldPosition);
            //For test
            facade.RetrieveProxy<PackProxy>().Init();
            yield return Loading.Instance.SetProgress(LoadingState.StartOver, 100);
		}
        public override IEnumerator End ()
		{
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 0);
			Game.Instance.TaskManager.SetAllActive(false);

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
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 10);
			
            InputManager.Instance.Enable = false;
			ResourceManager.Instance.DisposeAssets();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 20);
			UnityEngine.Resources.UnloadUnusedAssets();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 25);
			GC.Collect();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 30);
		}

	}
}

