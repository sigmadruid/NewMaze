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

        public override void OnRegister()
        {
            base.OnRegister();
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

                GameRecord gameRecord = new GameRecord();
                gameRecord.RandomSeed = Maze.Instance.Seed;
                gameRecord.Adam = facade.RetrieveProxy<AdamProxy>().GetRecord();
                gameRecord.Heroes = facade.RetrieveProxy<HeroProxy>().GetRecord();
                gameRecord.Monsters = facade.RetrieveProxy<MonsterProxy>().GetRecord();
                gameRecord.Hall = facade.RetrieveProxy<HallProxy>().GetRecord();
                gameRecord.Items = facade.RetrieveProxy<DropProxy>().GetRecord();
                gameRecord.Explorations = facade.RetrieveProxy<ExplorationProxy>().GetRecord();

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
                facade.RetrieveProxy<AdamProxy>().AdamRecord = gameRecord.Adam;
                facade.RetrieveProxy<HeroProxy>().RecordDic = gameRecord.Heroes;
                facade.RetrieveProxy<MonsterProxy>().RecordDic = gameRecord.Monsters;
                facade.RetrieveProxy<HallProxy>().Record = gameRecord.Hall;
                facade.RetrieveProxy<DropProxy>().RecordDic = gameRecord.Items;
                facade.RetrieveProxy<ExplorationProxy>().RecordDic = gameRecord.Explorations;
            }
        }

        public static void DeleteRecord()
        {
            File.Delete(RECORD_PATH);
        }
    }
}

