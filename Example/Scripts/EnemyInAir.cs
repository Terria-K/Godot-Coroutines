using Godot;
using Godot.Coroutines;
using System.Collections;

public class EnemyInAir : Node2D
{
    private Player player;

    public override void _Ready()
    {
        player = GetNode<Player>("../Player");
        CoroutineAutoload.StartCoroutines(KillPlayer());
    }

    private IEnumerator KillPlayer() 
    {
        yield return PlayerAwait();
        player.QueueFree();
    }

    private IEnumerator PlayerAwait()
    {
        yield return new WaitUntil(() => player.isDone);
        GD.Print("Player is done, awaiting further instructions");
        yield return new WaitForSeconds(2f);
    }
}
