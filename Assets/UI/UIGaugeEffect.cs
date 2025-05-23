using UnityEngine;
using UnityEngine.UI;

public class UIGaugeEffect : MonoBehaviour
{
    [SerializeField] private SlicedFilledImage _guage;
    [SerializeField] private Image _lightEffect;
    [SerializeField] private float _frequency = 1f;
    private Color _color;
    private Color _waitingColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _waitingColor = new Color(0, 0, 0, 0);
        _color = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_guage.fillAmount != 1)
        {
            _lightEffect.color = _waitingColor;
            return;
        }
        _color.a = (Mathf.Sin(Time.time * _frequency * Mathf.PI) + 1f) / 2f;
        _lightEffect.color = _color;
    }
}
