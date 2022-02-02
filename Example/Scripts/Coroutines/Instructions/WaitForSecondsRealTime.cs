public class WaitForSecondsRealTime : YieldInstruction
{
    private readonly float finishTime;
    public WaitForSecondsRealTime(float seconds)
    {
        this.finishTime = seconds;
    }

    public override bool Condition => finishTime > Time.realTime;
}
