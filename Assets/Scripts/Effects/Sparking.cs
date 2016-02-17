using UnityEngine;

using System;

namespace Effects
{
    public class Sparking : MonoBehaviour
    {
        public bool IsEnabled = true;

        public float MaxEmission = 1f;

        public float MinEmission = 0f;

        public float Duration = 1f;

        private float timer;
        private float t;

        private Material material;

        void Start()
        {
            material = GetComponentInChildren<Renderer>().material;
            material.EnableKeyword("_EMISSION");

            timer = 0f;
            t = 0f;
        }

        void Update()
        {
            if(!IsEnabled) return;

            timer += Time.deltaTime;
            if(timer < Duration * 0.5f)
            {
                t += Time.deltaTime / Duration;
            }
            else if(timer > Duration * 0.5f && timer < Duration)
            {
                t -= Time.deltaTime / Duration;
            }
            else
            {
                timer = 0f;
                t = 0f;
            }
            float val = Mathf.Lerp(MinEmission, MaxEmission, t);
            material.SetColor("_EmissionColor", Color.white * val);
        }
    }
}

