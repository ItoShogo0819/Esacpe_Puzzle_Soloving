using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LifeManager : MonoBehaviour
{
    public int Life = 5;
    public List<RagDollLifeIcon> Icon;

    public void LoseLife()
    {
        if (Life <= 0 || Icon.Count == 0) return;

        Icon[0].Exploed();
        Icon.RemoveAt(0);

        Life--;
    }

    public void AddLife(RagDollLifeIcon newIcon)
    {
        Icon.Add(newIcon);
        Life++;
    }
}
