using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using GameLogic;

public class Main : MonoBehaviour 
{
    public int Seed;
	public StageEnum CurrentStageType;

	private Game game;

	void Awake()
	{
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
        game.Dispose();
    }

	private void Test()
	{
        Debug.Log(Application.persistentDataPath);
	}

}
