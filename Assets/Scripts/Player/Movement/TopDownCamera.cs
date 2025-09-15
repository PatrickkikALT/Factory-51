using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCamera : MonoBehaviour {
  public Transform player;
  public Vector3 offset;
  public float minZoom, maxZoom;

  private void Start() {
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update() {
    transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, player.position.z + offset.z);
    // transform.LookAt(player);
  }

  public void OnZoom(InputAction.CallbackContext ctx) {
    offset.z -= ctx.ReadValue<float>();
    offset.z = Mathf.Clamp(offset.z, maxZoom, minZoom);
  }
}
