using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTile : BaseTile
{
    protected override void Awake()
    {
        base.Awake();
        if(gameObject.GetComponent<BaseTile>() != null)
        {
            iTileIndex = gameObject.GetComponent<BaseTile>().iTileIndex;
        }
    }
    protected override void OnMouseDown()
    {
        switch (StageManager.GetInstance().GetStageState())
        {
            case StageState.Default:
                {
                    // 현재 클릭한 타일, 인접타일 바꾸기
                    foreach (int iIndex in MapManager.GetInstance().GetAdjList(iTileIndex))
                    {
                        Debug.Log("playerTileClick"+iIndex);
                        BaseTile AdjTile = MapManager.GetInstance().GetBaseTile(iIndex);
                        AdjTile.gameObject.GetComponent<SpriteRenderer>().color = candidTileColor;
                        AdjTile.b_CanGo = true;
                    }

                    MapManager.GetInstance().SetCurrentPos(iTileIndex);

                    // state는 ClickOrigin으로
                    StageManager.GetInstance().SetStageState(StageState.ClickOrigin);
                    break;
                }
            case StageState.ClickOrigin:
                {
                    // 현재 클릭한 타일, 인접타일 되돌리기
                    // state는 Default로
                    if (!b_CanGo)
                    {

                    }

                    // state는 Default으로
                    StageManager.GetInstance().SetStageState(StageState.Default);
                    break;
                }
            case StageState.AfterMove:
                {
                    StageManager.GetInstance().SetStageState(StageState.Default);
                    break;
                }
        }
    }
}
