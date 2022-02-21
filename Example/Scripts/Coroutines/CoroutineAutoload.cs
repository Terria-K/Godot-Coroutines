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

        public static Coroutine StartCoroutine(IEnumerator method)
        {
            return handler.StartCoroutine(method);
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
    }

}
