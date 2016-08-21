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

        public List<string> FirstTalkList;
        public List<string> InTaskTalkList;
        public List<string> FinishTalkList;
        public List<string> EndTalkList;

		//TODO: Reward Info;
    }
}

