using UnityEngine;

public class Transition
{
    public float t;
    public float max;

    public Transition(float newMax = 1.0f)
    {
        t = 0;
        max = newMax;
    }
    public float Progression
    {
        get
        {
            if (max == 0) return 1;
            return t / max;
        }
        set
        {
            t = Mathf.Clamp(value, 0, 1);
        }
    }
    public void Progress()
    {
        t = Mathf.Clamp(t + Time.deltaTime, 0, max);
    }
    public void Revert()
    {
        t = Mathf.Clamp(t - Time.deltaTime, 0, max);
    }
}
