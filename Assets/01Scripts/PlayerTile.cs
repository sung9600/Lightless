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
                    // ���� Ŭ���� Ÿ��, ����Ÿ�� �ٲٱ�
                    foreach (int iIndex in MapManager.GetInstance().GetAdjList(iTileIndex))
                    {
                        Debug.Log("playerTileClick"+iIndex);
                        BaseTile AdjTile = MapManager.GetInstance().GetBaseTile(iIndex);
                        AdjTile.gameObject.GetComponent<SpriteRenderer>().color = candidTileColor;
                        AdjTile.b_CanGo = true;
                    }

                    MapManager.GetInstance().SetCurrentPos(iTileIndex);

                    // state�� ClickOrigin����
                    StageManager.GetInstance().SetStageState(StageState.ClickOrigin);
                    break;
                }
            case StageState.ClickOrigin:
                {
                    // ���� Ŭ���� Ÿ��, ����Ÿ�� �ǵ�����
                    // state�� Default��
                    if (!b_CanGo)
                    {

                    }

                    // state�� Default����
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
