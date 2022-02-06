using Godot;
using Godot.Coroutines;
using System.Collections;

public class Player : KinematicBody2D
{
    private readonly CoroutineHandler handler = new CoroutineHandler();
    [Export] private readonly CoroutineType coroutineType = CoroutineType.Process;
    public bool isDone;


    private Vector2 startPos = new Vector2(512, 88);
    private Vector2 endPos = new Vector2(512, 488);
    private Vector2 centerPoint;
    private Vector2 startRelCenter;
    private Vector2 endRelCenter;

    public override void _Ready()
    {
        AddChild(handler);
        handler.StartCoroutine(FreeSpriteCoroutines(), coroutineType);
    }

    private Vector2 Slerp(Vector2 src, Vector2 dst, float speed)
    {
        float len = src.Length();
        Vector2 normalizedSource = src.Normalized();
        Vector2 normalizedDistance = dst.Normalized();

        return normalizedSource.Slerp(normalizedDistance, speed) * len;
    }

    private void GetCenter(Vector2 from, Vector2 to, Vector2 direction)
    {
        centerPoint = (startPos + endPos) * 0.5f;
        centerPoint -= direction;
        startRelCenter = from - centerPoint;
        endRelCenter = to - centerPoint;
    }

    private IEnumerator FreeSpriteCoroutines() 
    {
        WaitForDistance waitForDistance = new WaitForDistance(3f, (progress) =>
        {
            GetCenter(startPos, endPos, Vector2.Up);
            Position = Slerp(startRelCenter, endRelCenter, progress);
            RotationDegrees = Mathf.Lerp(0, 360, progress);
            Position += centerPoint;
        });
        yield return waitForDistance;
        isDone = true;
        yield break;
    }
}

