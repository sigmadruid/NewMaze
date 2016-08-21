using UnityEngine;

using System.Collections.Generic;

namespace Base
{
	public class RandomUtils
	{
		public const float RANDOM_BASE = 100f;

        private static int seed = 0;
		public static int Seed
		{
            get { return seed; }
			set
			{
				seed = value;
                Random.InitState(seed);
			}
		}

		public static float Value()
		{
			return Random.value;
		}

		public static int Range(int min, int max)
		{
			return Random.Range(min, max);
		}

		public static float Range(float min, float max)
		{
			return Random.Range(min, max);
		}

		public static int Weight(List<int> weightList)
		{
			int i;
			int totalWeight = 0;
			for (i = 0; i < weightList.Count; ++i)
			{
				totalWeight += weightList[i];
			}
			int weight = (int)(totalWeight * Value());
			for (i = 0; i < weightList.Count; ++i)
			{
				weight -= weightList[i];
				if (weight <= 0)
				{
					return i;
				}
			}
			return i;
		}
	}
}

