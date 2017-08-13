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
        private static readonly string RECORD_PATH = Application.persistentDataPath + "/GameData.bin";

        private HeroProxy heroProxy;
        private MonsterProxy monsterProxy;
        private ExplorationProxy explorationProxy;
        private DropProxy dropProxy;
        private PackProxy packProxy;

        private GameRecord gameRecord;

        public override void OnRegister()
        {
            base.OnRegister();
            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
            monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
            explorationProxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
            dropProxy = ApplicationFacade.Instance.RetrieveProxy<DropProxy>();
            packProxy = ApplicationFacade.Instance.RetrieveProxy<PackProxy>();
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
            
            if(Adam.Instance != null && Adam.Instance.Info.IsAlive)
            {
                SerializeGame();

                using(Stream stream = new FileStream(RECORD_PATH, FileMode.Create, FileAccess.ReadWrite))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, gameRecord);
                }
                string json = JsonUtility.ToJson(gameRecord);
                Debug.Log(json);
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
                Game.Instance.IsNewGame = true;
                return;
            }
            gameRecord = null;
            using(Stream stream = new FileStream(persistPath, FileMode.Open, FileAccess.ReadWrite))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                gameRecord = (GameRecord)formatter.Deserialize(stream);
            }

            Game.Instance.IsNewGame = gameRecord != null;
            if(gameRecord != null)
            {
                DeserializeGame();
            }
        }
        public static void DeleteRecord()
        {
            File.Delete(RECORD_PATH);
        }

        private void SerializeGame()
        {
            var facade = ApplicationFacade.Instance;

            gameRecord = new GameRecord();

            gameRecord.Items = packProxy.GetRecord();

            heroProxy.SaveRecord();
            monsterProxy.SaveRecord();
            explorationProxy.SaveRecord();
            dropProxy.SaveRecord();
            facade.RetrieveProxy<HallProxy>().CreateRecord();

            gameRecord.RandomSeed = Maze.Instance.Seed;
            gameRecord.Hero = heroProxy.Record;
            gameRecord.Monsters = monsterProxy.RecordDic;
            gameRecord.Hall = facade.RetrieveProxy<HallProxy>().Record;
            gameRecord.Drops = dropProxy.RecordDic;
            gameRecord.Explorations = explorationProxy.RecordDic;
        }
        private void DeserializeGame()
        {
            var facade = ApplicationFacade.Instance;

            packProxy.SetRecord(gameRecord.Items);

            Maze.Instance.Seed = gameRecord.RandomSeed;
            heroProxy.Record = gameRecord.Hero;
            monsterProxy.RecordDic = gameRecord.Monsters;
            facade.RetrieveProxy<HallProxy>().Record = gameRecord.Hall;
            dropProxy.RecordDic = gameRecord.Drops;
            explorationProxy.RecordDic = gameRecord.Explorations;
        }
    }
}

