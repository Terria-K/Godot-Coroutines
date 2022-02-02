using System;

public class WaitWhile : YieldInstruction
{
    private Func<bool> WhileLoop {get;}
    public WaitWhile(Func<bool> whileLoop)
    {
        WhileLoop = whileLoop;
    }

    public override bool Condition => WhileLoop();
}
