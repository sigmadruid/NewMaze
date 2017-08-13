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
            InputManager.Instance.EnableKeyboardAction(KeyboardActionType.MazeMap, true);
            InputManager.Instance.EnableKeyboardAction(KeyboardActionType.Function, true);
            InputManager.Instance.EnableKeyboardAction(KeyboardActionType.Talk, true);
            InputManager.Instance.EnableKeyboardAction(KeyboardActionType.Pause, true);
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
            facade.DispatchNotification(NotificationEnum.BLOCK_INIT);
            if(Adam.Instance.Info.IsInHall)
            {
                facade.DispatchNotification(NotificationEnum.HALL_INIT);
                facade.DispatchNotification(NotificationEnum.HALL_SPAWN);
            }
            else
            {
                facade.DispatchNotification(NotificationEnum.BLOCK_SPAWN);
            }
            facade.DispatchNotification(NotificationEnum.NPC_INIT);
            facade.DispatchNotification(NotificationEnum.BATTLE_UI_INIT);
            Game.Instance.TaskManager.SetAllActive(true);
            yield return Loading.Instance.SetProgress(LoadingState.StartOver, 100);
		}
        public override IEnumerator End ()
		{
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 0);

            //Logic
			Game.Instance.TaskManager.SetAllActive(false);
            TriggerEntityScript.ClearTriggers();

            ApplicationFacade facade = ApplicationFacade.Instance;
            facade.DispatchNotification(NotificationEnum.BLOCK_DISPOSE);
            if(Adam.Instance.Info.IsInHall)
            {
                facade.DispatchNotification(NotificationEnum.HALL_DISPOSE);
                facade.DispatchNotification(NotificationEnum.HALL_DESPAWN);
            }
            else
            {
                facade.DispatchNotification(NotificationEnum.BLOCK_DESPAWN);
            }

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

