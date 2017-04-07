using UnityEngine;
using System;

namespace Effects
{
    public class Fade : MonoBehaviour
    {
        public bool StartEnabled = true;

        private bool isEnabled;
        public float alphaFrom;
        public float alphaTo;
        public float delay;
        public float duration;

        private float timer;

        private Material material;

        void Start()
        {
            material = GetComponentInChildren<Renderer>().material;
            material.SetColor("_Color", new Color(1, 1, 1, alphaFrom));

            if(StartEnabled)
                StartEffect();
        }

        void Update()
        {
            if(!isEnabled)
                return;

            timer += Time.deltaTime;
            if(timer > delay)
            {
                if(timer > delay + duration)
                {
                    StopEffect();
                    return;
                }
                float alpha = Mathf.Lerp(alphaFrom, alphaTo, (timer - delay) / duration);
                material.SetColor("_Color", new Color(1, 1, 1, alpha));
            }
            
        }

        public void StartEffect()
        {
            timer = 0f;
            isEnabled = true;
        }
        public void StopEffect()
        {
            isEnabled = false;
        }
    }
}

