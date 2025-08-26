using UnityEngine;

public class BananaPickUp : MonoBehaviour
{
    [SerializeField]
    int _bananaPickUpValue=1;

    public void OnTriggerEnter(Collider other)
    {
        //Get the parent (character handler)
        Inventory inventory = other.transform.parent.GetComponent<Inventory>();

        //Change bananas by a value
        if (inventory)
        {
            inventory.changeBananasBy(_bananaPickUpValue);
            Destroy(gameObject);
        }
    }
}
