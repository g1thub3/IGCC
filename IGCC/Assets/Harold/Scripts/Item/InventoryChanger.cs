using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Item/InventoryChanger")]
public class InventoryChanger : Item {
    [SerializeField]
    float _bananaChangeValue;
    [SerializeField]
    float _livesChangeValue;
    public override void onObtained(Inventory inventory) {
        inventory.changeBananasBy(_bananaChangeValue);
        inventory.changeLivesBy(_livesChangeValue);
    }
}
