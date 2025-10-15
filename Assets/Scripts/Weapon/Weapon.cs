using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum UpgradeType {
  UNIQUE,
  ADDITIVE
}


public class Weapon : MonoBehaviour {
  public delegate void OnShoot();

  public float delay;
  public float currentDamageMultiplier;
  
  public Transform shootPos;
  private bool _canShoot = true;
  private LineRenderer _line;

  public Animator animator;
  private bool _isFiring;
  public AnimationClip shootClip;
  
  private void Start() {
    _line = GetComponent<LineRenderer>();
    animator = GetComponent<Animator>();
  }

  // private void Update() {
  //   _line.SetPosition(0, shootPos.position);
  //   _line.SetPosition(1, shootPos.position + transform.forward * 6);
  // }

  public event OnShoot ShootEvent;


  public void Shoot(InputAction.CallbackContext context) {
    if (context.performed || context.started) {
      _isFiring = true;
      StartCoroutine(AutoFire());
    } else if (context.canceled) {
      animator.SetTrigger("StopShooting");
      _isFiring = false;
    }
  }

  private IEnumerator AutoFire() {
    while (_isFiring) {
      if (_canShoot) {
        animator.speed = (shootClip.length / delay);
        animator.Play("Shooting", 0, 0f);
        ShootEvent?.Invoke();
        _canShoot = false;
        yield return new WaitForSeconds(delay);
        animator.speed = 1;
        _canShoot = true;
      } else {
        yield return null;
      }
    }
  }
  
  public void AddUpgrade(Upgrade upgrade) {
    ShootEvent += upgrade.Shoot;
  }
  
}