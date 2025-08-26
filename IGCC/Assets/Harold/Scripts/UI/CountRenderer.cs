using TMPro;
using UnityEngine;

public class CountRenderer : MonoBehaviour
{
    [SerializeField]
    TMP_Text _count;

    public void setCount(float count)
    {
        _count.text = count.ToString();
    }
}
