using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEventSystem
{
    public static event Action<int> OnValuableCollected;
    public static event Action<int> OnPlayerScoreUpdated;

    public static void CollectValuable(int scorePoints)
    {
        OnValuableCollected.Invoke(scorePoints);
    }

    public static void PlayerScoreUpdate(int currnetScorePoints)
    {
        OnPlayerScoreUpdated(currnetScorePoints);
    }

}
