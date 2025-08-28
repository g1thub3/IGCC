using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    [SerializeField] float _angle = 180.0f;
    [SerializeField] AXIS _axis = AXIS.Y;
    public enum AXIS
    {
        X,Y,Z
    }

    // Update is called once per frame
    void Update()
    {
        switch (_axis)
        {
            case AXIS.X:
                transform.Rotate(new Vector3(_angle * Time.deltaTime, 0, 0));
                break;
            case AXIS.Y:
                transform.Rotate(new Vector3(0, _angle * Time.deltaTime, 0));
                break;
            case AXIS.Z:
                transform.Rotate(new Vector3(0, 0, _angle * Time.deltaTime));
                break;
        }
    }
}
