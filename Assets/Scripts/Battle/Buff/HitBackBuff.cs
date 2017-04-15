using UnityEngine;

using System;
using System.Collections.Generic;

using Battle;
using GameLogic;
using StaticData;

namespace Battle
{
    public class HitBackBuff : Buff
    {
        private Monster monster;

        public override void Start(CharacterScript script)
        {
            base.Start(script);
            monster = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>().GetMonster(script.Uid);
            monster.Info.IsStunned = true;
        }

        public override void End()
        {
            base.End();
            monster.Info.IsStunned = false;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            float speed = float.Parse(Data.Param1);
            script.SimpleMove(-script.transform.forward * deltaTime * speed);
        }
    }
}