using UnityEngine;
using System;

public class BezierUtil
{
	public static float straightThreshold = 3f;

	public static float maxHeight = 10f;

	public static float curveRate = 0.3f;

	public static Vector3 LerpPosition(Vector3 from, Vector3 to, float t)
	{
		Vector3 position = Vector3.zero;
		
		if (t >= 0 && t <= 1f)
		{
			position = (1 - t) * from + t * to;
		}
		
		return position;
	}

	public static Vector3 LerpPosition(Vector3 from, Vector3 middle, Vector3 to, float t)
	{
        t = Mathf.Clamp01(t);
        //if(t >= 0 && t <= 1f)
        //{
        //  position = (1 - t) * (1 - t) * from + 2 * t * (1 - t) * middle + t * t * to;
        //}
        return (1 - t) * (1 - t) * from + 2 * t * (1 - t) * middle + t * t * to;
	}

	public static Vector3 LerpDirection(Vector3 from, Vector3 to)
	{
		return to - from;
	}

	public static Vector3 LerpDirection(Vector3 from, Vector3 middle, Vector3 to, float t)
	{
		Vector3 direction = Vector3.zero;
		
		if (t >= 0 && t <= 1f)
		{
			direction = (t - 1) * from + (1 - 2 * t) * middle + t * to;
		}
		
		return direction;
	}

	public static Vector3 GetMiddlePosition(Vector3 from, Vector3 to, float distance = -1f)
	{
		if (distance < 0)
		{
			distance = Vector3.Distance(from, to);
		}

		return (from + to) / 2 + Vector3.up * distance;
	}
}

