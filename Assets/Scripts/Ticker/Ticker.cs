using System;
using System.Collections;
using UnityEngine;

public class Ticker : MonoBehaviour {
  public delegate void OnTick();
  public event OnTick OnTickEvent;
  [SerializeField] private float tickTime;
  public bool tickPaused;

  public static Ticker Instance;

  private void Awake() {
    if (Instance is null) Instance = this;
    else Destroy(this);
  }

  private void Start() {
    StartCoroutine(Tick());
  }

  private IEnumerator Tick() {
    while (!tickPaused) {
      yield return new WaitForSeconds(tickTime);
      OnTickEvent?.Invoke();
    }

    if (tickPaused) {
      yield return new WaitUntil(() => !tickPaused);
      StartCoroutine(Tick());
      tickPaused = false;
    }
  }
}
