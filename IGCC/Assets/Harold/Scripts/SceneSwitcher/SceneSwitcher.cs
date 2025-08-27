using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    SceneEventHandler _sceneEventHandler;

    [SerializeField]
    FadingTransition _fadeTransition;

    static SceneSwitcher _instance;
    public static SceneSwitcher Instance => _instance;

    public void CloseGame()
    {
        Application.Quit();
    }
    public void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);
    }

    //public void Awake()
    //{
    //    _sceneEventHandler.OnSceneLoadedEvent += (x,y)=> {
    //        _fadeTransition.onSceneOpen();
    //    };
    //}

    public void goToScene(string scene,Color color)
    {
        _fadeTransition.FadeOutAndRun(() => _sceneEventHandler.ChangeScene(scene),color);
        //_sceneEventHandler.ChangeScene(scene);
    }

    public void goToScene(string scene)
    {
        _fadeTransition.FadeOutAndRun(() => _sceneEventHandler.ChangeScene(scene), Color.black);
        //_sceneEventHandler.ChangeScene(scene);
    }
}
