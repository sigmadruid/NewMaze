using System;

namespace StaticData
{
    public enum AreaType
    {
        Circle = 1,
        Fan,
        Rect,
    }

    public class AreaData
    {
        public int Kid;

        public AreaType AreaType;

        public bool CenterBased;
        public float Param1;    
        public float Param2;
    }
}

