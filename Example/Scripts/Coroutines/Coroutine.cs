using Godot;
using System;
using System.Collections;

public sealed class Coroutine : YieldInstruction
{
    public readonly IEnumerator routine;
    public Coroutine(IEnumerator routine) 
    {
        this.routine = routine;
    }
    public override bool Condition => routine.MoveNext();
}

public class WaitForSeconds : YieldInstruction
{
    private readonly float finishTime;

    public WaitForSeconds(float seconds)
    {
        this.finishTime = Time.time + seconds;
    }

    public override bool Condition => finishTime > Time.time;
}

public class WaitForSecondsRealTime : YieldInstruction
{
    private readonly float finishTime;
    public WaitForSecondsRealTime(float seconds)
    {
        this.finishTime = seconds;
    }

    public override bool Condition => finishTime > Time.realTime;
}

public class WaitUntil : YieldInstruction
{
    private Func<bool> UntilCall {get;}
    public WaitUntil(Func<bool> action)
    {
        this.UntilCall = action;   
    }

    public override bool Condition => !UntilCall();
}

public class WaitWhile : YieldInstruction
{
    private Func<bool> WhileLoop {get;}
    public WaitWhile(Func<bool> whileLoop)
    {
        this.WhileLoop = whileLoop;
    }

    public override bool Condition => WhileLoop();
}

public class WaitForInput : YieldInstruction
{
    private readonly string inputName;
    public WaitForInput(string inputName)
    {
        this.inputName = inputName;
    }

    public override bool Condition => !Input.IsActionJustPressed(inputName);
}

public class WaitForSignal : YieldInstruction
{
    private bool toSignal = true;
    public WaitForSignal(Node node, string signalName)
    {
        node.Connect(signalName, new CoroutineObject(OnWait), nameof(CoroutineObject.Invoke),
            new Godot.Collections.Array() {signalName, node});
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
        private void QueueFree() {
            CallDeferred("free");
        }
    }
}
