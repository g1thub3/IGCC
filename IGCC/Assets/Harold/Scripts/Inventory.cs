using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    float _lives;
    public float Lives => _lives;

    [SerializeField]
    float _bananas;
    public float Bananas => _bananas;

    float _iniLives;
    float _iniBananas;


    [SerializeField]
    CountRenderer _livesCountRenderer;
    [SerializeField]
    CountRenderer _bananasCountRenderer;


    public event System.Action<float> OnLivesChangedEvent;
    public event System.Action<float> OnBananasChangedEvent;

    public void Start()
    {
        OnLivesChangedEvent += _livesCountRenderer.setCount;
        OnLivesChangedEvent?.Invoke(_lives);

        OnBananasChangedEvent += _bananasCountRenderer.setCount;
        OnBananasChangedEvent?.Invoke(_bananas);

        _iniLives = _lives;
        _iniBananas = _bananas;
    }

    public void changeLivesBy(float value)
    {
        setLives(_lives + value);
    }

    public void changeBananasBy(float value)
    {
        setBananas(_bananas + value);
    }

    public void setLives(float lives)
    {
        _lives = lives;
        _lives = Mathf.Clamp(_lives, 0, Mathf.Infinity);
        OnLivesChangedEvent?.Invoke(_lives);
    }


    public void setBananas(float bananas)
    {
        _bananas = bananas;
        _bananas = Mathf.Clamp(_bananas, 0, Mathf.Infinity);
        OnBananasChangedEvent?.Invoke(_bananas);
    }

    public void resetInventory()
    {
        _bananas = _iniBananas;
        _lives =_iniLives;

        OnLivesChangedEvent?.Invoke(_lives);
        OnBananasChangedEvent?.Invoke(_bananas);
    }

}
