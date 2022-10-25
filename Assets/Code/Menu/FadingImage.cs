using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadingImage : MonoBehaviour
{
    public static event Action OnFullAlpha;

    private float _str;
    private float _alpha;

    private Image _image;

    void Awake()
    {
        _str = -0.35f;
        _alpha = 0.99f;
        _image = GetComponent<Image>();
    }

    public void StartAlpha()
    {
        _alpha = 0.0f;
        _str = Mathf.Abs(_str);
    }

    void Update()
    {
        _alpha += _str * Time.deltaTime;
        _alpha = Mathf.Clamp(_alpha, 0.0f, 1.0f);
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _alpha);

        if (_alpha >= 1.0f)
        {
            _str *= -1;
            OnFullAlpha?.Invoke();
        }
        if (_alpha <= 0.0f) this.gameObject.SetActive(false);
    }
}
