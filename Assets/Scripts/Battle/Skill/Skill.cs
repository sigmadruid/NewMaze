using System;
using System.Collections.Generic;

using StaticData;

using Base;

namespace Battle
{
    public class Skill
    {
        public string Uid;

        public SkillData Data;

        public float CD;

        public List<SkillEffect> EffectList = new List<SkillEffect>();

        public void Cast(ICharacterInfoAgent infoAgent)
        {
            CD = Data.CD;
            EffectList.Clear();
            for(int i = 0; i < Data.EffectList.Count; ++i)
            {
                SkillEffect effect = SkillEffect.Create(Data.EffectList[i], infoAgent);
                EffectList.Add(effect);
            }
        }

        public SkillEffect GetEffect(int index)
        {
            if(index < 1 || index > EffectList.Count)
            {
                BaseLogger.LogFormat("effect index out of range: {0}", index);
            }
            SkillEffect effect = EffectList[index - 1];
            return effect;
        }

        public static Skill Create(int kid, float cd)
        {
            Skill skill = new Skill();
            skill.Uid = Guid.NewGuid().ToString();
            skill.Data = SkillDataManager.Instance.GetData(kid);
            skill.CD = cd;
            return skill;
        }
    }
}

