using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
  [Header("Movement")]
  public float moveSpeed = 5f;
  public float dashSpeed = 15f;
  public float dashDuration = 0.2f;
  public float dashCooldown = 1f;

  [Header("Mouse Movement")]
  public float mouseSensitivity = 10f;
  
  private Rigidbody _rb;
  private Vector3 _moveInput;
  private bool _isDashing = false;
  private float _dashTime = 0f;
  private float _lastDash = -Mathf.Infinity;
  
  private void Awake() {
    _rb = transform.parent.GetComponent<Rigidbody>();
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
  }
  

  public void OnMove(InputAction.CallbackContext context) {
    Vector2 input = context.ReadValue<Vector2>();
    _moveInput = new Vector3(input.x, 0f, input.y);
    //prevent sliding with no input
  }

  public void OnDash(InputAction.CallbackContext context) {
    if (context.performed && Time.time >= _lastDash + dashCooldown) {
      _isDashing = true;
      _dashTime = dashDuration;
      _lastDash = Time.time;
    }
  }

  public void OnLook(InputAction.CallbackContext context) {
    if (context.performed) {
      Vector2 input = context.ReadValue<Vector2>();
      transform.Rotate(0, input.x * Time.deltaTime * mouseSensitivity, 0, Space.Self);
    }
  }
}