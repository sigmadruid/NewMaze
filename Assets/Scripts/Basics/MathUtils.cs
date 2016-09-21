using UnityEngine;

using System;

namespace Base
{
	public enum AreaType
	{
		Circle = 1,
		Fan,
		Rectangle,
	}

	public class MathUtils
	{
		public static float XZSqrDistance(Vector3 from, Vector3 to)
		{
			return (from.x - to.x) * (from.x - to.x) + (from.z - to.z) * (from.z - to.z);
		}
		public static float XZDistance(Vector3 from, Vector3 to)
		{
			return Mathf.Sqrt((from.x - to.x) * (from.x - to.x) + (from.z - to.z) * (from.z - to.z));
		}
		public static Vector3 XZDirection(Vector3 from, Vector3 to)
		{
			return new Vector3(to.x - from.x, 0, to.z - from.z);
		}

		public static bool CircleContains(Vector3 basePos, float radius, Vector3 testPos)
		{
			return XZSqrDistance(testPos, basePos) <= radius * radius;
		}
		public static bool RectContains(Vector3 basePos, Vector3 direction, float width, float length, Vector3 testPos)
		{
			Vector3 farPos = basePos + direction.normalized * length;

			float dotA = Vector3.Dot(farPos - basePos, testPos - basePos);
			float dotB = Vector3.Dot(basePos - farPos, testPos - farPos);
//			BaseLogger.Log(dotA, dotB);
			if (dotA < 0 || dotB < 0)
			{
				return false;
			}

			float a = Vector3.Distance(basePos, testPos);
			float b = Vector3.Distance(farPos, testPos);
			float c = Vector3.Distance(basePos, farPos);
			float p = (a + b + c) / 2;
			float s = Mathf.Sqrt(p * (p - a) * (p - b) * (p - c));
			float distance = 2 * s / c;
			if (distance > width)
			{
				return false;
			}

			return true;
		}
		public static bool FanContains(Vector3 basePos, Vector3 forward, Vector3 testPos, float distance, float angle)
		{
			if (Vector3.SqrMagnitude(testPos - basePos) <= distance * distance)
			{
				return Vector3.Angle(forward, testPos - basePos) < angle;
			}
			return false;
			
		}

		public static Vector3 RandomOffset(float distance)
		{
			Vector3 offset = UnityEngine.Random.onUnitSphere;
			offset.y = 0;
			return offset * distance;
		}
	}
}

