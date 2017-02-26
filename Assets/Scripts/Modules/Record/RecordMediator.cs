﻿using UnityEngine;

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
        private DropProxy dropProxy;

        private static readonly string RECORD_PATH = Application.persistentDataPath + "/GameData.bin";

        public override void OnRegister()
        {
            base.OnRegister();
            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
            monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
            hallProxy = ApplicationFacade.Instance.RetrieveProxy<HallProxy>();
            dropProxy = ApplicationFacade.Instance.RetrieveProxy<DropProxy>();
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
                monsterProxy.DoRecord();
                hallProxy.DoRecord();
                dropProxy.DoRecord();

                GameRecord gameRecord = new GameRecord();
                gameRecord.RandomSeed = Maze.Instance.Seed;
                gameRecord.Hero = Adam.Instance.ToRecord();
                gameRecord.Monsters = monsterProxy.RecordDic;
                gameRecord.Hall = hallProxy.Record;
                gameRecord.Items = dropProxy.RecordDic;

                using(Stream stream = new FileStream(RECORD_PATH, FileMode.Create, FileAccess.ReadWrite))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, gameRecord);
                }

                string json = LitJson.JsonMapper.ToJson(gameRecord);
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
                RandomUtils.Seed = gameRecord.RandomSeed;
                heroProxy.Record = gameRecord.Hero;
                monsterProxy.RecordDic = gameRecord.Monsters;
                hallProxy.Record = gameRecord.Hall;
                dropProxy.RecordDic = gameRecord.Items;
            }
        }

        public static void DeleteRecord()
        {
            File.Delete(RECORD_PATH);
        }
    }
}

