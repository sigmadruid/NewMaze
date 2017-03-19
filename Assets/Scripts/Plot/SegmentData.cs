using UnityEngine;
using System;

namespace StaticData
{
    public enum SegmentType
    {
        Animation,
        Move,
        Dialog,
        Notice,
        CameraMove,
        CameraShake,
        Monster,
        NPC,
        Exploration,
        Effect,
    };
    public class SegmentData
    {
        public int Step;
        public float StartTime;
        public float EndTime;
        public SegmentType Type;
        public string Target;
        public Vector3 Position;
        public Vector3 Orientation;
        public string Param1;
        public string Param2;
        public string Param3;
    }
}

