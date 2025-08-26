using UnityEngine;
using UnityEngine.InputSystem;

public class WhiteMonkey : Monkey
{
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] LayerMask _hiddenLayer;
    private readonly float _distThreshold = 5.0f;
    private readonly float _minAlpha = 0.1f;
    private readonly float _changeRate = 2.5f;
    private float _desiredAlpha;
    protected new void Start()
    {
        base.Start();
        index = 0;
    }

    public Transform GetEnemies()
    {
        Transform foundObj = null;
        var colliders = Physics.OverlapSphere(transform.position, _interactHitbox.radius, _hiddenLayer);
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

    private void Update()
    {
        var enemy = GetEnemies();
        if (enemy != null)
        {
            float dist = (enemy.transform.position - transform.position).magnitude;
            _desiredAlpha = Mathf.Clamp(dist / _distThreshold, _minAlpha, 1.0f);
        }
        else
            _desiredAlpha = 1;

        float currAlpha = _sprite.color.a;
        if (currAlpha == _desiredAlpha)
            return;
        bool isLess = currAlpha < _desiredAlpha;
        currAlpha = Mathf.Clamp(currAlpha + ((isLess ? _changeRate : -_changeRate) * Time.deltaTime), isLess ? currAlpha : _desiredAlpha, isLess ? _desiredAlpha : currAlpha);
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, currAlpha);
    }

    public override void Controls(PlayerInput input)
    {
        base.Controls(input);

    }
}
