using System;

namespace Godot.Coroutines
{
    public sealed class WaitForSignal : YieldInstruction
    {
        private bool toSignal = true;
        public WaitForSignal(Node node, string signalName)
        {
            node.Connect(signalName, new CoroutineObject(OnWait), nameof(CoroutineObject.Invoke),
                new Godot.Collections.Array() { signalName, node });
        }

        private void OnWait()
        {
            toSignal = false;
        }

        public override bool Condition => toSignal;

        private sealed class CoroutineObject : Godot.Object
        {
            private readonly Action returnTrue;

            public CoroutineObject(Action returnTrue)
            {
                this.returnTrue = returnTrue;
            }

            //If the method is not found, that is because most signals do have their own unique parameters.
            //To solve it, add your method signal that has a parameters in it.
            public void Invoke(string signal, Node node)
            {
                returnTrue();
                node.Disconnect(signal, this, "Invoke");
                QueueFree();
            }
            public void Invoke(int result, int responseCode, string[] headers, byte[] body, string signal, Node node)
            {
                returnTrue();
                node.Disconnect(signal, this, "Invoke");
                QueueFree();
            }
            private void QueueFree()
            {
                CallDeferred("free");
            }
        }
    }
}

