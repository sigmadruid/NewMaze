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
            this.script = script;
            if (Data.EmissionColor != Color.clear)
                script.SetEmissionColor(Data.EmissionColor, GlobalConfig.BattleConfig.BuffTransitionDuration);
        }

        public virtual void End()
        {
            script.SetEmissionColor(Color.black, GlobalConfig.BattleConfig.BuffTransitionDuration);
            this.script = null;
        }

        public virtual void Update(float deltaTime)
        {
            RemainTime -= deltaTime;
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
            BuffData data = BuffDataManager.Instance.GetData(kid);
            Buff buff = null;
            switch(data.SpecialType)
            {
                case BuffSpecialType.HitBack:
                    buff = new HitBackBuff();
                    break;
                default:
                    buff = new Buff();
                    break;
            }
            buff.Uid = Guid.NewGuid().ToString();
            buff.Data = data;
            buff.RemainTime = remainTime > 0 ? remainTime : data.Duration;
            return buff;
        }

	}
}

