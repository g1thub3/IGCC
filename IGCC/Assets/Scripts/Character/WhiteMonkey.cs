using UnityEngine;
using UnityEngine.InputSystem;

public class WhiteMonkey : Monkey
{
    private Transform _climbing;
    public bool IsClimbing
    {
        get {  return _climbing != null; }
    }
    protected new void Start()
    {
        base.Start();
        index = 0;
        _climbing = null;
    }

    public Transform GetClimbable()
    {
        Transform foundObj = null;
        var colliders = Physics.OverlapSphere(transform.position, _interactHitbox.radius, LayerMask.GetMask("Climbable"));
        float closest = 999.0f;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == gameObject) continue;
            float dist = (transform.position - colliders[i].transform.position).magnitude;
            if (dist < closest)
            {
                closest = dist;
                foundObj = colliders[i].transform;
            }
        }
        return foundObj;
    }

    private void EnterClimb(Transform foundClimb)
    {
        _climbing = foundClimb;
        // Take the y
        // get its point in the lerp
        // transpose it into the progression
        // begin the climb
        // anchor the position
    }
    private void ExitClimb()
    {
        _climbing = null;
    }


    public override void Controls(PlayerInput input)
    {
        base.Controls(input);
        if (input.actions["Special"].WasPressedThisFrame())
        {
            if (IsClimbing)
            {
                var find = GetClimbable();
                if (find != null) {
                    EnterClimb(find);
                }
            } else
            {
                ExitClimb();
            }
        }
    }
}
