using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageState
{
    Tutorial,
    Default,
    ClickOrigin,
    Pause,
    MenuOpen,
    AfterMove,
    Aiming,
    Inventory,
    GameClear,
    GameOver
}
public enum TileType
{
    Default,
    Player,
    Wall,
    Gem,
    Goal
}
public enum GemType
{
    Null,
    TempGem,
}

public class Colors
{
    public static readonly Color playerTileColor = new Color(0, 1, 0);
    public static readonly Color baseTileColor = new Color(1, 1, 1);
    public static readonly Color candidTileColor = new Color(0, 0, 1);
    public static readonly Color white = new Color(1, 1, 1);
}