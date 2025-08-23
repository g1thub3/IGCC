using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    Sprite _sprite;
    public Sprite Sprite=>_sprite;

    [SerializeField]
    string _name;
    public string Name => _name;
}
