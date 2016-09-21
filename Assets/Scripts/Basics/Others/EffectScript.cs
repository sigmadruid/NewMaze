using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;

public class EffectTask
{
	public float Delay;
    public System.Action<object> Callback;
	public object Param;

	public bool HasExecuted;
}

public class EffectScript : MonoBehaviour 
{
	public float SelfDestroyDelay = -1f;

	private ParticleEmitter[] emitters;

	private float timer;
	private List<EffectTask> taskList;

	void Awake()
	{
		taskList = new List<EffectTask>();
		emitters = GetComponentsInChildren<ParticleEmitter>(true);
	}

	void OnDestroy()
	{
		ResetTask();
	}
	
	void Update () 
	{
		if (!IsEnabled)
		{
			return;
		}

		timer += Time.deltaTime;

		for (int i = 0; i < taskList.Count; ++i)
		{
			EffectTask task = taskList[i];
			if (!task.HasExecuted && timer >= task.Delay)
			{
				task.Callback(task.Param);
				task.HasExecuted = true;
			}
		}

		if (SelfDestroyDelay > 0 && timer > SelfDestroyDelay)
		{
			Destroy(gameObject);
		}
	}

	private bool isEnabled;
	public bool IsEnabled
	{
		get
		{
			return isEnabled;
		}
		set
		{
			isEnabled = value;
			for (int i = 0; i < emitters.Length; ++i)
			{
				emitters[i].enabled = isEnabled; 
			}
		}
	}

	public void Active(Vector3 startPosition)
	{
		timer = 0f;
		isEnabled = true;

		transform.position = startPosition;
		gameObject.SetActive(true);
	}
	public void Deactive()
	{
		gameObject.SetActive(false);
	}

    public void AddTask(float delay, System.Action<object> callback, object param = null)
	{
		EffectTask task = new EffectTask();
		task.Delay = delay;
		task.Callback = callback;
		task.Param = param;
		taskList.Add(task);
	}

	public void ResetTask()
	{
		taskList.Clear();
	}

}
