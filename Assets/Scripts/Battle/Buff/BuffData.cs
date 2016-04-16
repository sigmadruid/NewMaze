using System;
using System.Collections.Generic;

namespace StaticData
{
    public enum BuffType
    {
        Attribute,
        Gradual,
        Special,
    }
    public enum BuffSpecialType
    {
        None,
        Invisible,
    }

    public class BuffData
    {
        public int Kid;

        public BuffType Type;

        public BuffSpecialType SpecialType;

        public Dictionary<int, float> AttributeDic = new Dictionary<int, float>();

        public float Duration;
    }
}

