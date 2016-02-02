using UnityEngine;
using System;

public class EntityScript : MonoBehaviour
{
	private Transform cachedTransform;

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null) cachedTransform = transform;
			return cachedTransform;
		}
	}

	public virtual void Pause(bool isPause)
	{
	}
}

