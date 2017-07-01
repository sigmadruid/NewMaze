using UnityEngine;

using System;
using System.Collections.Generic;

namespace GameLogic
{
    public enum AnimatorEventType
    {
        NONE,

        SKILL_1,
        SKILL_2,

        UNSHEATH,
        SHEATH,

        HIT,
        DIE,
        ROLL,
    }
    public class AnimatorStateScript : StateMachineBehaviour
    {
        public AnimatorEventType StartEvent;
        public AnimatorEventType EndEvent;

        public float[] EventTimes;
        public AnimatorEventType[] MiddleEvents;

        private CharacterScript script;
        private int startIndex;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            startIndex = 0;
            if(script == null)
                script = animator.gameObject.GetComponent<CharacterScript>();
            if (StartEvent != AnimatorEventType.NONE)
                script.OnAnimatorStart(StartEvent);
            
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EndEvent != AnimatorEventType.NONE)
                script.OnAnimatorEnd(EndEvent);
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for(int i = startIndex; i < EventTimes.Length; ++i)
            {
                float time = EventTimes[i];
                if(stateInfo.normalizedTime > time)
                {
                    script.OnAnimatorMiddle(MiddleEvents[i]);
                    startIndex++;
                }
            }
        }
    }
}

