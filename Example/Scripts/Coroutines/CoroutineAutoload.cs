using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class CoroutineAutoload : Node
{
    private static readonly List<IEnumerator> routines = new List<IEnumerator>();
    private static float waitTimer = 0f;
    private static float currentTimer = 0f;

    public override void _Process(float delta)
    {
        Time.Update();
        if (waitTimer > 0)
        {
            waitTimer -= delta;
        }
        HandleCoroutines(routines);
    }

    public static void HandleCoroutines(List<IEnumerator> coroutineList) 
    {
        for (int i = 0; i < coroutineList.Count; i++)
        {
            if (coroutineList[i].Current is float time && waitTimer <= 0)
            {
                if (currentTimer > 0f)
                {
                    currentTimer = 0f;
                    i = Move(coroutineList, i);
                    continue;
                }
                waitTimer = time;
                currentTimer = time;
            }
            if (coroutineList[i].Current is IEnumerator enumerator)
            {
                if (Advance(enumerator))
                {
                    continue;
                }
            }
            if (coroutineList[i].Current is float)
            {
                continue;
            }
            i = Move(coroutineList, i);
        }
    }

    private static int Move(List<IEnumerator> coroutineList, int i)
    {
        if (!coroutineList[i].MoveNext())
        {
            coroutineList.RemoveAt(i--);
        }

        return i;
    }

    public static bool Advance(IEnumerator routine)
    {
        if (routine.Current is IEnumerator enumerator)
        {
            if (Advance(enumerator))
            {
                return true;
            }
        }
        if (routine.Current is float time)
        {
            if (time > Time.time + time)
            {
                return routine.MoveNext();
            }
        }
        return routine.MoveNext();
    }

    public static Coroutine StartCoroutines(IEnumerator method) 
    {
        routines.Add(method);
        return new Coroutine(method);
    }

    public static void StopCoroutines(IEnumerator method) 
    {
        routines.Remove(method);
    }
    public static void StopAllCoroutines() 
    {
        routines.Clear();
    }

    ////////////////////////////////////////// Dead Code ////////////////////////////////////////////////
    //                  Bring it back if you dont want to have a float awaiter                         //
    /////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /*
    //public static void OldHandleCoroutines(List<IEnumerator> coroutineList)
    {
        for (int i = 0; i < coroutineList.Count; i++)
        {
            if (coroutineList[i].Current is IEnumerator enumerator)
            {
                if (Advance(enumerator))
                {
                    continue;
                }
            }
            if (!coroutineList[i].MoveNext())
            {
                coroutineList.RemoveAt(i--);
            }
        }
    }
    */

}
