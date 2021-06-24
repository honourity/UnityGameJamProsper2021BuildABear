using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Button _button;
    public Text _label;

    private Slider _slider;

    private AudioSource _audioSource;
    private float _startingVolume;
    private Coroutine _fadingCoroutine;
    private bool _audioPlaying;
    private bool _notched;
    private float _sliderStartingValue;
    private float _sliderStartingMinValue;
    private float _sliderStartingMaxValue;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _audioSource = GetComponent<AudioSource>();
        _startingVolume = _audioSource.volume;

        _sliderStartingValue = _slider.value;
        _sliderStartingMinValue = _slider.minValue;
        _sliderStartingMaxValue = _slider.maxValue;
    }

    public void SetNotched(bool notched, int? num)
    {
        if (notched)
        {
            _notched = true;
            _label.text = "Shape";
            _slider.wholeNumbers = true;
            _slider.value = 0;
            _slider.minValue = 0;
            _slider.maxValue = num.Value - 1;
        }
        else
        {
            _notched = false;
            _label.text = "Size";
            _slider.wholeNumbers = false;
            _slider.value = _sliderStartingValue;
            _slider.minValue = _sliderStartingMinValue;
            _slider.maxValue = _sliderStartingMaxValue;
        }
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

        if (!_notched)
        {
            _audioSource.Stop();
            _audioPlaying = false;
        }
    }

    public void Activate(UnityAction valueChangedEventListener)
    {
        _slider.onValueChanged.RemoveAllListeners();

        _slider.value = 0.5f;

        _button.gameObject.SetActive(true);
        _slider.gameObject.SetActive(true);
        _slider.onValueChanged.AddListener(delegate { valueChangedEventListener(); });

        if (!_notched)
        {
            _slider.onValueChanged.AddListener(delegate { SetPitch(); });
        }
    }

    public float GetValue()
    {
        return _slider.value;
    }

    public void SetPitch()
    {
        if (!_audioPlaying)
        {
            _audioSource.Play();
            _audioPlaying = true;
        }
        
        _audioSource.pitch = 1f / (_slider.value + 0.5f);

        _audioSource.volume = _startingVolume;

        if (_fadingCoroutine == null)
        {
            _fadingCoroutine = StartCoroutine(AudioFadeCoroutine());
        }
    }

    public IEnumerator AudioFadeCoroutine()
    {
        while (_audioSource.volume > 0f)
        {
            _audioSource.volume -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        _fadingCoroutine = null;
    }
}
