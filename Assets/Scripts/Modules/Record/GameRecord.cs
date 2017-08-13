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

        public Dictionary<int, List<DropRecord>> Drops = new Dictionary<int, List<DropRecord>>();

        public Dictionary<int, ItemRecord> Items = new Dictionary<int, ItemRecord>();

        public Dictionary<int, List<ExplorationRecord>> Explorations = new Dictionary<int, List<ExplorationRecord>>();

        //TODO:NPCs

    }
}

