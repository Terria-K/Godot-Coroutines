# Godot-coroutines
A coroutines for Godot C# with yielding and instruction.

I implement this coroutines because I had some issues using async-await on C#. I am using this in most of my projects and I can say it's very nice indeed. I don't have any performance benchmark on this so I don't know how is this perform well compare to async-await. It support running multiple coroutines at once just like how threads works, but be aware that this is not a thread, it uses a game loop instead.

# How to Use It?

Here's an example on how to use this coroutines. It's not that hard but it's worth it.

**Global Coroutines (Unsafe)**
This start a coroutine globally, which means if a scene instance is gone, the coroutine is still yielding. Make sure to finish it or stop first before freeing it.
```c#
using Godot;
using System.Collections;
using System.Collections.Generic;

public class CoroutineTest : Node2D 
{
    [Signal]
    public delegate void SignalCall();
    private bool beLate = false;

    public override void _Ready() 
    {
        CoroutineAutoload.StartCoroutine(WaitASecond()); // Prints out "Waited" if 2 seconds has passed out
        CoroutineAutoload.StartCoroutine(WaitAnInput()); // Emit a signal when input was pressed
        CoroutineAutoload.StartCoroutine(WaitASignal()); // Prints out "Emitted" if User pressed space Button
        CoroutineAutoload.StartCoroutine(WaitAWhile()); // Prints out "Im Late" if the condition isn't met
    }
    
    private IEnumerator WaitASecond() 
    {
        yield return new WaitForSeconds(2f);
        GD.Print("Waited"); // Will print after 2 seconds.
        yield return new WaitForSeconds(10f);
        beLate = true;
    }
    
    private IEnumerator WaitAnInput() 
    {
        yield return new WaitForInput("space");
        EmitSignal("SignalCall");
    }
    
    private IEnumerator WaitASignal() 
    {
        yield return new WaitForSignal(this, "SignalCall");
        GD.Print("Emitted");
    }
    private IEnumeartor WaitAWhile() 
    {
        yield return new WaitWhile(() => beLate == false);
        GD.Print("Im Late");
    }
}
```
**Handler Coroutines (Safe)**
This start a coroutine using a Node which is a child of the node of CoroutineTest. So if CoroutineTest instance was gone, the Handler instance was also gone.
```c#
using Godot;
using System.Collections;
using System.Collections.Generic;

public class CoroutineTest : Node2D 
{
    [Signal]
    public delegate void SignalCall();
    private CoroutineHandler handler = new CoroutineHandler();

    public override void _Ready() 
    {
        CoroutineAutoload.StartCoroutine(WaitASecond()); // Prints out "Waited" if 2 seconds has passed out
        CoroutineAutoload.StartCoroutine(WaitAnInput()); // Emit a signal when input was pressed
        CoroutineAutoload.StartCoroutine(WaitASignal()); // Prints out "Emitted" if User pressed space Button
        CoroutineAutoload.StartCoroutine(WaitAWhile()); // Prints out "Im Late" if the condition isn't met
    }
    
    private IEnumerator WaitASecond() 
    {
        yield return new WaitForSeconds(2f);
        GD.Print("Waited"); // Will print after 2 seconds.
        yield return new WaitForSeconds(10f);
        beLate = true;
    }
    
    private IEnumerator WaitAnInput() 
    {
        yield return new WaitForInput("space");
        EmitSignal("SignalCall");
    }
    
    private IEnumerator WaitASignal() 
    {
        yield return new WaitForSignal(this, "SignalCall");
        GD.Print("Emitted");
    }
    private IEnumeartor WaitAWhile() 
    {
        yield return new WaitWhile(() => beLate == false);
        GD.Print("Im Late");
    }
}
```

I hope everyone can understand this enough. As always, have a nice day!
