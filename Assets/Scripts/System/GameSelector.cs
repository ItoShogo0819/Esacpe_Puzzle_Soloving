using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GameSelector : MonoBehaviour
{
    public string[] NomalGames;
    public string[] BossGames;

    public List<string> CreateNewCycle()
    {
        List<string> newCycle = new();

        var shuffledNomal = NomalGames.OrderBy(x => Random.value).ToList();
        newCycle.AddRange(shuffledNomal.Take(4));

        string boss = BossGames[Random.Range(0, BossGames.Length)];
        newCycle.Add(boss);

        return newCycle;
    }
}
