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

        private MonsterProxy monsterProxy;
        private ExplorationProxy explorationProxy;

        public override void OnRegister()
        {
            base.OnRegister();
            monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
            explorationProxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
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
                var facade = ApplicationFacade.Instance;

                monsterProxy.SaveRecord();
                explorationProxy.SaveRecord();

                GameRecord gameRecord = new GameRecord();
                gameRecord.RandomSeed = Maze.Instance.Seed;
                gameRecord.Adam = facade.RetrieveProxy<AdamProxy>().CreateRecord();
                gameRecord.Heroes = facade.RetrieveProxy<HeroProxy>().CreateRecord();
                gameRecord.Monsters = monsterProxy.RecordDic;
                gameRecord.Hall = facade.RetrieveProxy<HallProxy>().CreateRecord();
                gameRecord.Items = facade.RetrieveProxy<DropProxy>().CreateRecord();
                gameRecord.Explorations = explorationProxy.RecordDic;

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
            GameRecord gameRecord = null;
            using(Stream stream = new FileStream(persistPath, FileMode.Open, FileAccess.ReadWrite))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                gameRecord = (GameRecord)formatter.Deserialize(stream);
            }
            if(gameRecord != null)
            {
                var facade = ApplicationFacade.Instance;

                Maze.Instance.Seed = gameRecord.RandomSeed;
                facade.RetrieveProxy<AdamProxy>().SetRecord(gameRecord.Adam);
                facade.RetrieveProxy<HeroProxy>().SetRecord(gameRecord.Heroes);
                monsterProxy.RecordDic = gameRecord.Monsters;
                facade.RetrieveProxy<HallProxy>().SetRecord(gameRecord.Hall);
                facade.RetrieveProxy<DropProxy>().SetRecord(gameRecord.Items);
                explorationProxy.RecordDic = gameRecord.Explorations;

                Game.Instance.IsNewGame = false;
            }
        }

        public static void DeleteRecord()
        {
            File.Delete(RECORD_PATH);
        }
    }
}

