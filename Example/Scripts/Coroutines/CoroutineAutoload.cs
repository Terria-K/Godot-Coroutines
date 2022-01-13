using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class CoroutineAutoload : Node
{
    private static List<IEnumerator> coroutines = new List<IEnumerator>();
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        Time.Update();
        HandleCoroutines(coroutines);
    }

    public static void HandleCoroutines(List<IEnumerator> coroutinesList) 
    {
        for (int i = 0; i < coroutines.Count; i++) 
        {
            bool yielded = coroutinesList[i].Current is YieldInstruction yielder && yielder.MoveNext();
            if (yielded || coroutinesList[i].MoveNext()) continue; // Guard Clauses

            if (coroutineList.Count != 0) {
                coroutineList.RemoveAt(i);
            }
            i--;
            GC.Collect(); // This is optional
        }
    }

    public static Coroutine StartCoroutines(IEnumerator method) 
    {
        coroutines.Add(method);
        return new Coroutine(method);
    }

    public static void StopCoroutines(IEnumerator method) 
    {
        coroutines.Remove(method);
    }
    public static void StopAllCoroutines() 
    {
        coroutines.Clear();
    }

}
