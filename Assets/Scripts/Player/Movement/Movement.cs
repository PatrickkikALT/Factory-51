using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
  public float moveSpeed = 5f;
  public float dashSpeed = 15f;
  public float dashDuration = 0.2f;
  public float dashCooldown = 1f;

  private Rigidbody _rb;
  private Vector3 _moveInput;
  private bool _isDashing = false;
  private float _dashTime = 0f;
  private float _lastDash = -Mathf.Infinity;

  private void Awake() {
    _rb = GetComponent<Rigidbody>();
  }

  private void FixedUpdate() {
    if (_isDashing) {
      _rb.linearVelocity = _moveInput * dashSpeed;
      _dashTime -= Time.fixedDeltaTime;
      if (_dashTime <= 0)
        _isDashing = false;
    }
    else {
      _rb.linearVelocity = _moveInput * moveSpeed;
    }

    if (_moveInput != Vector3.zero && !_isDashing) {
      Quaternion targetRotation = Quaternion.LookRotation(_moveInput);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
    }
  }

  public void OnMove(InputAction.CallbackContext context) {
    Vector2 input = context.ReadValue<Vector2>();
    _moveInput = new Vector3(input.x, 0, input.y).normalized;
  }

  public void OnDash(InputAction.CallbackContext context) {
    if (context.performed && Time.time >= _lastDash + dashCooldown) {
      _isDashing = true;
      _dashTime = dashDuration;
      _lastDash = Time.time;
    }
  }

  public void OnShoot(InputAction.CallbackContext context) {
    throw new NotImplementedException();
  }
}