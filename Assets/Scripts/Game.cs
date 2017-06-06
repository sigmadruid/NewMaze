using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using GameLogic;
using Battle;
using GamePlot;

namespace GameLogic
{
	public class Game
	{
        public StageEnum LoadingStageEnum;

        public bool IsNewGame = true;

		public bool IsPause = false;

        public ConfigManager ConfigManager;
        public PlotRunner PlotRunner;
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
            PlotRunner = new PlotRunner();
		}

		public void Init()
		{
			if (hasInit)
				return;

            Application.targetFrameRate = 60;

            framework.Init();
			ConfigManager.InitAllData();

			ApplicationFacade.Instance.Startup();
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.DESERIALIZE_GAME);

            AICore.Init();
            PlotRunner.Init();

			hasInit = true;
		}

        public void Dispose()
        {
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.SERIALIZE_GAME);
        }

		public void Update(float deltaTime)
		{
            InputManager.Instance.Update();
            PlotRunner.Update(deltaTime);

			if (IsPause)  return; 
			TaskManager.Update(deltaTime);
		}

		public void Start(StageEnum stageEnum)
		{
			LoadingStageEnum = stageEnum;
			SwitchStageComplete();
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
            Main main = GameObject.FindObjectOfType<Main>();
            main.StartCoroutine(DoSwitch(stageEnum));
		}
        private IEnumerator DoSwitch(StageEnum stageEnum)
        {
            Loading.Instance.Begin(stageEnum.ToString());
            if (currentStage != null)
            {
                BaseLogger.LogFormat("stage end: {0}", currentStage.Type);
                yield return currentStage.End();
            }
            yield return Loading.Instance.SetProgress(LoadingState.LoadScene, 0);
        }

		public void SwitchStageComplete()
		{
			currentStage = BaseStage.CreateStage(LoadingStageEnum);
            BaseLogger.LogFormat("stage start: {0}", currentStage.Type);
            Main main = GameObject.FindObjectOfType<Main>();
            main.StartCoroutine(currentStage.Start());
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

