using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlotMachineUI : MonoBehaviour
{
    [SerializeField]
    Inventory _inventory;

    [SerializeField]
    private List<Item> _itemList = new List<Item>();
    [SerializeField]
    private List<SlotUI> _slotUI;

    [SerializeField]
    GameObject _spinButton;

    [SerializeField]
    GameObject _claimButton;

    [SerializeField]
    Canvas _slotMachineCanvas;

    private static SlotMachineUI _instance;
    public static SlotMachineUI Instance=>_instance;

    int _currentIndex = 0;

    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        _slotMachineCanvas.gameObject.SetActive(false);
    }

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

    public void enableSlotMachine()
    {
        _slotMachineCanvas.gameObject.SetActive(true);
        AudioManager.Instance.PlaySFXOneShot("sfx_switch");
    }

    [ContextMenu("SpinWheel")]
    public void spin()
    {
        int randomRange = Random.Range(0, _itemList.Count);

        //Debug.Log("Picked index: " + randomRange);

        _currentIndex = randomRange;
        
        //Bandaid fix bad do not do
        List<int> spinCounts = new List<int>(); // index = randomRange

        for(int i=0; i<_itemList.Count; i++)
        {
            if (i == 0)
            {
                spinCounts.Add(spinCounts.Count-1);
                continue;
            }

            spinCounts.Add(i-1);
        }

        //int spinCount = spinCounts[randomRange] + (3*Random.Range(5,10));
        //Debug.Log("spint count: " + spinCount)/10;

        for (int i = 0; i < _slotUI.Count; i++)
        {
           int spinCount = spinCounts[randomRange] + (_itemList.Count * Random.Range(3, 5) * (i+1));
            _slotUI[i].spin(spinCount);
        }

    }

    public void claimRewards()
    {
        _itemList[_currentIndex].onObtained(_inventory);
        AudioManager.Instance.PlaySFXOneShot("sfx_victory");
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

        AudioManager.Instance.PlaySFXOneShot("sfx_victory");


        _spinButton.SetActive(false);
        _claimButton.SetActive(true);
    }
}
