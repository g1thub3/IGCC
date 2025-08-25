using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System.Collections;

public class SlotUI : MonoBehaviour
{
    [SerializeField]
    RectTransform _slotUIContainer;


    private List<Item> _itemList = new List<Item>();

    public List<Image> _images = new List<Image>();

    float _imageSize;

    Vector3 _iniPos;

    bool _isSpinning=false;
    public bool IsSpinning => _isSpinning;

    public event System.Action onSpinCompleteEvent;

    public void Awake()
    {
        _images = _slotUIContainer.GetComponentsInChildren<Image>().ToList();
        _imageSize = 200;
        _iniPos = _slotUIContainer.position;
    }

    public void spin(int total)
    {
        if (_isSpinning)
            return;

        _slotUIContainer.transform.DOMove(_iniPos,0.5f).onComplete += () => StartCoroutine(SpinLoop(total));
    }

    public void setImages(List<Item> items)
    {
        _itemList = items;
        int count = items.Count;
        int currentIndex = 0;

        for (int i = 0; i < _images.Count; i++)
        {
            currentIndex %= count;
            _images[i].sprite = items[currentIndex].Sprite;
            currentIndex++;
        }
        //Vector3 curSlot = _slotUIContainer.position;
        //Vector3 nextSlot = new Vector3(0, _imageSize, 0) * 0.5f;

        //_slotUIContainer.position = (curSlot + nextSlot);

    }
    IEnumerator SpinLoop(int total)
    {
        Vector3 iniPos = _slotUIContainer.position;
        int curIndex = 0;
        int totalSpins = 0;
        _isSpinning = true;

        while (true)
        {
            curIndex++;
            totalSpins++;


            if (totalSpins > total)
            {
                _isSpinning=false;
                onSpinCompleteEvent?.Invoke();
                yield break;
            }


            Vector3 curSlot = _slotUIContainer.position;
            Vector3 nextSlot = iniPos + new Vector3(0, _imageSize, 0) * curIndex;

            float timer = 0f;
            float dur=0.05f;

            //if (curIndex == _images.Count - _itemList.Count)
            //{
            //    nextSlot += new Vector3(0, _imageSize, 0) * 0.5f;
            //    _slotUIContainer.position = nextSlot;
            //}

            //Lerp to the next slot
            while (dur >= timer)
            {
                _slotUIContainer.position = Vector3.Lerp(curSlot, nextSlot, timer/dur);

                timer += Time.deltaTime;

                yield return null;
            }

            _slotUIContainer.position = nextSlot;

            //Images - Item List
            if (curIndex >= 3)
            {
                _slotUIContainer.position = iniPos;
            }


            //Use the modulus operator to limit the current index to the images 
            curIndex %= 3;

            yield return null;
        }
    }
}