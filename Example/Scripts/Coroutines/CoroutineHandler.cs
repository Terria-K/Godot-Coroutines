using System.Collections;
using System.Collections.Generic;

namespace Godot.Coroutines
{
    public class CoroutineHandler : Node
    {
        private readonly List<IEnumerator> routines = new List<IEnumerator>();
        private List<float> delays = new List<float>();

        public override void _Process(float delta)
        {
            HandleCoroutines(routines);
        }

        public void HandleCoroutines(List<IEnumerator> coroutineList)
        {
            if (coroutineList.Count < 0) 
            {
                return;
            }
            for (int i = 0; i < coroutineList.Count; i++)
            {
                if (delays[i] > 0f) 
                {
                    delays[i] -= Time.deltaTime;
                    continue;
                }
                if (!Advance(coroutineList[i], i))
                {
                    coroutineList.RemoveAt(i);
                    delays.RemoveAt(i--);   
                }
            }
        }

        private bool Advance(IEnumerator routine, int current)
        {
            if (routine.Current is IEnumerator enumerator)
            {
                if (Advance(enumerator, current))
                {
                    return true;
                }
                delays[current] = 0f;
            }
            bool doMove = routine.MoveNext();

            if (routine.Current is float time) 
                delays[current] = time;

            return doMove;
        }

        public Coroutine StartCoroutine(IEnumerator method)
        {
            routines.Add(method);
            delays.Add(0f);
            return new Coroutine(method);
        }

        public void StopCoroutine(IEnumerator method)
        {
            int i = routines.IndexOf(method);
            routines.RemoveAt(i);
            delays[i] = 0f;
        }
        
        public void StopCoroutine(Coroutine coroutine) 
        {
            StopCoroutine(coroutine.routine);
        }

        public void StopAllCoroutines()
        {
            routines.Clear();
            delays.Clear();
        }
    }
}
