using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePortal : MonoBehaviour
{
    //Room to go to
    [SerializeField]
    string _sceneToGoTo;

    private void OnTriggerEnter(Collider other)
    {
        SceneSwitcher.Instance.goToScene(_sceneToGoTo);
    }

}
