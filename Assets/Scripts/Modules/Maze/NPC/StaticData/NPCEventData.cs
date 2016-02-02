using System;
using System.Collections.Generic;

namespace StaticData
{
	public enum NPCEventType
	{
		Normal,
		Result,
		Task,
	}

    public class NPCEventData
    {
		public int Kid;

		public NPCAppearScene AppearScene;
		public NPCEventType Type;

		public List<int> FirstTalkList;
		public List<int> InTaskTalkList;
		public List<int> FinishTalkList;
		public List<int> EndTalkList;

		//TODO: Reward Info;
    }
}

