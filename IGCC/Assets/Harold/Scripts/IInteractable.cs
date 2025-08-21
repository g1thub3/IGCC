using UnityEngine;

public interface IInteractable
{
    public abstract void onInteract(Transform player);
    public abstract void onEnterProximity(Transform player);
    public abstract void onExitProximity(Transform player);
}
