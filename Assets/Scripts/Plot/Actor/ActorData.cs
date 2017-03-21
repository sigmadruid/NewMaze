using UnityEngine;
using System;

namespace StaticData
{
    public enum ActorType
    {
        Adam,
        Monster,
        NPC,
    };
    public class ActorData
    {
        public string Name;

        public ActorType Type;

        public Vector3 StartPosition;

        public Vector3 StartOrientation;

        public string StartTrigger;
    }
}

