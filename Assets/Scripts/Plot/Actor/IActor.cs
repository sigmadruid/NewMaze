using UnityEngine;

using System;

namespace GamePlot
{
    public interface IActor
    {
        void SetPosition(Vector3 position);
        void SetRotation(Vector3 direction);
        void Idle();
        void Move(Vector3 destination);
        void LookAt(Vector3 destPos);
        void Skill(int skillIndex);
        void Hit();
        void Die();
        void PlayAnimation(string trigger);
    }
}

