using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum UpgradeType {
  UNIQUE,
  ADDITIVE
}


public class Weapon : MonoBehaviour {
  public delegate void OnShoot();

  public int delay;
  public float currentDamageMultiplier;

  private bool _canShoot;
  private LineRenderer _line;

  private void Start() {
    // _line = GetComponent<LineRenderer>();
    StartCoroutine(ShootDelay());
  }

  // private void Update() {
  //   _line.SetPosition(0, transform.position);
  //   _line.SetPosition(1, transform.position + transform.forward * 10);
  // }

  public event OnShoot ShootEvent;


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