using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

namespace GamePlot
{
    public class Segment
    {
        public SegmentData Data;

        public bool IsPlaying;

        public IActor Actor;

        protected float timer;

        protected ActorProxy actorProxy;

        public virtual void Start() 
        {
            Debug.Log("segment start: " + Data.Type.ToString());
            IsPlaying = true;
            timer = 0f;
            actorProxy = ApplicationFacade.Instance.RetrieveProxy<ActorProxy>();
            Actor = actorProxy.GetActor(Data.Target);
        }
        public virtual void Update(float deltaTime) 
        {
            Debug.Log("segment update: " + Data.Type.ToString());
        }
        public virtual void End() 
        {
            Debug.Log("segment end: " + Data.Type.ToString());
            IsPlaying = false;
        }

        public static Segment Create(SegmentData data)
        {
            Segment segment = null;
            switch(data.Type)
            {
                case SegmentType.Animation:
                    segment = new AnimationSegment();
                    break;
                case SegmentType.Move:
                    segment = new MoveSegment();
                    break;

            }
            segment.Data = data;
            return segment;
        }
    }

    public class AnimationSegment : Segment
    {
        public override void Start()
        {
            base.Start();
            Actor.Idle();
            Actor.PlayAnimation(Data.Param1);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        public override void End()
        {
            base.End();
        }
    }

    public class MoveSegment : Segment
    {
        public override void Start()
        {
            base.Start();
            Vector3 startPos = Data.Position;
            Vector3 startDir = Data.Orientation;
            Vector3 endPos = StaticReader.ReadVector3(Data.Param1);
            if(startPos != Vector3.zero)
                Actor.SetPosition(startPos);
            if(startDir != Vector3.zero)
                Actor.SetRotation(startDir);
            Actor.Move(endPos);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        public override void End()
        {
            base.End();
            Vector3 endPos = StaticReader.ReadVector3(Data.Param1);
            Actor.SetPosition(endPos);
        }
    }
}

