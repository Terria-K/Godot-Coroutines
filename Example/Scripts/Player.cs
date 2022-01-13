using Godot;
using System.Collections;

public class Player : KinematicBody2D
{
    private Sprite sprite;
    private CoroutineHandler handler = new CoroutineHandler();
    public override void _Ready()
    {
        sprite = GetNode<Sprite>("PlayerSprite");   
        AddChild(handler);
        handler.StartCoroutines(FreeSpriteCoroutines());
    }

    private IEnumerator FreeSpriteCoroutines() 
    {
        yield return new WaitForSeconds(10f);
        sprite.QueueFree();

        yield break;
    }
}
