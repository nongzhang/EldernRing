using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class CooldownTimer
    {
        private float duration;
        private float timeLeft;

        public CooldownTimer(float duration)
        {
            this.duration = duration;
            this.timeLeft = 0;
        }

        public void Start()
        {
            timeLeft = duration;
        }

        public void Tick(float deltaTime)
        {
            if (timeLeft > 0f)
            {
                timeLeft -= deltaTime;
            }
        }

        public bool IsReady()
        {
            return timeLeft <= 0f;
        }
    }
}