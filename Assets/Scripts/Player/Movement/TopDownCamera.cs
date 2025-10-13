using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCamera : MonoBehaviour {
  [Header("References")]
  public Transform player;

  private Movement _playerMovement;
  [Header("Settings")]
  public float distance = 10f;
  private float _defaultDistance;
  private float _defaultHeight;
  public float height = 5f;
  
  [Header("Zoom")]
  public float maxHeight;
  public float maxDistance;
  public float minHeight;
  public float minDistance;
  
  [Header("Rotate")]
  public float rotationSpeed = 10f;
  public bool rotateCameraEnabled;
  
  private void Start() {
    _defaultHeight = height;
    _defaultDistance = distance;
    Cursor.lockState = CursorLockMode.Locked;
    player = GameManager.Instance.player;
    _playerMovement = player.GetComponent<Movement>();
  }

  private void Update() {
    Quaternion rotation = _playerMovement.bodyBone.rotation;
    Vector3 offset = rotation * new Vector3(0, 0, -distance);
    
    offset.y += height;

    var position = player.position;
    transform.position = position + offset;
    transform.LookAt(position);
  }

  public void OnZoom(InputAction.CallbackContext context) {
    height += context.ReadValue<float>() / 10;
    distance += context.ReadValue<float>() / 10;
    height = Mathf.Clamp(height, minHeight, maxHeight);
    distance = Mathf.Clamp(distance, minDistance, maxDistance);
  }

  public void ResetZoom() {
    height = _defaultHeight;
    distance = _defaultDistance;
  }


}