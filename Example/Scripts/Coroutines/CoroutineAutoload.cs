using System.Collections;
using System.Collections.Generic;

namespace Godot.Coroutines
{
    public class CoroutineAutoload : Node
    {

        private static readonly CoroutineHandler handler = new CoroutineHandler();

        public override void _Ready()
        {
            AddChild(handler);
        }

        public override void _Process(float delta)
        {
            Time.Update();
        }

        public static void HandleCoroutines(List<IEnumerator> coroutineList)
        {
            for (int i = 0; i < coroutineList.Count; i++)
            {
                if (coroutineList[i].Current is IEnumerator enumerator)
                {
                    if (Advance(enumerator))
                    {
                        continue;
                    }
                }
                if (!coroutineList[i].MoveNext())
                {
                    coroutineList.RemoveAt(i--);
                }
            }
        }

        public static bool Advance(IEnumerator routine)
        {
            if (routine.Current is IEnumerator enumerator)
            {
                if (Advance(enumerator))
                {
                    return true;
                }
            }
            return routine.MoveNext();
        }

        public static Coroutine StartCoroutine(IEnumerator method, CoroutineType coroutineType = CoroutineType.Process)
        {
            return handler.StartCoroutine(method, coroutineType);
        }

        public static void StopCoroutines(IEnumerator method)
        {
            handler.StopCoroutine(method);
        }

        public static void StopCoroutines(Coroutine coroutine)
        {
            handler.StopCoroutine(coroutine);
        }

        public static void StopAllCoroutines()
        {
            handler.StopAllCoroutines();
        }

        ////////////////////////////////////////// Dead Code ////////////////////////////////////////////////
        //                  Bring it back if you want to have a bugged float awaiter                       //
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         private static float currentTimer = 0f;
         private static float waitTimer = 0f;
         public override void _Process(float delta)
         {
             Time.Update();
             if (waitTimer > 0)
             {
                 waitTimer -= delta;
             }
             HandleCoroutines(routines);
         }
        public static void HandleCoroutines(List<IEnumerator> coroutineList)
        {
            for (int i = 0; i < coroutineList.Count; i++)
            {
                if (coroutineList[i].Current is float time && waitTimer <= 0)
                {
                    if (currentTimer > 0f)
                    {
                        currentTimer = 0f;
                        i = Move(coroutineList, i);
                        continue;
                    }
                    waitTimer = time;
                    currentTimer = time;
                }
                if (coroutineList[i].Current is IEnumerator enumerator)
                {
                    if (Advance(enumerator))
                    {
                        continue;
                    }
                }
                if (coroutineList[i].Current is float)
                {
                    continue;
                }
                i = Move(coroutineList, i);
            }
        }

        private static int Move(List<IEnumerator> coroutineList, int i)
        {
            if (!coroutineList[i].MoveNext())
            {
                coroutineList.RemoveAt(i--);
            }

            return i;
        }

        public static bool Advance(IEnumerator routine)
        {
            if (routine.Current is IEnumerator enumerator)
            {
                if (Advance(enumerator))
                {
                    return true;
                }
            }
            if (routine.Current is float time)
            {
                if (time > Time.time + time)
                {
                    return routine.MoveNext();
                }
            }
            return routine.MoveNext();
        }
        */
    }

}
