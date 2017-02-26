using System;

using StaticData;

namespace Battle
{
    public class Skill
    {
        public string Uid;

        public SkillData Data;

        public float CD;

        public void Cast()
        {
            CD = Data.CD;
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

