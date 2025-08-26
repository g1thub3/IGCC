using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    [HideInInspector] public string name;
    public AudioClip clip;
    public float volume = 1;
}

[CreateAssetMenu(fileName = "AudioManager", menuName = "Scriptable Objects/Audio Manager")]
public class AudioManager : SingletonScriptableObject<AudioManager>
{
    private Dictionary<string, AudioData> _sfxDictionary;
    [SerializeField] private List<AudioData> _sfxList;

    private Dictionary<string, AudioData> _bgmDictionary;
    [SerializeField] private List<AudioData> _bgmList;

    public void Refresh()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        _sfxDictionary = new Dictionary<string, AudioData>();
        if (_sfxList != null)
        {
            foreach (AudioData sfx in _sfxList)
            {
                sfx.name = sfx.clip.name;
                _sfxDictionary.Add(sfx.name, sfx);
            }
        }

        _bgmDictionary = new Dictionary<string, AudioData>();
        if (_bgmList != null)
        {
            foreach (AudioData sfx in _bgmList)
            {
                sfx.name = sfx.clip.name;
                _bgmDictionary.Add(sfx.name, sfx);
            }
        }
    }

    private AudioData GetSFX(string name)
    {
        if (_sfxDictionary == null) return null;
        return _sfxDictionary[name];
    }

    private AudioData GetBGM(string name)
    {
        if (_bgmDictionary == null) return null;
        return _bgmDictionary[name];
    }

    public void PlaySFXOneShot(string name, Vector3 pos)
    {
        AudioData audio = GetSFX(name);
        if (audio == null) return;
        AudioSource.PlayClipAtPoint(audio.clip, pos);
    }

    public void PlayBGM(string name)
    {
        AudioData audio = GetBGM(name);
        if (audio == null) return;
        Camera.main.GetComponent<AudioSource>().clip = audio.clip;
        Camera.main.GetComponent<AudioSource>().Play();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    AudioManager myTarget;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        myTarget = target as AudioManager;
        if (GUILayout.Button("Refresh"))
            myTarget.Refresh();
    }
}
#endif