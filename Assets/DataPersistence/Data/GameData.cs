using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public Vector3 snowTestPlayerPosition;
    public int killCount;

    public GameData()
    {
        snowTestPlayerPosition = Vector3.zero;
        killCount = 0;
    }
}
