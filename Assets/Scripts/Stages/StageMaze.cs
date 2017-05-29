using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using Battle;

namespace GameLogic
{
	public class StageMaze : BaseStage
	{
        public StageMaze () : base(StageEnum.Maze) {}

        public override IEnumerator Start ()
		{
            //Resources
            yield return PreloadAssets(IDManager.Instance.GetKid(IDType.Maze, 1));
            yield return Loading.Instance.SetProgress(LoadingState.StartStage, 0);

            //Framework
            InputManager.Instance.Init();
            InputManager.Instance.SetKeyboardAction(KeyboardActionType.MazeMap, () =>
                {
                    ApplicationFacade.Instance.DispatchNotification(NotificationEnum.MAZE_MAP_SHOW);
                });
            yield return Loading.Instance.SetProgress(LoadingState.StartStage, 10);

            //Logic
            Maze.Instance.Init();
            ApplicationFacade facade = ApplicationFacade.Instance;
            facade.RetrieveProxy<BlockProxy>().Init();
            facade.RetrieveProxy<HallProxy>().Init();
            facade.RetrieveProxy<MonsterProxy>().Init();
            facade.RetrieveProxy<DropProxy>().Init();
            facade.RetrieveProxy<BulletProxy>().Init();
            facade.RetrieveProxy<NPCProxy>().Init();
            facade.RetrieveProxy<ExplorationProxy>().Init();
            facade.RetrieveProxy<BattleProxy>().Init();
            facade.DispatchNotification(NotificationEnum.HERO_INIT);
            if (Adam.Instance.Info.IsInHall)
                facade.DispatchNotification(NotificationEnum.HALL_INIT,  facade.RetrieveProxy<HallProxy>().Record);
            facade.DispatchNotification(NotificationEnum.BLOCK_INIT);
            facade.DispatchNotification(NotificationEnum.NPC_INIT);
            facade.DispatchNotification(NotificationEnum.BATTLE_UI_INIT);
            if (!Adam.Instance.Info.IsInHall)
                facade.DispatchNotification(NotificationEnum.BLOCK_REFRESH, Adam.Instance.WorldPosition);
            Game.Instance.TaskManager.SetAllActive(true);
            yield return Loading.Instance.SetProgress(LoadingState.StartOver, 100);
		}
        public override IEnumerator End ()
		{
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 0);

            //Logic
			Game.Instance.TaskManager.SetAllActive(false);
            ApplicationFacade facade = ApplicationFacade.Instance;
            facade.RetrieveProxy<BlockProxy>().Dispose();
            facade.RetrieveProxy<HallProxy>().Dispose();
            facade.RetrieveProxy<MonsterProxy>().Dispose();
            facade.RetrieveProxy<DropProxy>().Dispose();
            facade.RetrieveProxy<BulletProxy>().Dispose();
            facade.RetrieveProxy<NPCProxy>().Dispose();
            facade.RetrieveProxy<ExplorationProxy>().Dispose();
            facade.RetrieveProxy<BattleProxy>().Dispose();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 10);
			
            //Framework
            PopupManager.Instance.Clear();
            InputManager.Instance.Enable = false;
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 20);

            //Resources
			ResourceManager.Instance.DisposeAssets();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 20);

            //GC
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 40);
		}

	}
}

