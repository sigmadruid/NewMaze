using UnityEngine;

using System;
using System.Collections.Generic;

using StaticData;

namespace GamePlot
{
    public class Plot
    {
        public PlotData Data { get; protected set;}

        private int step;
        private float timer;

        public void Init()
        {
            List<SegmentData> dataList = new List<SegmentData>();
            PlotDataParser dataParser = new PlotDataParser();
            dataParser.Parse("TestPlot.csv", out dataList);
            Debug.Log(dataList);
        }
        public void Dispose()
        {
        }

        public void Update()
        {
        }
    }
}

