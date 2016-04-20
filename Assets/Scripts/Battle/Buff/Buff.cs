using UnityEngine;

using System;
using System.Collections.Generic;

using Battle;
using GameLogic;
using StaticData;

namespace Battle
{
    public class Buff
    {
        public string Uid;

        public BuffData Data;

        protected CharacterScript script;
        protected float t;

        public virtual void Start(CharacterScript script)
        {
            t = 0;
            this.script = script;
            script.SetEmissionColor(Data.EmissionColor);
        }

        public virtual void End()
        {
            script.SetEmissionColor(Color.black);
            this.script = null;
        }

        public virtual void Update(float deltaTime)
        {
            if(Data.Duration < 0)
                return;

            t += deltaTime;
            if(t > Data.Duration)
            {
                End();
            }
        }

        public float GetAttributeRatio(BattleAttribute attribute)
        {
            int attr = (int)attribute;
            if(Data.AttributeRatioDic.ContainsKey(attr))
            {
                return Data.AttributeRatioDic[attr];
            }
            return 1f;
        }
        public int GetAttributeRaise(BattleAttribute attribute)
        {
            int attr = (int)attribute;
            if(Data.AttributeRaiseDic.ContainsKey(attr))
            {
                return Data.AttributeRaiseDic[attr];
            }
            return 0;
        }

        public static Buff Create(int kid)
        {
            Buff buff = new Buff();
            buff.Uid = Guid.NewGuid().ToString();
            buff.Data = BuffDataManager.Instance.GetData(kid);
            return buff;
        }

	}
}

