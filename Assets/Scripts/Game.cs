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

        public ConfigManager ConfigManager;
        public AICore AICore;

        private BaseStage currentStage;
        private bool hasInit;

        private Framework framework;

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
            framework = Framework.Instance;
            ConfigManager = ConfigManager.Instance;
			AICore = new AICore();
		}

		public void Init()
		{
			if (hasInit)
			{
				return;
			}

            framework.Init();

			ConfigManager.InitAllData();

            framework.TaskManager.AddTask(TaskEnum.AI_UPDATE, -1f, -1, AICore.Update);
            framework.TaskManager.AddTask(TaskEnum.AI_HEART_BEAT, AICore.AI_UPDATE_INTERVAL, -1, AICore.SlowUpdate);

			ApplicationFacade.Instance.Startup();
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.DESERIALIZE_GAME);

			hasInit = true;
		}

		public void Update(float deltaTime)
		{
            InputManager.Instance.Update();

			if (IsPause)  return; 
			TaskManager.Update(deltaTime);
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

        #region Managers

        public ResourceManager ResourceManager
        {
            get { return framework.ResourceManager; }
        }

        public InputManager InputManager
        {
            get { return framework.InputManager; }
        }

        public PopupManager PopupManager
        {
            get { return framework.PopupManager; }
        }

        public TaskManager TaskManager
        {
            get { return framework.TaskManager; }
        }

        #endregion

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
            SceneManager.LoadSceneAsync("Loading");
		}

		public void SwitchStageComplete()
		{
			currentStage = BaseStage.CreateStage(LoadingStageEnum);
			currentStage.Start();
		}

        #endregion

        public void SetPause(bool state)
        {
            IsPause = state;
            InputManager.Instance.IsPause = IsPause;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_PAUSE, IsPause);
        }
	}
}

