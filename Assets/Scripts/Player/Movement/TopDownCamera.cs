using UnityEngine;

public class TopDownCamera : MonoBehaviour {
  public Transform player;
  public Vector3 offset;
  private void Update() {
    transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, player.position.z + offset.z);
    transform.LookAt(player);
  }
}
