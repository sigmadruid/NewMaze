using System;
using System.Collections.Generic;

namespace StaticData
{
    public class PlotData
    {
        public string Name;

        public List<ActorData> Actors = new List<ActorData>();

        public List<SegmentData> Segments = new List<SegmentData>();//Must be ordered by startTime
    }
}

