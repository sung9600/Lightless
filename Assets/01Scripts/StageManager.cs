using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class StageManager : MonoBehaviour
{
    private static StageManager SM_Instance;
    [SerializeField]
    private MapManager Map_Instance = null;
    [SerializeField]
    private StageState eStageState = StageState.Tutorial;
    [SerializeField]
    private List<GemType> inventory = new List<GemType>();
    [SerializeField]
    private GemType selectedGem = GemType.Null;

    public delegate void AfterMove();
    public AfterMove Del_AfterMove;
    public GameObject AimButton;
    public GameObject InventoryButton;
    public GameObject InventoryGameObject;
    public List<GameObject> InventorySquares = new List<GameObject>();
    private StageState prevState = StageState.Default;


    [SerializeField]
    private TextMeshProUGUI turnIndicator;
    [SerializeField]
    private int turnCount = 7;
    [SerializeField]
    private GameObject gameover = null;
    [SerializeField]
    private GameObject tutorial = null;
    [SerializeField]
    private GameObject PlayerLight = null;

    [SerializeField]
    private List<Obstacles> obstacles = new List<Obstacles>();

    private Dictionary<GemType, int> DictRequiredGems = new Dictionary<GemType, int>();

    private void Awake()
    {
        if (SM_Instance == null)
            SM_Instance = this;

        Map_Instance.MyAwake();
        Del_AfterMove = AfterMoveCallback;

        // 이부분 개선필요
        obstacles.Add(new Tree(1, new List<int>() { 4 }));
        obstacles.Add(new Tree(1, new List<int>() { 5 }));
        obstacles.Add(new Tree(1, new List<int>() { 8 }));
        obstacles.Add(new Tree(1, new List<int>() { 14 }));
        obstacles.Add(new Tree(1, new List<int>() { 16 }));
    }
    private void Start()
    {
        Map_Instance.MyStart();

        // For Test
        DictRequiredGems.Add(GemType.TempGem, 1);
    }
    private void Update()
    {
        Map_Instance.MyUpdate();
    }


    #region GetSet
    public static StageManager GetInstance()
    {
        return SM_Instance;
    }

    public StageState GetStageState()
    {
        return eStageState;
    }

    public void SetStageState(StageState input)
    {
        eStageState = input;
    }

    public List<GemType> GetInventory()
    {
        return inventory;
    }
    public Dictionary<GemType,int> GetRequiredGems()
    {
        return DictRequiredGems;
    }
    public void SetRequiredGems(GemType gem, int iCount)
    {
        DictRequiredGems[gem] = iCount;
    }
    public void ReduceRequiredGem(GemType gem)
    {
        DictRequiredGems[gem]--;
    }
    public void ReduceRequiredGem()
    {
        if(selectedGem != GemType.Null)
            DictRequiredGems[selectedGem]--;
    }
    public GemType GetSelectedGem()
    {
        return selectedGem;
    }
    public void SetSelectedGem(GemType gem)
    {
        selectedGem = gem;
    }
    public List<Obstacles> GetObstacles()
    {
        return obstacles;
    }
    public void SetObstacles(List<Obstacles> obstacles)
    {
        this.obstacles = obstacles;
    }
    #endregion
    public void AddGemToInventory(GemType gem)
    {
        inventory.Add(gem);
        InventorySquares[inventory.Count - 1].GetComponent<InventoryButton>().SetGem(gem);
    }

    void AfterMoveCallback()
    {
        int CurrentPlayerPos = MapManager.GetInstance().GetCurrentPos();

        if(PlayerLight != null)
            PlayerLight.transform.position = MapManager.GetInstance().GetBaseTile(CurrentPlayerPos).transform.position;

        turnIndicator.text = "Turn Remain : " + --turnCount;
        if(turnCount == 0 && eStageState != StageState.GameClear)
        {
            // gameover
            eStageState = StageState.GameOver;
            gameover.SetActive(true);
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void StartScene()
    {
        eStageState = StageState.Default;
        tutorial.SetActive(false);
    }

    public void ShowHideInventory()
    {
        if(eStageState == StageState.Inventory)
        {
            InventoryGameObject.SetActive(false);
            SetStageState(prevState);
        }
        else
        {
            prevState = eStageState;
            InventoryGameObject.SetActive(true);
            SetStageState(StageState.Inventory);
        }
    }
}
