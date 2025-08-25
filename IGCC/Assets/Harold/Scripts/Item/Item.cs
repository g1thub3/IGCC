using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    Sprite _sprite;
    public Sprite Sprite=>_sprite;

    [SerializeField]
    string _name;
    public string Name => _name;
}
