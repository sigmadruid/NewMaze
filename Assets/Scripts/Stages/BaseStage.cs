using System;

namespace GameLogic
{
	public enum StageEnum
	{
		HomeTown,
		Maze,
	}

	public abstract class BaseStage
	{
		public BaseStage (StageEnum stageEnum) { Type = stageEnum; }

		public StageEnum Type { get; protected set; }

		public abstract void Start();

		public abstract void End();

		public static BaseStage CreateStage(StageEnum stageEnum)
		{
			switch(stageEnum)
			{
				case StageEnum.HomeTown: return new StageHomeTown();
				case StageEnum.Maze: return new StageMaze();
				default: return null;
			}
		}
	}
}

