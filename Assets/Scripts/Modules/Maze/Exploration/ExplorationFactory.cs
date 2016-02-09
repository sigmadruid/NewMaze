using System;
using System.Collections.Generic;

using StaticData;

namespace GameLogic
{
    public static class ExplorationFactory
    {
        public static Exploration Create(int kid, List<object> paramList = null)
        {
            ExplorationData data = ExplorationDataManager.Instance.GetData(kid) as ExplorationData;
            Exploration expl = DoCreate(data, paramList);
            return expl;
        }

        public static Exploration Create(ExplorationType type, List<object> paramList = null)
        {
            ExplorationData data = ExplorationDataManager.Instance.GetRandomData(type);
            Exploration expl = DoCreate(data, paramList);
            return expl;
        }

        private static Exploration DoCreate(ExplorationData data, List<object> paramList)
        {
            Exploration exploration = null;
            switch (data.Type)
            {
                case ExplorationType.Transporter:   
                {
                    TransporterExpl expl = new TransporterExpl();
                    expl.Data = data;
                    TransporterExpl.Init(expl, (TransporterDirectionType)paramList[0]);
                    exploration = expl;
                    break;
                }
                default:      
                {
                    exploration = new Exploration();
                    exploration.Data = data;
                    Exploration.Init(exploration);
                    break;
                }
            }
            return exploration;
        }

    }
}

