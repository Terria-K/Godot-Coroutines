# Godot-coroutines
A coroutines for Godot C# with yielding and instruction.

I implement this coroutines because I had some issues using async-await on C#. I am using this in most of my projects and I can say it's very nice indeed. I don't have any performance benchmark on this so I don't know how is this perform well compare to async-await. It support running multiple coroutines at once just like how threads works, but be aware that this is not a thread, it uses a game loop instead.

# How to Use It?

Here's an example on how to use this coroutines. It's not that hard but it's worth it.

**Global Coroutines (Unsafe)**
This start a coroutine globally, which means if a scene instance is gone, the coroutine is still yielding. Make sure to finish it or stop first before freeing it.
```c#
using Godot.Coroutines;
using Godot;
using System.Collections;

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
    private IEnumerator WaitAWhile() 
    {
        // yield return new WaitWhile(() => beLate == false);
        // Suggested to use a while loop instead
        while (beLate == false) 
        {
            yield return null;
        }
        GD.Print("Im Late");
    }
}
```
**Handler Coroutines (Safe)**
This start a coroutine using a Node which is a child of the node of CoroutineTest. So if CoroutineTest instance was gone, the Handler instance was also gone.
```c#
using Godot.Coroutines;
using Godot;
using System.Collections;

public class CoroutineTest : Node2D 
{
    [Signal]
    public delegate void SignalCall();
    private CoroutineHandler handler = new CoroutineHandler();

    public override void _Ready() 
    {
        AddChild(handler);
        handler.StartCoroutine(WaitASecond()); // Prints out "Waited" if 2 seconds has passed out
        handler.StartCoroutine(WaitAnInput()); // Emit a signal when input was pressed
        handler.StartCoroutine(WaitASignal()); // Prints out "Emitted" if User pressed space Button
        handler.StartCoroutine(WaitAWhile()); // Prints out "Im Late" if the condition isn't met
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
    private IEnumerator WaitAWhile() 
    {
        // yield return new WaitWhile(() => beLate == false);
        // Suggested to use a while loop instead
        while (beLate == false) 
        {
            yield return null;
        }
        GD.Print("Im Late");
    }
}
```

There are some elements that you also have to know about, the following example will use the safe coroutines:
**Await Coroutines**
You can await for coroutines just like how `await MyFunction()` works!
```c#
using Godot.Coroutines;
using Godot;
using System.Collections;

public class EnemyInAir : Node2D
{
    private Player player;
    private CoroutineHandler handler = new CoroutineHandler();
    
    public override void _Ready()
    {
        player = GetNode<Player>("../Player");
        handler.StartCoroutines(KillPlayer());   
    }

    private IEnumerator KillPlayer() 
    {
        yield return PlayerAwait(); 
        player.QueueFree(); // Will be called once the PlayerAwait is done!
    }

    private IEnumerator PlayerAwait()
    {
        yield return new WaitUntil(() => player.isDone);
        GD.Print("Player is done, awaiting further instructions");
        yield return new WaitForSeconds(2f);
    }
}
```

**Wait for distance**
You can have a timer that shows only from 0 to 1, that means you can get the progress of the timer distance. Useful for lerp.
Idea By: WaterfordSS
Link: https://www.reddit.com/r/Unity3D/comments/6alcoj/simple_to_use_customyieldinstruction_for_running/

```c#
using Godot.Coroutines;
using Godot;
using System.Collections;

public class Player : KinematicBody2D
{
    private CoroutineHandler handler = new CoroutineHandler();

    public override void _Ready()
    {
        AddChild(handler);
        handler.StartCoroutines(MovePlayer());
    }

    private IEnumerator MovePlayer() 
    {
        yield return new WaitForDistance(5f, (progress) => 
        {
            Position = new Vector2().LinearInterpolate(new Vector2(209f, 74f), progress);
        });
        GD.Print("Successfully moved!");
        yield break;
    }
}
```

**Stop the coroutines**
You can stop the coroutines aswell! But take note, you cannot stop it directly, you have to declare a variable in a fields.
```c#
using Godot.Coroutines;
using Godot;
using System.Collections;

public class Simulation : Node2D
{
    private Coroutine aVirus;
    private CoroutineHandler handler = new CoroutineHandler();
    private Computer computer;

    public override void _Ready()
    {
        computer = GetNode<Computer>("Computer");
        aVirus = CoroutineAutoload.StartCoroutines(Virus());
        handler.StartCoroutines(AntiVirus());
    }

    private IEnumerator AntiVirus()
    {
        yield return new WaitForSeconds(2f);
        CoroutineAutoload.StopCoroutines(aVirus);
    }

    private IEnumerator Virus()
    {
        yield return new WaitForSeconds(10f);
        Kill("computer");
    }

    private void Kill() 
    {
        computer.QueueFree();
    }
}

```

I hope everyone can understand this enough. As always, have a nice day!
