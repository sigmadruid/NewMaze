using UnityEngine;

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

        public float Duration;

        public Dictionary<int, float> AttributeRatioDic = new Dictionary<int, float>();
        public Dictionary<int, int> AttributeRaiseDic = new Dictionary<int, int>();

        public Color EmissionColor;

    }
}

