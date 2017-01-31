using System;
using System.Collections.Generic;

using StaticData;

namespace GameLogic
{
    public static class ExplorationFactory
    {
        public static Exploration Create(int kid)
        {
            ExplorationData data = ExplorationDataManager.Instance.GetData(kid) as ExplorationData;
            Exploration expl = DoCreate(data);
            return expl;
        }

        public static Exploration Create(ExplorationType type)
        {
            ExplorationData data = ExplorationDataManager.Instance.GetRandomData(type);
            Exploration expl = DoCreate(data);
            return expl;
        }

        private static Exploration DoCreate(ExplorationData data)
        {
            Exploration exploration = null;
            switch (data.Type)
            {
                case ExplorationType.Transporter:   
                {
                    exploration = new TransporterExpl();
                    TransporterExpl.Init(exploration, data);
                    break;
                }
                default:      
                {
                    exploration = new Exploration();
                    Exploration.Init(exploration, data);
                    break;
                }
            }
            return exploration;
        }

    }
}

