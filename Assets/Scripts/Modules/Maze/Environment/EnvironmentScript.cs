using UnityEngine;

using System;
using System.Collections;

using Base;

public class EnvironmentScript : MonoBehaviour
{
	public bool IsNight;

    public Action CallbackUpdate;

	public LightScript MainLightScript;
    public Light MazeMapLight;

    public GuideEffect GuideEffect;

	void Update()
	{
        if (CallbackUpdate != null)
		    CallbackUpdate();
	}
}

