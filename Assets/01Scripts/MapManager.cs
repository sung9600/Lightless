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

    public void ChangeBaseTile(int iIndex, BaseTile baseTile)
    {
        ListBaseTiles[iIndex] = baseTile;
    }

    public List<int> GetAdjList(int iIndex)
    {
        return DictAdjcentIndex[iIndex];
    }
}
