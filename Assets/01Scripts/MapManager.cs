using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    private static MapManager MM_Instance;
    public static MapManager GetInstance()
    {
        return MM_Instance;
    }

    [SerializeField]
    private List<GameObject> ListTileObjects = new List<GameObject>();

    private List<BaseTile> ListBaseTiles = new List<BaseTile>();

    [SerializeField]
    private List<(int, int, int)> ListEdge = new List<(int, int, int)>();
    private Dictionary<int, List<int>> DictAdjcentIndex = new Dictionary<int, List<int>>();
    private static int iCurrentPos = 0;

    public void MyAwake()
    {
        if (MM_Instance == null)
            MM_Instance = this;
        else
            Destroy(this);

        List<List<string>> ListOriginReadData =  txtReader.Read("EdgeList");
        foreach(List<string> ListLineData in ListOriginReadData)
        {
            foreach(string str in ListLineData)
            {
                List<int> ListConvertedInt = new List<int>();
                {
                    string[] splitTarget = str.Split(' ');
                    foreach (string strWord in splitTarget)
                    {
                        ListConvertedInt.Add(int.Parse(strWord));
                    }
                }
                ListEdge.Add((ListConvertedInt[0], ListConvertedInt[1], ListConvertedInt[2]));
                if (!DictAdjcentIndex.ContainsKey(ListConvertedInt[0]))
                {
                    DictAdjcentIndex[ListConvertedInt[0]] = new List<int>();
                }
                DictAdjcentIndex[ListConvertedInt[0]].Add(ListConvertedInt[2]);
            }
        }
    }

    public void MyStart()
    {
        foreach (GameObject tileObject in ListTileObjects)
        {
            BaseTile tempTile = tileObject.GetComponent<BaseTile>();
            tempTile.SetIndex();
            ListBaseTiles.Add(tempTile);
        }
    }
    
    public void MyUpdate()
    {

    }

    public BaseTile GetBaseTile(int iIndex)
    {
        return ListBaseTiles[iIndex];
    }

    public int GetCurrentPos()
    {
        return iCurrentPos;
    }
    public void SetCurrentPos(int iTarget)
    {
        iCurrentPos = iTarget;
    }

    public void ChangeBaseTile(int iIndex, TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Default:
                {
                    ListBaseTiles[iIndex].SetTileType(tileType);
                    ListBaseTiles[iIndex].GetComponent<SpriteRenderer>().color = Colors.baseTileColor;
                    break;
                }
        }
    }

    public List<int> GetAdjList(int iIndex)
    {
        return DictAdjcentIndex[iIndex];
    }

    public List<(int,int,int)> GetListEdge(int iIndex)//, int iDirection)
    {
        List<(int, int, int)> result = new List<(int, int, int)>();
        foreach((int,int,int) tuple in ListEdge)
        {
            if (tuple.Item1 == iIndex)// && tuple.Item2 == iDirection)
                result.Add(tuple);
        }

        return result;
    }

    public List<int> GetShootingPath(int iStartIndex, int iDirection)
    {
        int iNextTile = GetAdjTileByDirection(iStartIndex, iDirection);
        if (iNextTile == -1)
            return new List<int>();

        List<int> path = new List<int>();
        while (iNextTile != -1)
        {
            path.Add(iNextTile);
            if (GetBaseTile(iNextTile).GetTileType() == TileType.Goal ||
                GetBaseTile(iNextTile).GetTileType() == TileType.Wall)
            {
                break;
            }

            iNextTile = GetAdjTileByDirection(iNextTile, iDirection);          
        }

        return path;
    }

    public int GetAdjTileByDirection(int iStartIndex, int iDirection)
    {
        foreach((int,int,int) info in ListEdge)
        {
            if(info.Item1 == iStartIndex && info.Item2 == iDirection)
            {
                return info.Item3;
            }
        }

        return -1;
    }    

    public void DestroyObstacleTile(List<int> positions)
    {
        foreach(int iPosition in positions)
        {
            ChangeBaseTile(iPosition, TileType.Default);
        }
    }
}
