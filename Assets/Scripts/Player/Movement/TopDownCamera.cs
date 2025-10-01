using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCamera : MonoBehaviour {
  [Header("References")]
  public Transform player;

  [Header("Settings")]
  public float distance = 10f;
  private float _defaultDistance;
  private float _defaultHeight;
  public float height = 5f;
  public float rotationSpeed = 100f;
  public bool rotateCameraEnabled;

  private float _yaw = 0f;
  private float _pitch = 45f;
  
  private Vector2 _lookInput;

  private void Start() {
    _defaultHeight = height;
    _defaultDistance = distance;
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update() {
    _yaw += _lookInput.x * rotationSpeed * Time.deltaTime;
    Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
    Vector3 offset = rotation * new Vector3(0, 0, -distance);
    
    offset.y += height;

    var position = player.position;
    transform.position = position + offset;
    transform.LookAt(position);
  }

  public void OnZoom(InputAction.CallbackContext context) {
    height += context.ReadValue<float>() / 2;
    distance += context.ReadValue<float>() / 2;
  }

  public void ResetZoom() {
    height = _defaultHeight;
    distance = _defaultDistance;
  }

  public void OnRotate(InputAction.CallbackContext context) {
    if (rotateCameraEnabled) {
      _lookInput.x = context.ReadValue<float>();
    }
  }
  private void OnCollisionEnter(Collision col) {
  }
}