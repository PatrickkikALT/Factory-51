using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCamera : MonoBehaviour {
  public Transform player;
  public Vector3 offset;
  public float minZoom, maxZoom;

  public bool collision;

  private void Start() {
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update() {
    if (!collision) {
      transform.position = player.position + offset;
    }
    else {
      var pos = player.position + offset;
      var coll = new Collider[4];
      var boxSize = new Vector3(1, 1, 1);
      var hitCount = Physics.OverlapBoxNonAlloc(pos, boxSize / 2, coll);
      if (hitCount == 0) collision = false;
    }
  }

  private void OnCollisionEnter(Collision col) {
    collision = true;
  }
  
  public void OnZoom(InputAction.CallbackContext ctx) {
    offset.x -= ctx.ReadValue<float>();
    offset.y -= ctx.ReadValue<float>();
    offset.z += ctx.ReadValue<float>();
    offset.x = Mathf.Clamp(offset.x, minZoom, maxZoom);
    offset.y = Mathf.Clamp(offset.y, minZoom, maxZoom);
    offset.z = Mathf.Clamp(offset.z, +minZoom, -maxZoom);
  }
}