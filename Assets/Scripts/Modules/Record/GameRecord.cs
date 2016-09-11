using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class GameRecord
    {
        public int RandomSeed;

        public HeroRecord Hero;

        public List<MonsterBlockRecord> MonstersInBlocks = new List<MonsterBlockRecord>();
        public List<MonsterHallRecord> MonstersInHalls = new List<MonsterHallRecord>();

        public HallRecord Hall;

        //TODO:NPCs
        //TODO:Explorations

    }
}

