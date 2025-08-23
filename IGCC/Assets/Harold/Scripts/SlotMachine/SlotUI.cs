using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class SlotUI : MonoBehaviour
{
    [SerializeField]
    RectTransform _slotUIContainer;

    public List<Image> _images = new List<Image>();

    float _imageSize;

    public void Awake()
    {
        _images = _slotUIContainer.GetComponentsInChildren<Image>().ToList();
        _imageSize = _slotUIContainer.rect.height;
    }

    public void spin(int total){
        _slotUIContainer.transform.DOMoveY(total*_imageSize, 0.5f);
    }

    public void setImages(List<Item> items)
    {
        int count = items.Count;
        int currentIndex = 0;
        
        for(int i=0; i <_images.Count; i++)
        {
            currentIndex %= count;
            _images[i].sprite = items[currentIndex].Sprite;
            currentIndex++;
        }
    }

}
