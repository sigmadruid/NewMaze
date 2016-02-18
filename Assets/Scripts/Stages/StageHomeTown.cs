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
            PreloadAssets(IDManager.Instance.GetID(IDType.Maze, 0));

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_HERO_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_INIT);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_NPC_SPAWN);

			Game.Instance.Looper.SetActive(TaskEnum.InputUpdate, true);

            InputManager.Instance.SetKeyboardAction(KeyboardActionType.MazeMap, null);
            InputManager.Instance.SetKeyboardAction(KeyboardActionType.Function, null);

		}
		public override void End ()
		{
			Game.Instance.Looper.SetActive(TaskEnum.InputUpdate, false);

            PopupManager.Instance.Clear();

			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.TOWN_HERO_DISPOSE);
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_DISPOSE);

			ResourceManager.Instance.DisposeAssets();
			UnityEngine.Resources.UnloadUnusedAssets();
			GC.Collect();
		}

	}
}

