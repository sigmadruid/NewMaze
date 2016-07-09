﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using GameLogic;

public class Main : MonoBehaviour 
{
	public const float AI_UPDATE_INTERVAL = 0.2f;
	public const float SLOW_UPDATE_INTERVAL = 1f;

	public StageEnum CurrentStageType;

	private Game game;

	void Awake()
	{
		Application.targetFrameRate = 60;

		game = Game.Instance;
		game.Init();

		Test();
	}

	void Start()
	{
		game.Start(CurrentStageType);
	}

	void Update () 
	{
		float deltaTime = Time.deltaTime;
		game.Update(deltaTime);

	}

    void OnApplicationQuit()
    {
        Debug.Log("Game ended");
        game.ApplicationQuit();
    }

	private void Test()
	{
        Debug.Log(Application.persistentDataPath);
	}

}
