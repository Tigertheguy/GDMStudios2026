using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SanityUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerSanity _playerSanity;
    [SerializeField] private MusicSwitch _musicSwitch;

    [SerializeField] private Color _barFullColor = Color.white;
    [SerializeField] private Color _barLowColor = Color.red;

    [Header("Meditation UI")]
    [SerializeField] private TextMeshProUGUI _meditationText;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _exhaustedColor = Color.red;

    [Header("Flicker Settings")]
    [SerializeField] private float _flickerSpeed = 15f;
    [SerializeField] private float _minAlpha = 0.4f;
    private Image _sliderFillImage;

    [Header("Fadout Settings")]
    [SerializeField] private float _maxDark = 0.9f;
    [SerializeField] private float _minDark = 0.1f;
    [SerializeField] private float _fadeSpeed = 2f;
    [SerializeField] private Image _meditationOverlay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    

    void Start()
    {
        _slider.maxValue = _playerSanity.MaxSanity;
        //Get filled part as image to mess around with later
        _sliderFillImage = _slider.fillRect.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSanity = _playerSanity.GetSanity();
        _slider.value = Mathf.Lerp(_slider.value, currentSanity, 5f * Time.deltaTime);

        MeditationFlicker();
        MeditationCheck();
        UpdateBarColor(currentSanity);
        MeditationScreenFade();
    }

    private void UpdateBarColor(float currentSanity)
    {

        if (currentSanity <= _playerSanity.MaxSanity / 2f)
        {

            Color targetColor = _barLowColor;
            targetColor.a = _sliderFillImage.color.a;
            _sliderFillImage.color = targetColor;
        }
        else
        {
            Color targetColor = _barFullColor;
            targetColor.a = _sliderFillImage.color.a;
            _sliderFillImage.color = targetColor;
        }
    }
    private void MeditationFlicker()
    {
        if (_playerSanity.IsMeditating)
        {
            float dynamicFlickerSpeed = _flickerSpeed + (_playerSanity.MeditationTimer * 10f);
            float alpha = _minAlpha + Mathf.PingPong(Time.time * dynamicFlickerSpeed, 1 - _minAlpha);
            Color newColor = _sliderFillImage.color;
            newColor.a = alpha;
            _sliderFillImage.color = newColor;
        }
        //Reset solid color when not meditating
        else
        {
            Color newColor = _sliderFillImage.color;
            newColor.a = 1f;
            _sliderFillImage.color = newColor;
        }
    }
    private void MeditationCheck()
    {
        int count = _playerSanity.CurrentMeditationCount;
        int cap = _playerSanity.MeditationCap;

        if (count >= cap)
        {
            _meditationText.text = "No More Charges";
            _meditationText.color = _exhaustedColor;
        }
        else
        {
            _meditationText.text = "Meditating: " + count + "/" + cap;
            _meditationText.color = _normalColor;
        }
    }
    private void MeditationScreenFade()
    {
        if (_playerSanity.IsMeditating)
        {
            float currentAlpha = _meditationOverlay.color.a;
            float newAlpha = Mathf.Lerp(currentAlpha, _maxDark, _fadeSpeed * Time.deltaTime);

            Color c = _meditationOverlay.color;
            c.a = newAlpha;
            _meditationOverlay.color = c;
            _musicSwitch.StartLowPass();
        }
        else
        {
            float currentAlpha = _meditationOverlay.color.a;
            float newAlpha = Mathf.Lerp(currentAlpha, _minDark, _fadeSpeed * Time.deltaTime);

            Color c = _meditationOverlay.color;
            c.a = newAlpha;
            _meditationOverlay.color = c;
            _musicSwitch.ResetLowPass();
        }
    }
}
