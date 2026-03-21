using UnityEngine;
using UnityEngine.UI;

public class SanityFilter : MonoBehaviour
{
    [SerializeField] private PlayerSanity _playerSanity;
    [SerializeField] private Image _overlay; // full-screen Image (Raycast Target = false)
    [SerializeField] private float _fadeSpeed = 3f;

    [Header("State Colors (alpha will be used)")]
    [SerializeField] private Color _saneColor = new Color(0, 0, 0, 0); // Transparent
    [SerializeField] private Color _shakenColor = new Color(1, 0, 0, 1); // FULL BRIGHT RED
    [SerializeField] private Color _disturbedColor = new Color(0, 1, 0, 1); // FULL BRIGHT GREEN
    [SerializeField] private Color _insaneColor = new Color(0, 0, 1, 1); // FULL BRIGHT BLUE

    private bool lowPassStart = false;
    private Color _targetColor;

    void Start()
    {
        if (_playerSanity == null)
        {
        _playerSanity = GameObject.FindAnyObjectByType<PlayerSanity>();
        }

        if (_overlay != null) _overlay.color = _saneColor;
    }

    void Update()
    {
      if (_playerSanity == null) {
        _playerSanity = FindAnyObjectByType<PlayerSanity>();
        return; // Skip this frame until we find it
    }

    int curSanity = _playerSanity.GetState(); 
    switch (curSanity)
    {
        case 0: _targetColor = _saneColor; break;
        case 1: _targetColor = _shakenColor; break;
        case 2: _targetColor = _disturbedColor; break;
        default: _targetColor = _insaneColor; break;
    }

    if (_overlay != null){
        _overlay.color = Color.Lerp(_overlay.color, _targetColor, _fadeSpeed * Time.deltaTime);
    } else {
        Debug.LogError("FILTER: Overlay Image is missing!");
    }
    }
}
