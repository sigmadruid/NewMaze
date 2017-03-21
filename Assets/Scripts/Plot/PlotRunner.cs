using System;
using System.Collections.Generic;

using Base;
using StaticData;
using GameLogic;

namespace GamePlot
{
    public class PlotRunner
    {
        private Dictionary<string, Plot> plotDic = new Dictionary<string, Plot>();
        private Plot currentPlot;

        public void Init()
        {
            Plot plot = new Plot();
            plot.CallbackEnd = OnEnd;
            plot.Init("TestPlot");
            plotDic.Add(plot.Data.Name, plot);

            //TODO:load all plot record and instantiate plots
        }
        public void Dispose()
        {
        }

        public void Update(float deltaTime)
        {
            if(IsPlaying)
            {
                currentPlot.Update(deltaTime);
            }
        }

        public bool IsPlaying
        {
            get { return currentPlot != null; }
        }
        public void Play(string plotName)
        {
            if(IsPlaying)
                return;
            if(!plotDic.ContainsKey(plotName))
            {
                BaseLogger.Log("Can't find plot: " + plotName);
            }

            currentPlot = plotDic[plotName];
            currentPlot.Play();

            Game.Instance.InputManager.Enable = false;
        }
        private void OnEnd()
        {
            currentPlot = null;

            Game.Instance.InputManager.Enable = true;
        }
    }
}

