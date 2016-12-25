using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class GameRecord
    {
        public int RandomSeed;

        public HeroRecord Hero;

        public Dictionary<int, List<MonsterRecord>> Monsters = new Dictionary<int, List<MonsterRecord>>();

        public HallRecord Hall;

        public Dictionary<int, List<ItemRecord>> Items = new Dictionary<int, List<ItemRecord>>();

        //TODO:NPCs
        //TODO:Explorations

    }
}

