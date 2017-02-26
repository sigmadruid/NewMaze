using System;

namespace StaticData
{
    public enum AreaType
    {
        Circle = 1,
        Fan,
        Rectangle,
    }

    public class AreaData
    {
        public int Kid;

        public AreaType AreaType;

        public float Param1;    
        public float Param2;
    }
}

