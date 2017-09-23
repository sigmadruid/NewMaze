using UnityEngine;

using System;

namespace GameLogic
{
    [Serializable]
    public class ColorRecord
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public ColorRecord(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public Color ToColor()
        {
            return new Color(r, g, b, a);
        }
    }
}

