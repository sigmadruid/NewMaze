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

        public List<string> TalkList;

		//TODO: Reward Info;
    }
}

