using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlotMachine : MonoBehaviour
{
    [SerializeField]
    private List<Item> _itemList = new List<Item>();
    [SerializeField]
    private List<SlotUI> _slotUI;

    public void Start()
    {
        for(int i=0; i < _itemList.Count; i++)
        {
            _slotUI[i].setImages(_itemList);
        }
    }


}
