using UnityEngine;

public class BananaPickUp : MonoBehaviour
{
    [SerializeField]
    int _bananaPickUpValue=1;

    [SerializeField]
    GameObject _pickUpParticles;

    public void OnTriggerEnter(Collider other)
    {
        //Get the parent (character handler)
        Inventory inventory = other.transform.parent.GetComponent<Inventory>();

        //Change bananas by a value
        if (inventory)
        {
            inventory.changeBananasBy(_bananaPickUpValue);
            Instantiate(_pickUpParticles, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySFXOneShot("sfx_coin");
            Destroy(gameObject);
        }
    }
}
