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

        public float RemainTime { get; protected set; }

        protected CharacterScript script;

        public virtual void Start(CharacterScript script)
        {
            RemainTime = 0;
            this.script = script;
            script.SetEmissionColor(Data.EmissionColor, 1f);
        }

        public virtual void End()
        {
            script.SetEmissionColor(Color.black, 1f);
            this.script = null;
        }

        public virtual void Update(float deltaTime)
        {
            if(Data.Duration < 0)
                return;

            RemainTime += deltaTime;
            if(RemainTime > Data.Duration)
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

        public static Buff Create(int kid, float remainTime)
        {
            Buff buff = new Buff();
            buff.Uid = Guid.NewGuid().ToString();
            buff.Data = BuffDataManager.Instance.GetData(kid);
            buff.RemainTime = remainTime;
            return buff;
        }

	}
}

