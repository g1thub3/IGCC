using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpriteAnimationController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    Coroutine _flickerCoroutine;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnEnable()
    {
        setToOpaque();
    }

    public void flipSpriteX(bool state)
    {
        _spriteRenderer.flipX = state;
    }

    public void toggleflipSpriteX()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    public void setFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

    public void setBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    public void setTrigger(string name)
    {
        _animator.SetTrigger(name);
    }


    public bool getBool(string name)
    {
        return _animator.GetBool(name);
    }

    public void setToTransparent()
    {
        Color tmp = _spriteRenderer.color;
        tmp.a = 0f;

        _spriteRenderer.color = tmp;
    }

    public void setToOpaque()
    {
        Color tmp = _spriteRenderer.color;
        tmp.a = 1f;

        _spriteRenderer.color = tmp;
    }

    public void setColor(Color color)
    {
        _spriteRenderer.color = color;
    }


    public void setToFlicker(float duration)
    {
        //Stop if already playing
        if (_flickerCoroutine != null)
        {

            setToOpaque();
            StopCoroutine(_flickerCoroutine);
        }
        _flickerCoroutine = StartCoroutine(flickerSprite(duration));
        //Debug.Log("Flickering");
    }


    IEnumerator flickerSprite(float duration)
    {
        //Debug.Log("flickered");
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        //Keep track of current time
        float timer = duration;

        while (timer > 0f)
        {
            setToTransparent();
            yield return waitTime;

            setToOpaque();
            yield return waitTime;

            //Off set by the 0.2f from waitTime

            //Debug.Log("flickering");
            timer -= Time.deltaTime + 0.2f;
        }

        setToOpaque();
    }

}
