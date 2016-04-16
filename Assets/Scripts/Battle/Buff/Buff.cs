using System;
using System.Collections.Generic;

using Battle;
using StaticData;

namespace Battle
{
    public class Buff
    {
        public BuffData Data;

        private float t;

        public virtual void Start()
        {
            t = 0;
        }

        public virtual void End()
        {
        }

        public virtual void Update(float deltaTime)
        {
            if(Data.Type == BuffType.Gradual)
            {
                t += deltaTime;
            }
        }

        public float GetAttribute(BattleAttribute attribute)
        {
            int attr = (int)attribute;
            if(Data.AttributeDic.ContainsKey(attr))
            {
                return Data.AttributeDic[attr];
            }
            return 0;
        }

	}
}

