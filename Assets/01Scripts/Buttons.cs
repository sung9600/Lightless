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
        Inventory,
        Inventory_Gem
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

    public void AimOnOff()
    {
        if (StageManager.GetInstance().GetInventory().Count == 0)
            return;

        StageState stageState = StageManager.GetInstance().GetStageState();
        if (stageState == StageState.Default && StageManager.GetInstance().GetInventory().Count > 0)
        {
            gameObject.GetComponent<Image>().color = Colors.playerTileColor;
            StageManager.GetInstance().SetStageState(StageState.Aiming);
        }
        else if (stageState == StageState.Aiming)
        {
            gameObject.GetComponent<Image>().color = Colors.white;
            StageManager.GetInstance().SetStageState(StageState.Default);
        }
    }

    public void Inventory()
    {
        StageManager.GetInstance().ShowHideInventory();
    }
}
