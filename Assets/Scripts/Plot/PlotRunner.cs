using System;
using System.Collections.Generic;

namespace GamePlot
{
    public class PlotRunner
    {
        private Dictionary<string, Plot> plotDic = new Dictionary<string, Plot>();
        private Plot currentPlot;

        public void Init()
        {
            Plot plot = new Plot();
            plot.Init();
            //TODO:load all plot record and instantiate plots
        }
        public void Dispose()
        {
        }

        public void Update()
        {
        }
    }
}

