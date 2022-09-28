using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BaseTile : MonoBehaviour
{
    public int iTileIndex;
    private static int iCurrentTotalTileCount = 0;
    protected SpriteRenderer SR_SpriteRenderer;

    [SerializeField]
    protected TileType eTileType = TileType.Default;
    [SerializeField]
    protected GemType eGemType = GemType.Null;
    [SerializeField]
    public bool b_CanGo { get; set; }

    protected static List<int> aimPathIndex = new List<int>();    

    protected virtual void Awake()
    {
        SR_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void OnMouseDown()
    {
        if (StageManager.GetInstance().GetStageState() != StageState.Aiming)
        {
            switch (eTileType)
            {
                case TileType.Default:
                case TileType.Gem:
                    {
                        DefaultTileMove();
                        break;
                    }
                case TileType.Player:
                    {
                        PlayerTileMove();
                        break;
                    }
            }
        }
        else
        {
            switch (eTileType)
            {
                case TileType.Player:
                    {
                        break;
                    }
                case TileType.Wall:
                case TileType.Default:
                case TileType.Gem:
                    {
                        StageManager.GetInstance().AimButton.GetComponent<Buttons>().AimOnOff();
                        Shoot();
                        break;
                    }
            }
        }
    }
    protected void OnMouseEnter()
    {
        if (StageManager.GetInstance().GetStageState() == StageState.Aiming)
        {
            List<int> path = GetAimPath();
            foreach (int iPathIndex in path)
            {
                BaseTile PathTile = MapManager.GetInstance().GetBaseTile(iPathIndex);
                PathTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Colors.playerTileColor;
            }
            aimPathIndex = path;
        }
    }
    protected void OnMouseExit()
    {
        if (aimPathIndex.Count > 0)
        {
            foreach(int iPathIndex in aimPathIndex)
            {
                BaseTile PathTile = MapManager.GetInstance().GetBaseTile(iPathIndex);
                PathTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Colors.baseTileColor;
            }
            aimPathIndex.Clear();
        }
    }

    #region getset
    public void SetIndex()
    {
        iTileIndex = iCurrentTotalTileCount++;
    }
    public void SetIndex(int iTargetNum)
    {
        iTileIndex = iTargetNum;
    }
    public int GetIndex()
    {
        return iTileIndex;
    }
    public void SetTileType(TileType tileType)
    {
        eTileType = tileType;
    }
    public TileType GetTileType()
    {
        return eTileType;
    }
    #endregion

    private void DefaultTileMove()
    {
        switch (StageManager.GetInstance().GetStageState())
        {
            case StageState.ClickOrigin:
                {
                    // ���� Ŭ���� Ÿ��, ����Ÿ�� �ǵ�����
                    // state�� Default��
                    if (!b_CanGo)
                    {
                        return;
                    }
                    int iCurrentTile = MapManager.GetInstance().GetCurrentPos();


                    // tileType�ٲٱ����� �������� Ȯ��
                    if (eTileType == TileType.Gem)
                    {
                        // �κ��丮�� �����߰�
                        // stagemanager?
                        if (eGemType != GemType.Null)
                        {
                            StageManager.GetInstance().AddGemToInventory(eGemType);
                            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Colors.baseTileColor;
                        }
                    }

                    {
                        // candidColor���� �ֵ� ���� ������� ������
                        // Type�� ����
                        List<int> needChangeTileList = MapManager.GetInstance().GetAdjList(iCurrentTile);
                        needChangeTileList.Add(iCurrentTile);
                        foreach (int iTargetTileIndex in needChangeTileList)
                        {
                            BaseTile baseTile = MapManager.GetInstance().GetBaseTile(iTargetTileIndex);
                            if (baseTile.GetTileType() == TileType.Wall)
                                continue;
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).GetComponent<SpriteRenderer>().color = Colors.baseTileColor;
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).b_CanGo = false;
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).SetTileType(TileType.Default);
                        }
                        needChangeTileList.Remove(iCurrentTile);
                    }

                    SR_SpriteRenderer.color = Colors.playerTileColor;
                    SetTileType(TileType.Player);
                    // state�� Default����
                    StageManager.GetInstance().SetStageState(StageState.Default);
                    MapManager.GetInstance().SetCurrentPos(iTileIndex);
                    StageManager.GetInstance().Del_AfterMove();

                    break;
                }
            case StageState.Aiming:
                {
                    if (aimPathIndex.Contains(iTileIndex))
                    {
                        foreach (int iPathIndex in aimPathIndex)
                        {
                            BaseTile PathTile = MapManager.GetInstance().GetBaseTile(iPathIndex);
                            PathTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Colors.baseTileColor;
                        }
                        aimPathIndex.Clear();

                        StageManager.GetInstance().SetStageState(StageState.Default);
                    }
                    break;
                }
        }
    }
  
    private void PlayerTileMove()
    {
        switch (StageManager.GetInstance().GetStageState())
        {
            case StageState.Default:
                {
                    // ���� Ŭ���� Ÿ��, ����Ÿ�� �ٲٱ�
                    foreach (int iIndex in MapManager.GetInstance().GetAdjList(iTileIndex))
                    {
                        BaseTile AdjTile = MapManager.GetInstance().GetBaseTile(iIndex);
                        if (AdjTile.GetTileType() == TileType.Wall)
                            continue;
                        AdjTile.gameObject.GetComponent<SpriteRenderer>().color = Colors.candidTileColor;
                        AdjTile.b_CanGo = true;
                    }

                    MapManager.GetInstance().SetCurrentPos(iTileIndex);

                    // state�� ClickOrigin����
                    StageManager.GetInstance().SetStageState(StageState.ClickOrigin);
                    break;
                }
            case StageState.AfterMove:
                {
                    StageManager.GetInstance().SetStageState(StageState.Default);
                    break;
                }
        }
    }

    private int Shoot()
    {
        if(MapManager.GetInstance().GetBaseTile(aimPathIndex.Last()).GetTileType() == TileType.Gem)
        {
            // Stage�� Ŭ�������ǿ� ���������� �ִ��� Ȯ��
            if (!StageManager.GetInstance().GetRequiredGems().ContainsKey(StageManager.GetInstance().GetSelectedGem()))
            {
                // �׳� �κ��丮���� ���� or �׳� return
            }

            StageManager.GetInstance().ReduceRequiredGem();
            StageManager.GetInstance().SetStageState(StageState.Default);
        }
        else if(MapManager.GetInstance().GetBaseTile(aimPathIndex.Last()).GetTileType() == TileType.Wall)
        {
            // ��ֹ� ������ 1����, ������ 0�Ǹ� �ı�
            List<Obstacles> tempObstacles = StageManager.GetInstance().GetObstacles().ToList();
            foreach (Obstacles obstacle in tempObstacles)
            {
                if (obstacle.ContainPosition(aimPathIndex.Last()))
                {
                    //TODO : ���� ���� ������ �ٸ���
                    obstacle.GetDamage(1);
                }
            }
        }
        else
        {
            // �׳� �� �Ҹ�
        }

        StageManager.GetInstance().SetStageState(StageState.Default);

        return 0;
    }
    
    private List<int> GetAimPath()
    {
        List<int> path = new List<int>();
        {
            int iStartIndex = MapManager.GetInstance().GetCurrentPos();
            int iTargetIndex = iTileIndex;
            //Debug.Log(iStartIndex + "," + iTargetIndex);

            if (!MapManager.GetInstance().GetAdjList(iStartIndex).Contains(iTargetIndex))
            {
                return path;
            }
            int iDirection = -1;
            {
                List<(int, int, int)> startAdjInfo = MapManager.GetInstance().GetListEdge(iStartIndex);

                foreach ((int, int, int) adjInfo in startAdjInfo)
                {
                    if (adjInfo.Item1 == iStartIndex && adjInfo.Item3 == iTargetIndex)
                    {
                        iDirection = adjInfo.Item2;
                    }
                }

                if (iDirection == -1)
                    return path;
            }
            path = MapManager.GetInstance().GetShootingPath(iStartIndex, iDirection);

            //���⼭ ��Ÿ��� ���� �߶󳻸� �ɵ�?
        }
        return path;
    }
}
