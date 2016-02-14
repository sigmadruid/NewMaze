using UnityEngine;

using System;

namespace Effects
{
    public class UpAndDown : MonoBehaviour
    {
        public float MaxHeight = 1f;

        public float Duration = 1f;

        private Vector3 originPosition;
        private Vector3 targetPosition;

        private float speed;
        private float timer;

        void Start()
        {
            timer = 0f;
            speed = MaxHeight / Duration;
            originPosition = transform.position;
            targetPosition = originPosition + Vector3.up * MaxHeight;
        }

        void Update()
        {
            if(timer <= Duration * 0.5f)
                transform.position = originPosition + Vector3.up * timer * speed;
            else
                transform.position = targetPosition - Vector3.up * timer * speed;

            timer += Time.deltaTime;

            if(timer >= Duration)
                timer = 0f;
        }
    }
}

