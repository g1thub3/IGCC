using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomPortal : MonoBehaviour
{
    //Room to go to
    [SerializeField]
    PortalData _data;

    RoomData _roomStored;

    private void Awake()
    {
        _roomStored = _data.getRandomRoom();
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger();
    }

    public void Trigger()
    {
        RoomManager.Instance.goToNewRoom(_roomStored);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(RoomPortal))]
public class PortalEditor : Editor
{
    RoomPortal myTarget;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        myTarget = target as RoomPortal;
        if (GUILayout.Button("Trigger Portal"))
            myTarget.Trigger();
    }
}
#endif