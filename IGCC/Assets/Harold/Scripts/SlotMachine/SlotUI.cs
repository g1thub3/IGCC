using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class SlotUI : MonoBehaviour
{
    [SerializeField]
    RectTransform _slotUIContainer;

    public List<Image> _images = new List<Image>();

    public void Awake()
    {
        _images = _slotUIContainer.GetComponentsInChildren<Image>().ToList();
    }

    public void setImages()
    {

    }

}
