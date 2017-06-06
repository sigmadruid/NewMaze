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

        public static Exploration Create(ExplorationRecord record)
        {
            ExplorationData data = ExplorationDataManager.Instance.GetData(record.Kid) as ExplorationData;
            Exploration expl = DoCreate(data, record.Uid);
            return expl;
        }

        private static Exploration DoCreate(ExplorationData data, string uid = null)
        {
            //Initialization
            Exploration exploration = null;
            switch (data.Type)
            {
                case ExplorationType.Transporter:   
                    {
                        exploration = new TransporterExpl();
                        break;
                    }
                case ExplorationType.Chest:   
                    {
                        exploration = new ChestExpl();
                        break;
                    }
                default:      
                    {
                        exploration = new Exploration();
                        break;
                    }
            }

            Exploration.Init(exploration, data, uid);

            //Post Process
            switch(data.Type)
            {
                case ExplorationType.Chest:   
                    {
                        ChestExpl.PostCreate(exploration);
                        break;
                    }
            }

            return exploration;
        }


    }
}

