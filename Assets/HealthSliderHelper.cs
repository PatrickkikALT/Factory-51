using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthSliderHelper : MonoBehaviour {
  private Slider _slider;
  private int _target;
  public int speed = 1;

  private Image _fillImage;
  private void Start() {
    _slider = GetComponent<Slider>();
    _fillImage = _slider.fillRect.GetComponent<Image>();
    StartCoroutine(LerpValue());
  }

  public void SetValue(int newValue) {
    _target = newValue;
  }

  public void SetColor(Color color) {
    _fillImage.color = color;
  }

  private IEnumerator LerpValue() {
    while (true) {
      _slider.value = Mathf.Lerp(_slider.value, _target, speed * Time.deltaTime);
      yield return null;
    }
  }
}
