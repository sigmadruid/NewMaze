using System;

namespace GameLogic
{
    public static class AdamConstDef
    {
        public const string STATE_UNARMED = "Unarmed";
        public const string STATE_SWORD = "Sword";
        public const string STATE_AXE = "Axe";

        public const string BOOL_IS_MOVING = "IsMoving";
        public const string TRIGGER_HIT = "Hit";
        public const string TRIGGER_DIE = "Die";
        public const string TRIGGER_EXIT = "Exit";
        public readonly static string[] TRIGGER_SKILLS = new string[]
            {
                "Skill_1", "Skill_2", "Skill_3", 
            };
    }
}

