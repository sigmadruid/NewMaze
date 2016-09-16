using System;

using StaticData;

namespace GameLogic
{
    public class EntityInfo
    {
        protected EntityData data;
        public EntityData Data
        {
            get { return data; }
            protected set { data = value; }
        }
    }
}

