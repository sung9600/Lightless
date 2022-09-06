using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager SM_Instance;
    [SerializeField]
    private MapManager Map_Instance = null;

    [SerializeField]
    private StageState eStageState = StageState.Default;

    private void Awake()
    {
        if (SM_Instance == null)
            SM_Instance = this;

        Map_Instance.MyAwake();
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

}
