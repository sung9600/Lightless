using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private static StageManager SM_Instance;
    [SerializeField]
    private MapManager Map_Instance = null;
    [SerializeField]
    private StageState eStageState = StageState.Tutorial;
    [SerializeField]
    private List<GemType> inventory = new List<GemType>();

    public delegate void AfterMove();
    public AfterMove Del_AfterMove;
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

    private void Awake()
    {
        if (SM_Instance == null)
            SM_Instance = this;

        Map_Instance.MyAwake();
        Del_AfterMove = AfterMoveCallback;
    }
    private void Start()
    {
        Map_Instance.MyStart();
    }
    private void Update()
    {
        Map_Instance.MyUpdate();
    }


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
    public void AddGemToInventory(GemType gem)
    {
        inventory.Add(gem);
    }

    void AfterMoveCallback()
    {
        int CurrentPlayerPos = MapManager.GetInstance().GetCurrentPos();
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
}
