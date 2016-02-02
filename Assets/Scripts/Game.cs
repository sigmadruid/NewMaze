using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using GameLogic;
using AI;

namespace GameLogic
{
	public class Game
	{
		public StageEnum LoadingStageEnum;

		public bool IsPause = false;

		public InputManager InputManager;
		public ResourceManager ResourceManager;
		public TextManager TextManager;
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
			InputManager.Instance.Init();

			Looper = new GameLooper();
			Looper.AddTask(TaskEnum.AIUpdate, 0, AICore.Update);
			Looper.AddTask(TaskEnum.AISlowUpdate, 0.2f, AICore.SlowUpdate);
			Looper.AddTask(TaskEnum.ResourceUpdate, 1f, ResourceManager.Tick);
			Looper.AddTask(TaskEnum.InputUpdate, 0, InputManager.Update);

			ApplicationFacade.Instance.Startup();

			hasInit = true;
		}

		public void Update(float deltaTime)
		{
			if (IsPause) { return; }
			Looper.Update(deltaTime);
		}

		public void Start(StageEnum stageEnum)
		{
			LoadingStageEnum = stageEnum;
			SwitchStageComplete();
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

		public void SetPause()
		{
			IsPause = !IsPause;
		}

		#endregion
	}
}

