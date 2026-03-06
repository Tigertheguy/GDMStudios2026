using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SanityUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerSanity _playerSanity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _slider.maxValue = _playerSanity.MaxSanity;
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = Mathf.Lerp(_slider.value, _playerSanity.GetComponentInParent<PlayerSanity>().GetSanity(), 5f * Time.deltaTime); 
    }
}
