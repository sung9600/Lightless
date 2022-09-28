using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryButton : Buttons, IPointerDownHandler
{
    [SerializeField]
    private int InventoryNumber = 0;
    private static int SelectedIndex = -1;

    [SerializeField]
    private GemType gem = GemType.Null;

    public void SetGem(GemType gem)
    {
        this.gem = gem;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(SelectedIndex == InventoryNumber)
        {
            GetComponent<Image>().color = Colors.white;
            SelectedIndex = -1;
        }
        else
        {
            if(SelectedIndex >= 0)
            {
                // selectedIndex의 색 바꿔야함
                StageManager.GetInstance().InventorySquares[SelectedIndex].GetComponent<Image>().color = Colors.white;
            }
            SelectedIndex = InventoryNumber;
            StageManager.GetInstance().SetSelectedGem(gem);
            GetComponent<Image>().color = Colors.candidTileColor;
        }
    }
}
