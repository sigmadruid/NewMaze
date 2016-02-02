using UnityEngine;
using System.Collections;

using Base;
using StaticData;
using GameLogic;

public class LightScript : MonoBehaviour
{
	public Utils.CallbackVoid CallbackConvertingEnds;

	private Light mainLight;

	private bool isConverting;
	private float t;
	private Color fromColor;
	private Color toColor;

	public static LightScript Instance;

    void Awake()
    {
		Instance = this;

		mainLight = GetComponent<Light>();
    }

	void Update()
	{
		if (isConverting)
		{
			t += Time.deltaTime / GlobalConfig.EnvironmentConfig.ConvertingDuration;
			if (t >= 1f)
			{
				isConverting = false;
				CallbackConvertingEnds();
			}
			mainLight.color = Color.Lerp(fromColor, toColor, t);
		}
	}

    #region Day Night

	public void DayNightConvert(bool isNight)
	{
		isConverting = true;
		t = 0;

		fromColor  = isNight ? GlobalConfig.EnvironmentConfig.NightLightColor : GlobalConfig.EnvironmentConfig.DayLightColor;
		toColor = isNight ? GlobalConfig.EnvironmentConfig.DayLightColor : GlobalConfig.EnvironmentConfig.NightLightColor;
	}
    
    #endregion
}

