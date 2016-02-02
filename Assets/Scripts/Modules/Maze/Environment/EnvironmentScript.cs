using UnityEngine;

using System;
using System.Collections;

using Base;

public class EnvironmentScript : MonoBehaviour
{
	public bool IsNight;

	public Utils.CallbackVoid CallbackUpdate;

	public LightScript LightScript;

	void Awake()
	{
	}

	void Update()
	{
		CallbackUpdate();
	}
}

