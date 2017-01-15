using System;
using System.Collections.Generic;

namespace Base
{
	public enum TaskEnum
	{
		AI_UPDATE,
		AI_HEART_BEAT,
		RESOURCE_UPDATE,
		INPUT_UPDATE,
        UI_ANIMATION,
	}

	public class GameTask
	{
		public TaskEnum Type;
		public bool Active;
		public float Delay;
        public int Loops;
		public float Timer;
        public Action Callback;
	}

	public class TaskManager
	{
        private Dictionary<TaskEnum, GameTask> taskDic = new Dictionary<TaskEnum, GameTask>();
        private List<TaskEnum> toRemoveList = new List<TaskEnum>();
		
        public TaskManager () 
        {
        }

		public void Update(float deltaTime)
		{
			var enumerator = taskDic.GetEnumerator();
			while(enumerator.MoveNext())
			{
				GameTask task = enumerator.Current.Value;
				if (task.Active)
				{
                    if(task.Loops == 0)
                        continue;

					task.Timer += deltaTime;
					if (task.Delay <= 0f)
					{
						task.Callback();
                        if(task.Loops > 0)
                            task.Loops--;
					}
					else if (task.Timer > task.Delay)
					{
						task.Timer = 0f;
						task.Callback();
                        if(task.Loops > 0)
                            task.Loops--;
					}
				}
			}
            if(toRemoveList.Count > 0)
            {
                for(int i = 0; i < toRemoveList.Count; ++i)
                {
                    taskDic.Remove(toRemoveList[i]);
                }
                toRemoveList.Clear();
            }
		}

        public void AddTask(TaskEnum type, float delay, int loops, Action callback)
		{
			if (!taskDic.ContainsKey(type))
			{
				GameTask task = new GameTask();
				task.Type = type;
				task.Active = false;
				task.Delay = delay;
                task.Loops = loops;
				task.Timer = 0f;
				task.Callback = callback;

				taskDic.Add(task.Type, task);
			}
		}

        public void RemoveTask(TaskEnum type)
        {
            toRemoveList.Add(type);
        }

		public void SetAllActive(bool active)
		{
			var enumerator = taskDic.GetEnumerator();
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

