using System.Collections;
using System.Collections.Generic;

namespace Godot.Coroutines
{
    public class CoroutineHandler : Node2D
    {
        private List<IEnumerator> coroutines = new List<IEnumerator>();

        public Coroutine StartCoroutines(IEnumerator method)
        {
            coroutines.Add(method);
            return new Coroutine(method);
        }

        public override void _Process(float delta) => HandleCoroutines();

        private void HandleCoroutines() => CoroutineAutoload.HandleCoroutines(coroutines);

        private void StopCoroutines(IEnumerator method)
        {
            coroutines.Remove(method);
        }
        private void StopAllCoroutines()
        {
            coroutines.Clear();
        }
    }
}
