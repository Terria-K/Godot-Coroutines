using Godot;
using Godot.Coroutines;
using System.Collections;

public class EnemyInAir : Node2D
{
    private Player player;

    public override void _Ready()
    {
        player = GetNode<Player>("../Player");
        CoroutineAutoload.StartCoroutine(KillPlayer());
    }

    private IEnumerator KillPlayer() 
    {
        yield return PlayerAwait();
        player.QueueFree();
    }

    private IEnumerator PlayerAwait()
    {
        while (!player.isDone) 
        {
            yield return null;
        }
        GD.Print("Player is done, awaiting further instructions");
        yield return 2f;
    }
}
