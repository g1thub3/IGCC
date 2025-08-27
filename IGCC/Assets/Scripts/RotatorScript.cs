using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    [SerializeField] float _angle = 180.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,_angle * Time.deltaTime,0));
    }
}
