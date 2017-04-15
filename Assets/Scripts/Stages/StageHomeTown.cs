using System;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class StageHomeTown : BaseStage
	{
        public StageHomeTown () : base(StageEnum.HomeTown) {}

		public override void Start ()
		{
            ApplicationFacade facade = ApplicationFacade.Instance;
            if(facade.RetrieveMediator<TownHeroMediator>() == null)
                facade.RegisterMediator(new TownHeroMediator());

            PreloadAssets(IDManager.Instance.GetKid(IDType.Maze, 0));
            Loading.Instance.SetProgress(LoadingState.StartStage, 10);

            facade.DispatchNotification(NotificationEnum.PATHFINDING_INIT, PathfindingType.HomeTown);
			facade.DispatchNotification(NotificationEnum.TOWN_HERO_INIT);
			facade.DispatchNotification(NotificationEnum.NPC_INIT);
			facade.DispatchNotification(NotificationEnum.TOWN_NPC_SPAWN);
            facade.DispatchNotification(NotificationEnum.ENVIRONMENT_INIT);

			Game.Instance.TaskManager.SetActive(TaskEnum.INPUT_UPDATE, true);

            InputManager.Instance.Init();
            InputManager.Instance.SetKeyboardAction(KeyboardActionType.MazeMap, null);
            InputManager.Instance.SetKeyboardAction(KeyboardActionType.Function, null);

            Loading.Instance.SetProgress(LoadingState.StartStage, 100);
		}
		public override void End ()
		{
            Loading.Instance.SetProgress(LoadingState.EndStage, 0);

			Game.Instance.TaskManager.SetActive(TaskEnum.INPUT_UPDATE, false);

            PopupManager.Instance.Clear();

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_HERO_DISPOSE);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_DISPOSE);

            InputManager.Instance.Enable = false;
			ResourceManager.Instance.DisposeAssets();
            Loading.Instance.SetProgress(LoadingState.EndStage, 20);
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
            Loading.Instance.SetProgress(LoadingState.EndStage, 30);
		}

	}
}

