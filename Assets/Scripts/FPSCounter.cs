using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
  public TextMeshProUGUI fpsText;

  public float updateInterval = 0.5f;

  private float _accum = 0f;
  private int _frames = 0;
  private float _timeLeft;
  private float _fps = 0f;

  void Start() {
    _timeLeft = updateInterval;
  }

  void Update() {
    _timeLeft -= Time.unscaledDeltaTime;
    _accum += 1f / Mathf.Max(0.00001f, Time.unscaledDeltaTime);
    _frames++;

    if (_timeLeft <= 0f) {
      _fps = _accum / _frames;
      _timeLeft = updateInterval;
      _accum = 0f;
      _frames = 0;
      fpsText.text = $"FPS: {_fps:F1}";
    }
  }
}