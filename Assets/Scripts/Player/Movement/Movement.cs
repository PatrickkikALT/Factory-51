using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour {
  [Header("References")] 
  public Transform cameraTransform;
  
  [Header("Movement")] 
  public float moveSpeed = 5f;
  public float dashSpeed = 15f;
  public float dashDuration = 0.2f;
  public float dashCooldown = 1f;

  [Header("Mouse Movement")] 
  public float mouseSensitivity = 10f;
  
  public Vector3 moveInput;
  private Vector3 _targetInput;
  private float _dashTime;
  private bool _isDashing;
  private float _lastDash = -Mathf.Infinity;

  private Rigidbody _rb;

  [Header("Bones")] 
  public Transform bodyBone;
  public Transform trackBone;
  public GameObject mesh;
  public Transform[] trackGears;
  
  [Header("Animation")]
  public Animator animator;
  private static int Moving = Animator.StringToHash("Moving");
  private static int Dead = Animator.StringToHash("Dies");

  private float _currentTrackSpeed;
  
  [Header("Cached Materials")]
  public Material trackMaterial;
  public float trackDashSpeed;

  private void Awake() {
    _rb = transform.GetComponent<Rigidbody>();
  }

  private void Start() {
    trackMaterial = mesh.GetComponent<Renderer>().materials[1];
  }

  private void FixedUpdate() {
    if (_isDashing) {
      _rb.linearVelocity = moveInput * dashSpeed;
      _dashTime -= Time.fixedDeltaTime;
      if (_dashTime <= 0)
        _isDashing = false;
    }
    else {
      _rb.linearVelocity = moveInput * moveSpeed;
    }
    
  }
  
  public void OnMove(InputAction.CallbackContext context) {
    var input = context.ReadValue<Vector2>();
    if (input == Vector2.zero) {
      trackMaterial.SetFloat("_ScrollSpeed", 0);
      _targetInput = Vector3.zero;
      animator.SetBool(Moving, false);
      return;
    }

    trackMaterial.SetFloat("_ScrollSpeed", _isDashing ? trackDashSpeed : 0.5f);
    animator.SetBool(Moving, true);
    Vector3 camForward = cameraTransform.forward;
    Vector3 camRight = cameraTransform.right;
    
    camForward.y = 0;
    camRight.y = 0;
    camForward.Normalize();
    camRight.Normalize();
    
    _targetInput = camForward * input.y + camRight * input.x;

  }

  private void Update() {
    moveInput = Vector3.Slerp(moveInput, _targetInput, Time.deltaTime * moveSpeed);
    
    if (moveInput != Vector3.zero && _targetInput != Vector3.zero && !_isDashing) {
      RotateGears();
      var targetRotation = Quaternion.LookRotation(moveInput);
      trackBone.rotation = targetRotation;
    }
  }

  public void RotateGears() {
    foreach (Transform t in trackGears) {
      t.Rotate(moveSpeed * 50 * Time.deltaTime, 0, 0);
    }
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
      var input = context.ReadValue<Vector2>();
      bodyBone.Rotate(0, input.x * Time.deltaTime * mouseSensitivity, 0, Space.Self);
      
    }
  }

  public void OnInteract(InputAction.CallbackContext context) {
    GameManager.Instance.playerPressedE = !context.canceled;
  }
}