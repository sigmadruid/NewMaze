using UnityEngine;
using System;

namespace GameLogic
{
    [Serializable]
    public class Vector3Record
    {
        public float x;
        public float y;
        public float z;

        public Vector3Record(Vector3 vec3)
        {
            this.x = vec3.x;
            this.y = vec3.y;
            this.z = vec3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}

