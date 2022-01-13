using Godot;
using System;
using System.Collections;

public class EnemyInAir : Node2D
{
    private Player player;
    public override void _Ready()
    {
        player = GetNode<Player>("../Player");
        CoroutineAutoload.StartCoroutines(SevereError());   
    }

    private IEnumerator SevereError() 
    {
        yield return new WaitForSeconds(5f);
        player.QueueFree();
    }
}
