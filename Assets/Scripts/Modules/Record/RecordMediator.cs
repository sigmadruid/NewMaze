using UnityEngine;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Base;

namespace GameLogic
{
    public class RecordMediator : Mediator
    {
        private HeroProxy heroProxy;
        private MonsterProxy monsterProxy;
        private HallProxy hallProxy;

        private static readonly string RECORD_PATH = Application.persistentDataPath + "/GameData.bin";

        public override void OnRegister()
        {
            base.OnRegister();
            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
            monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
            hallProxy = ApplicationFacade.Instance.RetrieveProxy<HallProxy>();
        }
        public override IList<Enum> ListNotificationInterests ()
        {
            return new Enum[]
            {
                NotificationEnum.SERIALIZE_GAME,
                NotificationEnum.DESERIALIZE_GAME,
            };
        }

        public override void HandleNotification (INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.SERIALIZE_GAME:
                {
                    HandleSerializeGame();
                    break;
                }
                case NotificationEnum.DESERIALIZE_GAME:
                {
                    HandleDeserializeGame();
                    break;
                }
            }
        }

        private void HandleSerializeGame()
        {
            if(Game.Instance.CurrentStageType != StageEnum.Maze)
                return;
            
            if(Hero.Instance.Info.IsAlive)
            {
                if (Hero.Instance.IsInHall)
                    monsterProxy.DoRecordInHall();
                monsterProxy.DoRecordBlock();
                hallProxy.DoRecord();

                GameRecord gameRecord = new GameRecord();
                gameRecord.RandomSeed = RandomUtils.Seed;
                gameRecord.Hero = Hero.Instance.ToRecord();
                gameRecord.MonstersInBlock = monsterProxy.RecordBlockDic;
                gameRecord.MonstersInHall = monsterProxy.RecordHallDic;
                gameRecord.Hall = hallProxy.Record;

                using(Stream stream = new FileStream(RECORD_PATH, FileMode.Create, FileAccess.ReadWrite))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, gameRecord);
                }
            }
            else//If dead, delete the record.
            {
                RecordMediator.DeleteRecord();
            }
        }
        private void HandleDeserializeGame()
        {
            string persistPath = Application.persistentDataPath + "/GameData.bin";
            if(!File.Exists(persistPath))
            {
                return;
            }
            GameRecord gameRecord = null;
            using(Stream stream = new FileStream(persistPath, FileMode.Open, FileAccess.ReadWrite))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                gameRecord = (GameRecord)formatter.Deserialize(stream);
            }
            if(gameRecord != null)
            {
                RandomUtils.Seed = gameRecord.RandomSeed;
                heroProxy.Record = gameRecord.Hero;
                monsterProxy.RecordBlockDic = gameRecord.MonstersInBlock;
                monsterProxy.RecordHallDic = gameRecord.MonstersInHall;
                hallProxy.Record = gameRecord.Hall;
            }
        }

        public static void DeleteRecord()
        {
            File.Delete(RECORD_PATH);
        }
    }
}

