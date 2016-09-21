using System;
using System.Collections.Generic;
using System.Linq;

namespace Base
{
	public enum TaskEnum
	{
		AIUpdate,
		AISlowUpdate,
		ResourceUpdate,
		InputUpdate,
	}

	public class GameTask
	{
		public TaskEnum Type;
		public bool Active;
		public float Delay;
		public float Timer;
		public Utils.CallbackVoid Callback;
	}

	public class GameLooper
	{
		private Dictionary<TaskEnum, GameTask> taskDic;
		private	Dictionary<TaskEnum, GameTask>.Enumerator enumerator;
		
		public GameLooper ()
		{
			taskDic = new Dictionary<TaskEnum, GameTask>();
		}

		public void Update(float deltaTime)
		{
			enumerator = taskDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				GameTask task = enumerator.Current.Value;
				if (task.Active)
				{
					task.Timer += deltaTime;
					if (task.Delay <= 0f)
					{
						task.Callback();
					}
					else if (task.Timer > task.Delay)
					{
						task.Timer = 0f;
						task.Callback();
					}
				}
			}
		}

		public void AddTask(TaskEnum type, float delay, Utils.CallbackVoid callback)
		{
			if (!taskDic.ContainsKey(type))
			{
				GameTask task = new GameTask();
				task.Type = type;
				task.Active = false;
				task.Delay = delay;
				task.Timer = 0f;
				task.Callback = callback;

				taskDic.Add(task.Type, task);
			}
		}

		public void SetAllActive(bool active)
		{
			enumerator = taskDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				GameTask task = enumerator.Current.Value;
				task.Active = active;
			}
		}
		public void SetActive(TaskEnum type, bool active)
		{
			if (taskDic.ContainsKey(type))
			{
				GameTask task = taskDic[type];
				task.Active = active;
			}
		}
	}
}

