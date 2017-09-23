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

        private PlayerProxy playerProxy;
        private WeaponProxy weaponProxy;
        private HeroProxy heroProxy;
        private MonsterProxy monsterProxy;
        private ExplorationProxy explorationProxy;
        private DropProxy dropProxy;
        private PackProxy packProxy;

        private GameRecord gameRecord;

        public override void OnRegister()
        {
            base.OnRegister();
            playerProxy = ApplicationFacade.Instance.RetrieveProxy<PlayerProxy>();
            heroProxy = ApplicationFacade.Instance.RetrieveProxy<HeroProxy>();
            weaponProxy = ApplicationFacade.Instance.RetrieveProxy<WeaponProxy>();
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
            if(File.Exists(persistPath))
            {
                gameRecord = null;
                using(Stream stream = new FileStream(persistPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    gameRecord = (GameRecord)formatter.Deserialize(stream);
                }
            }
            else
            {
                gameRecord = new GameRecord();
            }

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


            monsterProxy.Save();
            explorationProxy.SaveRecord();
            dropProxy.SaveRecord();

            gameRecord.RandomSeed = Maze.Instance.Seed;
            gameRecord.Player = playerProxy.Save();
            gameRecord.Heroes = heroProxy.Save();
            gameRecord.Weapons = weaponProxy.Save();
            gameRecord.Monsters = monsterProxy.RecordDic;
            gameRecord.Items = packProxy.Save();
            gameRecord.Drops = dropProxy.RecordDic;
            gameRecord.Explorations = explorationProxy.RecordDic;
        }
        private void DeserializeGame()
        {
            var facade = ApplicationFacade.Instance;

            Maze.Instance.Seed = gameRecord.RandomSeed;

            playerProxy.Init(gameRecord.Player);
            weaponProxy.Init(gameRecord.Weapons);
            heroProxy.Init(gameRecord.Heroes);
            monsterProxy.Init(gameRecord.Monsters);
            packProxy.Init(gameRecord.Items);
            dropProxy.RecordDic = gameRecord.Drops;
            explorationProxy.RecordDic = gameRecord.Explorations;
        }
    }
}

