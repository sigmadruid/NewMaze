using System;
using System.Collections.Generic;

using GameLogic;
using StaticData;

namespace Battle
{
    public class SkillEffect
    {
        public string Uid;

        public SkillEffectData Data;

        public Side CasterSide;
        public float Attack;
        public int Critical;

        public void Cast()
        {
            if(Data.CameraVibrationType != CameraVibration.Type.None)
            {
                Camera3DScript.Instance.Vibrate(Data.CameraVibrationType);
            }
        }

        public static SkillEffect Create(int kid, ICharacterInfoAgent infoAgent)
        {
            SkillEffect effect = new SkillEffect();
            effect.Uid = Guid.NewGuid().ToString();
            effect.Data = SkillEffectDataManager.Instance.GetData(kid);
            effect.CasterSide = infoAgent.GetSide();
            effect.Attack = infoAgent.GetAttribute(BattleAttribute.Attack) * effect.Data.Ratio;
            effect.Critical = (int)infoAgent.GetAttribute(BattleAttribute.Critical);
            return effect;
        }
    }
}

