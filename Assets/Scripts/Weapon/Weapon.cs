using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum UpgradeType {
  UNIQUE,
  ADDITIVE
}


public class Weapon : MonoBehaviour {
  public delegate void OnShoot();
  public event OnShoot ShootEvent;

  private bool _canShoot;
  public int delay;
  public float currentDamageMultiplier;

  
  private LineRenderer _line;
  public float lineLength;
  private void Start() {
    _line = GetComponent<LineRenderer>();
    StartCoroutine(ShootDelay());
  }

  void Update() {
    Vector3 target = transform.position + transform.forward * lineLength;
    _line.SetPosition(0, transform.position);
    _line.SetPosition(1, transform.position);
  }
  
  public void Shoot(InputAction.CallbackContext context) {
    if (!_canShoot) return;
    ShootEvent?.Invoke();
    _canShoot = false;
  }
  
  public void AddUpgrade(Upgrade upgrade) {
    ShootEvent += upgrade.Shoot;
  }

  private IEnumerator ShootDelay() {
    while (Application.isPlaying) {
      yield return new WaitUntil(() => !_canShoot);
      yield return new WaitForSeconds(delay);
      _canShoot = true;
    }
  }
}
