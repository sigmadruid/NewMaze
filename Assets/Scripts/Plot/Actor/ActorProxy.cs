using System;
using System.Collections.Generic;

using Base;

namespace GamePlot
{
    public class ActorProxy : Proxy
    {
        private Dictionary<string, IActor> actorDic = new Dictionary<string, IActor>();

        public IActor GetActor(string name)
        {
            if(!actorDic.ContainsKey(name))
            {
                BaseLogger.LogFormat("Can't find plot actor. name={0}", name);
            }
            return actorDic[name];
        }
        public void AddActor(string name, IActor actor)
        {
            if(!actorDic.ContainsKey(name))
            {
                actorDic[name] = actor;
            }
        }
        public void RemoveActor(string name)
        {
            if(!actorDic.ContainsKey(name))
            {
                BaseLogger.LogFormat("Can't find plot actor. name={0}", name);
            }
            actorDic[name] = null;
            //TODO: Dispose actor
        }
    }
}

