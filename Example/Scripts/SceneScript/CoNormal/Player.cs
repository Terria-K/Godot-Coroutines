using Godot;
using Godot.Coroutines;
using System.Collections;

public class Player : KinematicBody2D
{
    private readonly CoroutineHandler handler = new CoroutineHandler();
    public bool isDone;


    private Vector2 startPos = new Vector2(512, 88);
    private Vector2 endPos = new Vector2(512, 488);
    private Vector2 centerPoint;
    private Vector2 startRelCenter;
    private Vector2 endRelCenter;

    public override void _Ready()
    {
        AddChild(handler);
        handler.StartCoroutine(Multiple());
        handler.StartCoroutine(FreeSpriteCoroutines());
    }

    private IEnumerator Multiple() 
    {
        yield return 3f;
        GD.Print("RUN!");
    }

    private IEnumerator FreeSpriteCoroutines() 
    {
        yield return 3f;
        yield return AnotherBlocker();
        isDone = true;
        yield break;
    }

    private IEnumerator AnotherBlocker() 
    {
        GD.Print("Blocked Hahaha!");
        yield return 4f;
        GD.Print("Oh gosh you win");
    }
}

