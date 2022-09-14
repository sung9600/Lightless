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