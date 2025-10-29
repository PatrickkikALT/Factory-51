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
  
  public LayerMask masks;
  
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

    Vector3 position = player.position + offset;
    //
    // if (Physics.Linecast(player.position, position, out RaycastHit hit, masks, QueryTriggerInteraction.Ignore)) {
    //   transform.position = hit.point + hit.normal * 0.2f;
    // } else {
    transform.position = position;
    // }

    transform.LookAt(player.position);
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