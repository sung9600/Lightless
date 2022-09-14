using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseTile : MonoBehaviour
{
    public int iTileIndex;
    private static int iCurrentTotalTileCount = 0;
    protected SpriteRenderer SR_SpriteRenderer;

    protected static Color playerTileColor = new Color(0, 1, 0);
    protected static Color baseTileColor = new Color(1, 1, 1);
    protected static Color candidTileColor = new Color(0, 0, 1);

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
                case TileType.Default:
                case TileType.Gem:
                    {
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
                PathTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = playerTileColor;
            }
            aimPathIndex = path;
        }
    }
    protected void OnMouseExit()
    {
        if (StageManager.GetInstance().GetStageState() == StageState.Aiming && aimPathIndex.Count > 0)
        {
            foreach(int iPathIndex in aimPathIndex)
            {
                BaseTile PathTile = MapManager.GetInstance().GetBaseTile(iPathIndex);
                PathTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = baseTileColor;
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
                            transform.GetChild(0).GetComponent<SpriteRenderer>().color = baseTileColor;
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
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).GetComponent<SpriteRenderer>().color = baseTileColor;
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).b_CanGo = false;
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).SetTileType(TileType.Default);
                        }
                        needChangeTileList.Remove(iCurrentTile);
                    }

                    SR_SpriteRenderer.color = playerTileColor;
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
                            PathTile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = baseTileColor;
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
                        AdjTile.gameObject.GetComponent<SpriteRenderer>().color = candidTileColor;
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
        // TODO : ��ֹ� ����ó��
        int iStartIndex = MapManager.GetInstance().GetCurrentPos();
        int iTargetIndex = iTileIndex;

        if (!MapManager.GetInstance().GetAdjList(iStartIndex).Contains(iTargetIndex))
            return -1;

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
        }

        if (iDirection == -1)
            return -1;

        int iNextStart = iTargetIndex;
        while (true)
        {
            List<(int, int, int)> startAdjInfo = MapManager.GetInstance().GetListEdge(iNextStart);

            bool bSuccess = false;
            foreach((int,int,int) adjinfo in startAdjInfo)
            {
                if(adjinfo.Item2 == iDirection)
                {
                    iNextStart = adjinfo.Item3;
                    bSuccess = true;
                }
            }

            if (!bSuccess || MapManager.GetInstance().GetBaseTile(iNextStart).GetTileType()==TileType.Wall)
                break;
        }

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

            int iNextStart = iTargetIndex;
            path.Add(iNextStart);

            if (MapManager.GetInstance().GetBaseTile(iNextStart).GetTileType() == TileType.Wall)
                return path;

            while (true)
            {
                List<(int, int, int)> startAdjInfo = MapManager.GetInstance().GetListEdge(iNextStart);

                bool bSuccess = false;
                foreach ((int, int, int) adjinfo in startAdjInfo)
                {
                    if (adjinfo.Item2 == iDirection)
                    {
                        iNextStart = adjinfo.Item3;
                        bSuccess = true;
                        path.Add(iNextStart);
                    }
                }

                // ��ã�Ұų�, �ֱ��ѵ� wall�ΰ��
                if (!bSuccess)
                {
                    break;
                }
            }

        }

        foreach(int i in path)
        {
            Debug.Log(i);
        }
        return path;
    }
}
