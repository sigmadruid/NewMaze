using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class StageHomeTown : BaseStage
	{
        public StageHomeTown () : base(StageEnum.HomeTown) {}

        public override IEnumerator Start ()
		{
            ApplicationFacade facade = ApplicationFacade.Instance;

            //Resources
            yield return PreloadAssets(IDManager.Instance.GetKid(IDType.Maze, 0));

            //Framework
            InputManager.Instance.Init();
            InputManager.Instance.EnableKeyboardAction(KeyboardActionType.MazeMap, false);
            InputManager.Instance.EnableKeyboardAction(KeyboardActionType.Function, true);
            InputManager.Instance.EnableKeyboardAction(KeyboardActionType.Pause, true);
            //Logic
            facade.RetrieveProxy<HeroProxy>().Init();
            facade.RetrieveProxy<PackProxy>().Init();
            facade.DispatchNotification(NotificationEnum.PATHFINDING_INIT, PathfindingType.HomeTown);
			facade.DispatchNotification(NotificationEnum.TOWN_HERO_INIT);
			facade.DispatchNotification(NotificationEnum.NPC_INIT);
			facade.DispatchNotification(NotificationEnum.TOWN_NPC_SPAWN);
            facade.DispatchNotification(NotificationEnum.ENVIRONMENT_INIT);
            Game.Instance.TaskManager.SetActive(TaskEnum.INPUT_UPDATE, true);

            yield return Loading.Instance.SetProgress(LoadingState.StartOver, 100);
		}
        public override IEnumerator End ()
		{
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 0);

            //Logic
            Game.Instance.TaskManager.SetActive(TaskEnum.INPUT_UPDATE, false);
            TriggerEntityScript.ClearTriggers();
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_HERO_DISPOSE);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_DISPOSE);
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 10);

            //Framework
            PopupManager.Instance.Clear();
            InputManager.Instance.Enable = false;
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 20);

            //Resources
			ResourceManager.Instance.DisposeAssets();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 30);

            //GC
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
            yield return Loading.Instance.SetProgress(LoadingState.EndStage, 40);
		}

	}
}

