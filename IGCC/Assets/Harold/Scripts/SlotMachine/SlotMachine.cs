using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlotMachine : MonoBehaviour
{
    [SerializeField]
    private List<Item> _itemList = new List<Item>();
    [SerializeField]
    private List<SlotUI> _slotUI;

    [SerializeField]
    GameObject _spinButton;

    [SerializeField]
    GameObject _claimButton;

    int _currentIndex = 0;

    public void Start()
    {
        for(int i=0; i < _slotUI.Count; i++)
        {
            _slotUI[i].setImages(_itemList);
            _slotUI[i].onSpinCompleteEvent += checkCanClaim;
        }
    }

    public void OnEnable()
    {
        _spinButton.SetActive(true);
        _claimButton.SetActive(false);
    }

    [ContextMenu("SpinWheel")]
    public void spin()
    {
        int randomRange = Random.Range(0, _itemList.Count);

        Debug.Log("Picked index: " + randomRange);

        _currentIndex = randomRange;
        
        //Bandaid fix bad do not do
        int[] spinCounts = { 2, 0, 1 }; // index = randomRange
        //int spinCount = spinCounts[randomRange] + (3*Random.Range(5,10));



        //Debug.Log("spint count: " + spinCount)/10;

        for (int i = 0; i < _slotUI.Count; i++)
        {
           int spinCount = spinCounts[randomRange] + (3 * Random.Range(3, 5) * (i+1));
            _slotUI[i].spin(spinCount);
        }

    }

    public void claimRewards()
    {

    }

    public void checkCanClaim()
    {

        for (int i = 0; i < _slotUI.Count; i++)
        {
            if (_slotUI[i].IsSpinning)
            {
                return;
            }
        }

        _spinButton.SetActive(false);
        _claimButton.SetActive(true);
    }
}
