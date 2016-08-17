using System;

using System.Collections.Generic;

namespace StaticData
{
	public enum AnimatorPriorityEnum
	{
		Idle = 1,
		Run,
		Attack,
		Hit,
		Die,
	}

	public enum AnimatorParamKey
	{
		AreaType = 1,
		Range,
		Angle,
		Width,
	}

	public class AnimatorData
	{
		public int Kid;

		public string Name;
		public int NameHash;

		public bool IsLoop;

		public AnimatorPriorityEnum Priority;

		public float NormalTime;

		public List<AnimatorParamKey> ParamKeyList = new List<AnimatorParamKey>();
		public List<int> ParamValueList = new List<int>();

        public Dictionary<AnimatorParamKey, string> ParamDic = new Dictionary<AnimatorParamKey, string>();
	}
}

