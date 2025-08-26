using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(menuName = "SceneEventHandler")]
//Ensure that the scene has one SceneController enabled to ensure appropriate behaviour
public class SceneEventHandler : ScriptableObject
{
    public event System.Action<string> OnSceneChangeEvent;
    public event System.Action<Scene, LoadSceneMode> OnSceneLoadedEvent;


    //Call on enable when the scriptable object is first loaded onto the scene
    private void OnEnable()
    {
        //Dont destroy the scene controller when loading
        //DontDestroyOnLoad(gameObject);

        //Subscribe the on scene kiaded evebt
        SceneManager.sceneLoaded += OnSceneLoaded;
        //Debug.Log("subscribed");
    }

    //We remove the event when this scriptable object goes out of scope
    private void OnDisable()
    {
        //Dont destroy the scene controller when loading
        //DontDestroyOnLoad(gameObject);

        //Subscribe the on scene kiaded evebt
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    //Change scenes
    public void ChangeScene(string name)
    {
        //Call the on change scene event before we load the scene
        OnSceneChangeEvent?.Invoke(name);

        //Set all the subscribers to the two events to null
        OnSceneChangeEvent = null;
        OnSceneLoadedEvent = null;

        SceneManager.LoadScene(name);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Call the onSceneLoaded event
        OnSceneLoadedEvent?.Invoke(scene, mode);
    }
}
