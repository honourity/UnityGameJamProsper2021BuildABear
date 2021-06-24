using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Button _button;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        Deactivate();
    }

    public void Deactivate()
    {
        _slider.gameObject.SetActive(false);
        _button.gameObject.SetActive(false);
        _slider.onValueChanged.RemoveAllListeners();
    }

    public void Activate(UnityAction valueChangedEventListener)
    {
        _slider.onValueChanged.RemoveAllListeners();

        _slider.value = 0.5f;

        _button.gameObject.SetActive(true);
        _slider.gameObject.SetActive(true);
        _slider.onValueChanged.AddListener(delegate { valueChangedEventListener(); });
    }

    public float GetValue()
    {
        return _slider.value;
    }
}
