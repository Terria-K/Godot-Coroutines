using Godot;
using Godot.Coroutines;
using System.Collections;

public class Player : KinematicBody2D
{
    private Sprite sprite;
    private CoroutineHandler handler = new CoroutineHandler();
    public bool isDone;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("PlayerSprite");   
        AddChild(handler);
        handler.StartCoroutines(FreeSpriteCoroutines());
    }

    private IEnumerator FreeSpriteCoroutines() 
    {
        yield return new WaitForDistance(5f, progress => GD.Print($"Percent of Completion: {progress.ToString()}"));
        isDone = true;
        yield break;
    }
}
