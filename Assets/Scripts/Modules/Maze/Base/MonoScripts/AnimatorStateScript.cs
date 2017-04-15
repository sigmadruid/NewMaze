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

        DIE,
    }
    public class AnimatorStateScript : StateMachineBehaviour
    {
        public AnimatorEventType StartEvent;
        public AnimatorEventType EndEvent;

        public float[] EventTimes;
        public AnimatorEventType[] MiddleEvents;

        private AdamScript adamScript;
        private int startIndex;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            startIndex = 0;
            if(adamScript == null)
                adamScript = animator.gameObject.GetComponent<AdamScript>();
            if (StartEvent != AnimatorEventType.NONE)
                adamScript.OnAnimatorStart(StartEvent);
            
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EndEvent != AnimatorEventType.NONE)
                adamScript.OnAnimatorEnd(EndEvent);
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for(int i = startIndex; i < EventTimes.Length; ++i)
            {
                float time = EventTimes[i];
                if(stateInfo.normalizedTime > time)
                {
                    adamScript.OnAnimatorMiddle(MiddleEvents[i]);
                    startIndex++;
                }
            }
        }
    }
}

