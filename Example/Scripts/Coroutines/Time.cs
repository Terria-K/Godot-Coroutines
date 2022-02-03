using System;
namespace Godot.Coroutines
{
    public static class Time
    {
        public static float deltaTime = 0.1f;
        public static float unscaledDeltaTime = 0.1f;
        public static float fixedDeltaTime = 0.1f;
        public static float time { get; internal set; }
        public static float realTime => Godot.OS.GetTicksMsec() / 1000.0f;
        public static DateTime time1 = DateTime.Now;
        public static DateTime time2 = DateTime.Now;

        public static float timeScale
        {
            get => Godot.Engine.TimeScale;
            set => Godot.Engine.TimeScale = value;
        }
        public static int frameCount => Godot.Engine.GetFramesDrawn();

        public static void Update()
        {
            time2 = DateTime.Now;
            deltaTime = ((time2.Ticks - time1.Ticks) / 10000000f) * timeScale;
            unscaledDeltaTime = (time2.Ticks - time1.Ticks) / 10000000f;
            time1 = time2;
            time += deltaTime;
        }
    }


}
