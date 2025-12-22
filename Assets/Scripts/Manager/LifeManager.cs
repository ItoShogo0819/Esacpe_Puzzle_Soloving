using UnityEngine;
using System.Collections.Generic;

public class LifeManager : MonoBehaviour
{
    public List<RagDollLifeIcon> Icons = new();

    public int Life => Icons.Count;
    public bool IsGameOver => Icons.Count == 0;

    public void LoseLife()
    {
        if (Life <= 0 || Icons.Count == 0) return;

        var icon = Icons[^1];
        Icons.RemoveAt(Icons.Count - 1);
    }

    public void AddLife(RagDollLifeIcon newIcon)
    {
        Icons.Add(newIcon);
    }
}
