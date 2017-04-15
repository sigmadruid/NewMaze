using System;

using StaticData;

namespace Battle
{
    public interface ICharacterInfoAgent
    {
        Side GetSide();
        float GetAttribute(BattleAttribute attribute);
    }
}

