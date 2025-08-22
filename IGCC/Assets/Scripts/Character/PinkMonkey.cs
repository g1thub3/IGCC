using UnityEngine;

public class PinkMonkey : Monkey
{
    protected new void Start()
    {
        base.Start();
        index = 2;
    }

    public override void OnSwitch()
    {
        base.OnSwitch();

        // TRIGGER STUFF

    }
}
