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
    public bool b_CanGo { get; set; }

    protected virtual void Awake()
    {
        SR_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void OnMouseDown()
    {
        // D
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
                    Debug.Log("clickorigin");

                    int iCurrentTile = MapManager.GetInstance().GetCurrentPos();

                    List<int> needChangeTileList = MapManager.GetInstance().GetAdjList(iCurrentTile);
                    {
                        // candidColor���� �ֵ� ���� ������� ������
                        needChangeTileList.Add(iCurrentTile);
                        foreach (int iTargetTileIndex in needChangeTileList)
                        {
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).GetComponent<SpriteRenderer>().color = baseTileColor;
                            MapManager.GetInstance().GetBaseTile(iTargetTileIndex).b_CanGo = false;
                        }
                        needChangeTileList.Remove(iCurrentTile);
                    }

                    // ������ġ�� baseTile �߰�
                    if (MapManager.GetInstance().GetBaseTile(iCurrentTile).GetComponent<PlayerTile>() != null)
                    {
                        MapManager.GetInstance().ChangeBaseTile(iCurrentTile, MapManager.GetInstance().GetBaseTile(iCurrentTile).gameObject.AddComponent<BaseTile>());
                        MapManager.GetInstance().GetBaseTile(iCurrentTile).SetIndex(iCurrentTile);
                        Destroy(MapManager.GetInstance().GetBaseTile(iCurrentTile).gameObject.GetComponent<PlayerTile>());
                    }

                    SR_SpriteRenderer.color = playerTileColor;

                    // state�� Default����
                    StageManager.GetInstance().SetStageState(StageState.AfterMove);
                    gameObject.AddComponent<PlayerTile>();
                    gameObject.GetComponent<PlayerTile>().SetIndex(iTileIndex);

                    MapManager.GetInstance().ChangeBaseTile(iTileIndex, gameObject.GetComponent<PlayerTile>());

                    Destroy(this);
                    break;
                }
        }
    }

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
}
