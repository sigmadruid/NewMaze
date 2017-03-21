using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

namespace GamePlot
{
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
            if(IsPlaying)
            {
                int i = minStep;
                while(i < segmentList.Count)
                {
                    Segment segment = segmentList[i++];
                    if(!segment.IsPlaying)
                    {
                        if(timer >= segment.Data.StartTime && timer <= segment.Data.EndTime)
                        {
                            segment.Start();
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
                                IsPlaying = false;
                            }
                            minStep++;
                        }
                    }
                }
                timer += deltaTime;
            }
        }

        public bool IsPlaying { get; private set; }

        public void Play()
        {
            Debug.Log("plot play: " + Data.Name);
            IsPlaying = true;
            minStep = 0;
            timer = 0;

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

            for(int i = 0; i < Data.Segments.Count; ++i)
            {
                Segment segment = Segment.Create(Data.Segments[i]);
                segmentList.Add(segment);
            }
        }

    }
}

