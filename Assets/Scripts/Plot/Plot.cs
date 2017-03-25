using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

namespace GamePlot
{
    public enum PlotState
    {
        NotReady,
        Initialized,
        Prepared,
        Playing,
        Blocked,
        Ended,
    }
    public class Plot
    {
        public Action CallbackEnd;

        public PlotData Data { get; protected set;}

        private int minStep;
        private float timer;
        private List<Segment> segmentList = new List<Segment>();

        private ActorProxy actorProxy;

        public void Init(string plotName)
        {
            actorProxy = ApplicationFacade.Instance.RetrieveProxy<ActorProxy>();

            Data = new PlotData();
            Data.Name = plotName;
            ActorData actorData = new ActorData();
            actorData.Name = "Adam";
            actorData.Type = ActorType.Adam;
            actorData.StartPosition = Vector3.zero;
            actorData.StartOrientation = Vector3.zero;
            Data.Actors.Add(actorData);
            PlotDataParser dataParser = new PlotDataParser();
            dataParser.Parse(Data.Name + ".csv", out Data.Segments);

        }
        public void Dispose()
        {
            segmentList.Clear();
        }

        public void Update(float deltaTime)
        {
            if(state == PlotState.Playing)
            {
                int i = minStep;
                while(i < segmentList.Count)
                {
                    Segment segment = segmentList[i++];
                    if(!segment.IsPlaying)
                    {
                        if(timer >= segment.Data.StartTime)
                        {
                            segment.Start();
                            if(segment.Data.Type == SegmentType.Dialog)
                            {
                                state = PlotState.Blocked;
                            }
                        }
                    }
                    else
                    {
                        if(timer <= segment.Data.EndTime)
                        {
                            segment.Update(deltaTime);
                        }
                        else
                        {
                            segment.End();
                            if(minStep == segmentList.Count - 1)
                            {
                                Debug.Log("plot end: " + Data.Name);
                                CallbackEnd();
                                state = PlotState.Ended;
                            }
                            minStep++;
                        }
                    }
                }
                timer += deltaTime;
            }
            else if(state == PlotState.Blocked)
            {
                Wait();
            }
        }

        private PlotState state = PlotState.NotReady;
        public PlotState State { get { return state; } }

        public void Prepare()
        {
            Debug.Log("plot prepare: " + Data.Name);
            for(int i = 0; i < Data.Segments.Count; ++i)
            {
                Segment segment = Segment.Create(Data.Segments[i]);
                segmentList.Add(segment);
            }
            segmentList.Sort((Segment x, Segment y) =>
                {
                    return Mathf.CeilToInt(x.Data.StartTime - y.Data.StartTime);
                });
            for(int i = 0; i < Data.Actors.Count; ++i)
            {
                ActorData data = Data.Actors[i];
                if(data.Type == ActorType.Adam)
                {
                    actorProxy.AddActor(data.Name, Adam.Instance);
                }
                else if(data.Type == ActorType.Monster)
                {
                }
                else if(data.Type == ActorType.NPC)
                {
                }
                else
                {
                    BaseLogger.Log("Unsupport actor type: " + data.Type);
                }
                IActor actor = actorProxy.GetActor(data.Name);
                if (data.StartPosition != Vector3.zero)
                    actor.SetPosition(data.StartPosition);
                if (data.StartOrientation != Vector3.zero)
                    actor.SetRotation(data.StartOrientation);
            }
            state = PlotState.Prepared;
        }

        public void Play()
        {
            Debug.Log("plot play: " + Data.Name);
            state = PlotState.Playing;
            minStep = 0;
            timer = 0;
        }


        private void Wait()
        {
            if(Game.Instance.InputManager.MouseHitObject != null)
                state = PlotState.Playing;
        }
    }
}

