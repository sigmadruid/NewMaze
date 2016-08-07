using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using GameLogic;
using Battle;

namespace GameLogic
{
	public class Game
	{
		public StageEnum LoadingStageEnum;

		public bool IsPause = false;

		public InputManager InputManager;
		public ResourceManager ResourceManager;
		public AICore AICore;
		public GameLooper Looper;

		private bool hasInit;
		private BaseStage currentStage;

		private static Game instance;
		public static Game Instance
		{
			get
			{
				if (instance == null) instance = new Game();
				return instance;
			}
		}

		public Game ()
		{
			ResourceManager = ResourceManager.Instance;
			InputManager = InputManager.Instance;
			AICore = new AICore();
		}

		public void Init()
		{
			if (hasInit)
			{
				return;
			}

			ConfigManager.Instance.InitAllData();

			Looper = new GameLooper();
			Looper.AddTask(TaskEnum.AIUpdate, -1f, AICore.Update);
            Looper.AddTask(TaskEnum.AISlowUpdate, AICore.AI_UPDATE_INTERVAL, AICore.SlowUpdate);
            Looper.AddTask(TaskEnum.ResourceUpdate, ResourceManager.RESOURCE_UPDATE_INTERVAL, ResourceManager.Tick);

			ApplicationFacade.Instance.Startup();
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.DESERIALIZE_GAME);

			hasInit = true;
		}

		public void Update(float deltaTime)
		{
            InputManager.Instance.Update();

			if (IsPause)  return; 
			Looper.Update(deltaTime);
		}

		public void Start(StageEnum stageEnum)
		{
			LoadingStageEnum = stageEnum;
			SwitchStageComplete();
		}

        public void ApplicationQuit()
        {
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.SERIALIZE_GAME);
        }

		#region Stage Management

		public StageEnum CurrentStageType
		{
			get { return currentStage.Type; }
		}

		public void SwitchStage(StageEnum stageEnum)
		{
			if (currentStage != null)
			{
				currentStage.End();
			}
			LoadingStageEnum = stageEnum;
            SceneManager.LoadScene("Loading");
		}

		public void SwitchStageComplete()
		{
			currentStage = BaseStage.CreateStage(LoadingStageEnum);
			currentStage.Start();
		}

		public void SetPause(bool state)
		{
            IsPause = state;
            InputManager.Instance.IsPause = IsPause;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_PAUSE, IsPause);
		}

		#endregion
	}
}

