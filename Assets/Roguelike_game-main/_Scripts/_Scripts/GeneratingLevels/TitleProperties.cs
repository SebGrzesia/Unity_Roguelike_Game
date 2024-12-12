using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleProperties
{
    public Vector2Int Position { get; private set; }
    public bool Passable { get; private set; }

    public TitleProperties(Vector2Int position, bool passable)
    {
        Position = position;
        Passable = passable;
    }
}
