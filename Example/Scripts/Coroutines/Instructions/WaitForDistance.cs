﻿using System;

namespace Godot.Coroutines
{
    public sealed class WaitForDistance : YieldInstruction
    {
        private readonly Action<float> apply;
        private readonly float duration;
        private float timer;

        public WaitForDistance(float duration, Action<float> apply)
        {
            Reset();
            this.duration = duration;
            this.apply = apply;
        }

        public override bool Condition
        {
            get
            {
                apply(FixedClamp(timer / duration));
                float expectedTimer = timer;
                timer += Time.deltaTime;
                return expectedTimer < duration;
            }
        }

        public new void Reset()
        {
            timer = 0;
            base.Reset();
        }

        //Godot Clamp isn't fixed yet!
        private float FixedClamp(float value)
        {
            if (value < 0)
            {
                return 0;
            }
            if (value > 1)
            {
                return 1;
            }
            return value;
        }
    }
}


