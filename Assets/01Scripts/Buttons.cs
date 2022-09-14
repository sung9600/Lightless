using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buttons : MonoBehaviour, IPointerDownHandler
{
    enum buttonType
    {
        AimOnOff,
        Inventory
    }
    [SerializeField]
    private buttonType type;
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (type)
        {
            case buttonType.AimOnOff:
                {
                    AimOnOff();
                    break;
                }
            case buttonType.Inventory:
                {
                    Inventory();
                    break;
                }
        }
    }

    private void AimOnOff()
    {
        if (StageManager.GetInstance().GetInventory().Count == 0)
            return;

        StageState stageState = StageManager.GetInstance().GetStageState();
        if (stageState == StageState.Default && StageManager.GetInstance().GetInventory().Count > 0)
        {
            gameObject.GetComponent<Image>().color = Color.green;
            StageManager.GetInstance().SetStageState(StageState.Aiming);
        }
        else if (stageState == StageState.Aiming)
        {
            gameObject.GetComponent<Image>().color = Color.white;
            StageManager.GetInstance().SetStageState(StageState.Default);
        }
    }

    private void Inventory()
    {
        StageState stageState = StageManager.GetInstance().GetStageState();
        if (stageState == StageState.Default)
            StageManager.GetInstance().SetStageState(StageState.Inventory);
        else if (stageState == StageState.Inventory)
            StageManager.GetInstance().SetStageState(StageState.Default);
    }
}
