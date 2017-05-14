using System;

namespace GameLogic
{
    [Serializable]
    public class HeroRecord : EntityRecord
    {
        public int HP;

        public int Level;

        public int Exp;

        public bool IsVisible;

        public bool IsInHall;

    }
}
