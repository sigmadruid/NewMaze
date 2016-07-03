using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class GameRecord
    {
        public int RandomSeed;

        public HeroRecord Hero;

        public Dictionary<int, List<MonsterRecord>> MonstersInBlock = new Dictionary<int, List<MonsterRecord>>();
        public Dictionary<int, List<MonsterRecord>> MonstersInHall = new Dictionary<int, List<MonsterRecord>>();

        public HallRecord Hall;

        //TODO:NPCs
        //TODO:Explorations

    }
}

