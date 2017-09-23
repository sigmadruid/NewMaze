using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class GameRecord
    {
        public int RandomSeed = 0;

        public PlayerRecord Player;

        public Dictionary<int, HeroRecord> Heroes;
        public Dictionary<int, WeaponRecord> Weapons;

        public Dictionary<int, List<MonsterRecord>> Monsters = new Dictionary<int, List<MonsterRecord>>();

        public Dictionary<int, List<DropRecord>> Drops = new Dictionary<int, List<DropRecord>>();

        public Dictionary<int, ItemRecord> Items = new Dictionary<int, ItemRecord>();

        public Dictionary<int, List<ExplorationRecord>> Explorations = new Dictionary<int, List<ExplorationRecord>>();

        //TODO:NPCs

    }
}

